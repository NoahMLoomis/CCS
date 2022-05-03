using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCS.Controllers;
using CCS.Models;
using CTPTest.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CCSTest.UnitTests.Controllers {
    public class ChallengeControllerTest {
        private readonly CCSContext _context;

        public ChallengeControllerTest() {
            _context = new CCSContext();
        }

        private decimal GetLatestChallengeId() {
            decimal latestId = _context.Challenge
                    .OrderByDescending(i => i.ChallengeId)
                    .First().ChallengeId;
            return latestId;
        }

        private async Task<Teacher> AddTeacher() {
            Teacher _teacher = new(_context);
            Teacher teacher = new();
            teacher.TeacherId = 1;
            await _teacher.AddTeacher(teacher);
            return await _teacher.GetTeacherById(1);
        }

        private async Task<Challenge> AddChallenge() {
            Challenge _challenge = new(_context);
            Challenge challenge = new Challenge {
                Title = "Testing",
                Description = "divide two numbers together",
                Example = "2 / 2 = 1",
                FunctionName = "simpleDivision()",
                TeacherId = 1,
                Active = false,
                DifficultyLevel = "Medium",
                CreationDate = DateTime.Today.AddDays(-1),
                ReturnTypeId = 1
            };
            await _context.AddAsync(challenge);
            await _context.SaveChangesAsync();
            decimal challengeId = GetLatestChallengeId();
            return await _challenge.GetChallenge(challengeId);
        }


        private Mock<ICurrentSession> CreateSession(bool isAuthorized = false, bool isStudent = false, bool isTeacher = false) {
            Mock<ICurrentSession> session = new();
            session.Setup(s => s.IsAuthorized()).Returns(isAuthorized);
            session.Setup(s => s.IsUserAStudent()).Returns(isStudent);
            session.Setup(s => s.IsUserATeacher()).Returns(isTeacher);
            return session;
        }

        [Fact]
        public async Task IndexGet_ReturnsIndexView_UserIsStudent_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isStudent: true);
            ChallengeController mockController = new(mockSession.Object);
            ViewResult result = await mockController.Index(1) as ViewResult;
            Assert.True(string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public async Task IndexGet_ReturnsAccessDenied_UserIsTeacher_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            ChallengeController mockController = new(mockSession.Object);
            ViewResult result = await mockController.Index(1) as ViewResult;
            string expected = "AccessDenied";
            string resultUrl = $"{result.ViewName}";
            Assert.Equal(expected, resultUrl);
        }

        [Fact]
        public async Task CreateGet_ReturnsCreateView_UserIsTeacher_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            ChallengeController mockController = new(mockSession.Object);
            ViewResult result = await mockController.Create() as ViewResult;
            Assert.True(string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public async Task CreateGet_ReturnsAccessDenied_UserIsStudent_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isStudent: true);
            ChallengeController mockController = new(mockSession.Object);
            ViewResult result = await mockController.Create() as ViewResult;
            string expected = "AccessDenied";
            string resultUrl = $"{result.ViewName}";
            Assert.Equal(expected, resultUrl);
        }

        [Fact]
        public async Task EditGet_ReturnsEditView_UserIsTeacher_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            ChallengeController mockController = new(mockSession.Object);
            ViewResult result = await mockController.Edit(1) as ViewResult;
            Assert.True(string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public async Task EditGet_ReturnsAccessDenied_UserIsStudent_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isStudent: true);
            ChallengeController mockController = new(mockSession.Object);
            ViewResult result = await mockController.Edit(1) as ViewResult;
            string expected = "AccessDenied";
            string resultUrl = $"{result.ViewName}";
            Assert.Equal(expected, resultUrl);
        }

        [Fact]
        public async Task ChangeActive_NonExistent_ReturnsErrorView_UserIsTeacher_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            ChallengeController mockController = new(mockSession.Object);
            ViewResult result = await mockController.ChangeActive(0) as ViewResult;
            string expected = "Error";
            string resultUrl = $"{result.ViewName}";
            Assert.Equal(expected, resultUrl);
        }

        [Fact]
        public async Task Delete_ReturnsTeacherIndex_DeleteIsNotSuccessful_UserIsTeacher_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            ChallengeController mockController = new(mockSession.Object);
            var mockTempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            mockController.TempData = mockTempData;
            RedirectToActionResult result = (RedirectToActionResult)await mockController.Delete(0);
            string expected = "Teacher/Index";
            string resultUrl = $"{result.ControllerName}/{result.ActionName}";
            string expectedMsg = "An error occurred: This challenge does not exits";
            string resultMsg = (string)mockController.TempData["DeleteMsg"];
            Assert.Equal(expected, resultUrl);
            Assert.Equal(expectedMsg, resultMsg);
        }

    }
}
