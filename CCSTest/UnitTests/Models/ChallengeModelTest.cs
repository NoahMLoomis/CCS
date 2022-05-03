using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCS.Controllers;
using CCS.Models;
using CTPTest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CCSTest.UnitTests.Models {
    public class ChallengeModelTest {
        private readonly TestSetup _setup;

        public ChallengeModelTest() {
            _setup = new();
        }

        [Fact]
        public void SortingChallengesByDifficultyDecending_ReturnsCorrectOrder() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            List<Challenge> unorderedChallenges = context.Challenge.ToList();
            List<Challenge> decendingChallenges = _challenge.SortChallengesByDifficultyLevelDecending(unorderedChallenges);

            Assert.True(decendingChallenges[0].DifficultyLevel.ToLower() == "hard");
            Assert.True(decendingChallenges[1].DifficultyLevel.ToLower() == "medium");
            Assert.True(decendingChallenges[2].DifficultyLevel.ToLower() == "easy");
        }

        [Fact]
        public void SetDifficultyLevelNumber_ReturnsCorrectOrder() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            List<Challenge> challenges = context.Challenge.ToList();

            // DifficultyLevelNumber is defaulted to 0
            foreach (var challenge in challenges) {
                Assert.True(challenge.DifficultyLevelNumber == 0);
            }

            // Sets each challenge with a difficulty Level
            _challenge.SetDifficultyLevelNumber(challenges);
            foreach (var challenge in challenges) {
                if (challenge.DifficultyLevel.ToLower() == "easy") {
                    Assert.True(challenge.DifficultyLevelNumber == 1);
                } else if (challenge.DifficultyLevel.ToLower() == "medium") {
                    Assert.True(challenge.DifficultyLevelNumber == 2);
                } else if (challenge.DifficultyLevel.ToLower() == "hard") {
                    Assert.True(challenge.DifficultyLevelNumber == 3);
                }
            }
        }

        [Fact]
        public async Task AddChallenge_AddChallengeLanguage_AddsSuccessfully() {
            using var context = _setup.CreateContext();

            int teacherId = (int)context.Teacher.FirstOrDefault().TeacherId;

            Challenge challenge = new(context) {
                ChallengeId = 99,
                Title = "Testing adding this challenge",
                Description = "This is a sample description",
                Example = "",
                FunctionName = "wutang()",
                TeacherId = teacherId,
                Active = true,
                DifficultyLevel = "Easy",
                CreationDate = DateTime.Today.AddDays(-1),
                ReturnTypeId = 1
            };

            //When debugging, it passes on this line. When not debugging, it breaks :(
            await challenge.AddChallenge(teacherId);

            Challenge result = await challenge.GetChallenge((int)-1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetStudentsAttemptingChallenge_ReturnsNumberOfStudentsAttemptingChallenge() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            int result = int.Parse(await _challenge.GetStudentsAttemptingChallenge(1));
            int expected = 2;

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetStudentsAttemptingChallenge_ChallengeDoesNotExist_ThrowsExeption() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            await Assert.ThrowsAsync<Exception>(async () => await _challenge.GetStudentsAttemptingChallenge(-1));

        }

        [Fact]
        public async Task GetLastTimeChallengeWasAttempted_ChallengeHasBeenAttempted_ReturnsCorrectLastTime() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            string result = await _challenge.GetLastTimeChallengeWasAttempted(1);
            string expected = DateTime.Today.ToString("MMMM d, yyyy");

            Assert.Equal(expected, result);
        }


        [Fact]
        public async Task GetLastTimeChallengeWasAttempted_ChallengeHasNotBeenAttempted_ReturnsErrorMessage() {
            using var context = _setup.CreateContext();

            // Removing any CodeSubmissions for any challenges with ChallengeId 3
            foreach (var sub in context.CodeSubmission) {
                if (sub.ChallengeLanguage.ChallengeId == 3) {
                    context.Remove(sub);
                }
            }
            await context.SaveChangesAsync();
            Challenge _challenge = new(context);

            string result = await _challenge.GetLastTimeChallengeWasAttempted(3);
            string expected = "This challenge has never been attempted";

            Assert.Equal(result, expected);
        }

        [Fact]
        public async Task GetLastTimeChallengeWasAttempted_ChallengeDoesNotExist_ThrowsException() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            await Assert.ThrowsAsync<Exception>(async () => await _challenge.GetLastTimeChallengeWasAttempted(-1));
        }

        [Fact]
        public async Task GetChallengeTitle_ReturnsChallengeTitle() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            string result = await _challenge.GetChallengeTitle(1);
            string expected = "Simple addition";

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetChallengeTitle_ChallengeDoesNotExist_ThrowsException() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            await Assert.ThrowsAsync<Exception>(async () => await _challenge.GetChallengeTitle(-1));
        }

        [Fact]
        public void GetChallengeLanguageNameList_ReturnsChallengeList() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            Assert.NotNull(_challenge.GetChallengeLanguageNameList());
        }

        [Fact]
        public async Task DeleteChallenge_ReturnsTrue() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            bool deleted = await _challenge.DeleteChallenge(1);

            Assert.True(deleted);
        }

        [Fact]
        public async Task DeleteChallenge_ChallengeDoesNotExist_ReturnsFalse() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);

            Assert.False(await _challenge.DeleteChallenge(-1));
        }

        [Fact]
        public async Task ToggleChallenge_TurnActiveOff() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            Challenge activeChallenge = await _challenge.GetChallenge(1);
            activeChallenge.ToggleChallenge();
            bool expected = false;
            bool result = activeChallenge.Active;
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task ToggleChallenge_TurnActiveOn() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            Challenge activeChallenge = await _challenge.GetChallenge(3);
            activeChallenge.ToggleChallenge();
            bool expected = true;
            bool result = activeChallenge.Active;
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetChallenge_ChallengeExists() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            Challenge challenge = await _challenge.GetChallenge(1);
            Assert.NotNull(challenge);
        }

        [Fact]
        public async Task GetChallenge_ChallengeDoesntExist() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            Challenge challenge = await _challenge.GetChallenge(0);
            Assert.Null(challenge);
        }

        [Fact]
        public async Task EditChallenge_ChallengeUpdates() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new(context);
            Challenge challenge = await _challenge.GetChallenge(1);
            challenge.Title = "Testing";
            await challenge.EditChallenge(challenge);
            Challenge updatedChallenge = await _challenge.GetChallenge(1);
            string expected = "Testing";
            string result = updatedChallenge.Title;
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task EditChallenge_ChallengeDoesntUpdate() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            Challenge challenge = await _challenge.GetChallenge(0);
            await Assert.ThrowsAsync<NullReferenceException>(() => challenge.EditChallenge(challenge));
        }

        [Fact]
        public void SanitizeChallenge_Caught() {
            Challenge challenge = new() {
                Title = "test \"/><script>StealCredentials()</script>",
                Description = "test",
                Example = "test",
                FunctionName = "test",
                TeacherId = 1,
                Active = true,
                DifficultyLevel = "Hard",
            };
            challenge.SanitizeChallenge();
            Assert.Equal("test \"/&gt;", challenge.Title);
        }

        [Fact]
        public void GetDifficultiesDropdown_DifficultiesComesBack() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            SelectList difficulties = _challenge.GetDifficultiesDropdown();
            Assert.NotNull(difficulties.Items);
        }

        [Fact]
        public async Task GetChallengeParameters_TestCaseDoesntExist() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await new Parameter().GetParametersForChallenge(0));
        }

        [Fact]
        public async Task GetChallengeParameters_TestCaseExist_ReturnsParameter() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            IEnumerable<Parameter> parameters = await new Parameter().GetParametersForChallenge(1);
            int expected = 2;
            int result = parameters.Count();
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task AllChallenges_ShowsActive() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            List<Challenge> challenge = await _challenge.AllChallenges(true);
            int expected = 2;
            int result = challenge.Count();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SortChallenges_SortAscendingDifficultyLevel() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            List<Challenge> unorderedChallenges = context.Challenge.ToList();
            List<Challenge> ascendingChallenges = _challenge.SortChallenges(unorderedChallenges, true);
            Assert.True(ascendingChallenges[0].DifficultyLevelOrderedAsce == false);
            Assert.True(ascendingChallenges[1].DifficultyLevelOrderedAsce == false);
            Assert.True(ascendingChallenges[2].DifficultyLevelOrderedAsce == false);
        }

        [Fact]
        public void SortChallenges_SortDescendingDifficultyLevel() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            List<Challenge> unorderedChallenges = context.Challenge.ToList();
            List<Challenge> descendingChallenges = _challenge.SortChallenges(unorderedChallenges, false);
            Assert.True(descendingChallenges[0].DifficultyLevelOrderedAsce == true);
            Assert.True(descendingChallenges[1].DifficultyLevelOrderedAsce == true);
            Assert.True(descendingChallenges[2].DifficultyLevelOrderedAsce == true);
        }

        [Fact]
        public async Task SortTeacherChallenge_TeacherDoesntExist() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            await Assert.ThrowsAsync<NullReferenceException>(() => _challenge.SortTeacherChallenges(0, true));
        }

        [Fact]
        public async Task SortTeacherChallenge_TeacherExists() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            Teacher teacherChallenge = await _challenge.SortTeacherChallenges(1, true);
            int expected = 3;
            int result = teacherChallenge.Challenge.Count();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SortChallengesByDifficultyLevel_SortIsCorrect() {
            using var context = _setup.CreateContext();
            Challenge _challenge = new Challenge(context);
            List<Challenge> unorderedChallenges = context.Challenge.ToList();
            List<Challenge> ascendingChallenges = _challenge.SortChallengesByDifficultyLevel(unorderedChallenges);
            Assert.True(ascendingChallenges[0].DifficultyLevel.ToLower() == "easy");
            Assert.True(ascendingChallenges[1].DifficultyLevel.ToLower() == "medium");
            Assert.True(ascendingChallenges[2].DifficultyLevel.ToLower() == "hard");
        }
    }
}
