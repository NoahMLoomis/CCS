using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Ganss.XSS;
using System.ComponentModel.DataAnnotations;

namespace CCS.Models {
    public partial class Challenge {
        private readonly CCSContext _context;
        private readonly Student _student;
        private readonly Teacher _teacher;
        private readonly HtmlSanitizer sanitizer;

        public Challenge() {
            _context = new CCSContext();
            sanitizer = new HtmlSanitizer();
            ChallengeLanguage = new HashSet<ChallengeLanguage>();
            _student = new Student();
            _teacher = new Teacher();
        }

        public Challenge(CCSContext context) {
            _context = context;
            sanitizer = new HtmlSanitizer();
            ChallengeLanguage = new HashSet<ChallengeLanguage>();
            _student = new Student(context);
            _teacher = new Teacher(context);
        }


        public decimal ChallengeId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 50 characters")]
        public string Title { get; set; }

        [Required]
        [StringLength(600, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 600 characters")]
        public string Description { get; set; }

        [StringLength(300, MinimumLength = 2, ErrorMessage = "Example must be between 2 and 300 characters")]
        public string Example { get; set; }

        [DisplayName("Function Name")]
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Function Name must be between 2 and 20 characters")]
        public string FunctionName { get; set; }

        public decimal TeacherId { get; set; }

        [Required]
        public bool Active { get; set; }

        [DisplayName("Difficulty")]
        public string DifficultyLevel { get; set; }

        public decimal ReturnTypeId { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual Teacher Teacher { get; set; }

        [DisplayName("Language")]
        public virtual ICollection<ChallengeLanguage> ChallengeLanguage { get; set; }

        public virtual DataType DataType { get; set; }

        [NotMapped]
        public int DifficultyLevelNumber { get; set; }

        [NotMapped]
        public bool DifficultyLevelOrderedAsce = true;

        [NotMapped]
        public decimal Language { get; set; }

        public void ToggleChallenge() {
            Active = !Active;
            _context.Update(this);
            _context.SaveChanges();
        }

        public async Task<Challenge> GetChallenge(decimal challengeId) {
            return await _context.Challenge
                .Include(x => x.Teacher)
                .Include(x => x.ChallengeLanguage)
                .ThenInclude(x => x.Language)
                .Include(x => x.DataType)
                .Include(x => x.ChallengeLanguage)
                .ThenInclude(x => x.TestCase)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ChallengeId == challengeId);
        }

        public async Task EditChallenge(Challenge challenge) {
            _context.Update(challenge);
            await _context.SaveChangesAsync();
        }

        public void SanitizeChallenge() {
            Title = sanitizer.Sanitize(Title);
            Description = sanitizer.Sanitize(Description);
            Example = sanitizer.Sanitize(Example);
            FunctionName = sanitizer.Sanitize(FunctionName);
        }

        public SelectList GetDifficultiesDropdown() {
            List<object> difficulties = new List<object> {
                new { DifficultyName = "Easy", DifficultyCode = "Easy" },
                new { DifficultyName = "Medium", DifficultyCode = "Medium" },
                new { DifficultyName = "Hard", DifficultyCode = "Hard" },
            };

            return new SelectList(difficulties, "DifficultyName", "DifficultyCode");
        }

        public async Task<List<Challenge>> AllChallenges(bool order) {
            var challenges = await _context.Challenge
                                        .Where(a => a.Active)
                                        .Include(c => c.ChallengeLanguage)
                                        .ThenInclude(l => l.Language)
                                        .ToListAsync();

            return SortChallenges(challenges, order);
        }

        public List<Challenge> SortChallenges(List<Challenge> challenges, bool order) {
            SetDifficultyLevelNumber(challenges);
            if (order) {
                challenges = SortChallengesByDifficultyLevel(challenges);
            } else {
                challenges = SortChallengesByDifficultyLevelDecending(challenges);
            }

            foreach (var c in challenges) {
                c.DifficultyLevelOrderedAsce = !order;
            }

            challenges.OrderBy(t => t.Title.ToLower());

            return challenges;
        }

        public async Task<Teacher> SortTeacherChallenges(int teacherId, bool order) {
            var challenges = await _context.Challenge
                            .OrderBy(x => x.Title.ToLower())
                            .Where(x => x.TeacherId == teacherId)
                            .Include(x => x.ChallengeLanguage)
                            .ThenInclude(x => x.Language)
                            .ToListAsync();

            Teacher teacher = await _teacher.GetTeacherById(teacherId);

            teacher.Challenge = SortChallenges(challenges, order);

            return teacher;
        }

        public List<Challenge> SortChallengesByDifficultyLevel(List<Challenge> challenges) {
            SetDifficultyLevelNumber(challenges);
            return challenges.OrderBy(c => c.DifficultyLevelNumber)
                             .ThenBy(t => t.Title.ToLower()).ToList();
        }

        public List<Challenge> SortChallengesByDifficultyLevelDecending(List<Challenge> challenges) {
            SetDifficultyLevelNumber(challenges);
            return challenges
                .OrderByDescending(c => c.DifficultyLevelNumber)
                .ThenBy(t => t.Title.ToLower()).ToList();
        }

        public void SetDifficultyLevelNumber(List<Challenge> challenges) {
            foreach (var challenge in challenges) {
                if (challenge.DifficultyLevel.ToLower() == "easy") {
                    challenge.DifficultyLevelNumber = 1;
                } else if (challenge.DifficultyLevel.ToLower() == "medium") {
                    challenge.DifficultyLevelNumber = 2;
                } else if (challenge.DifficultyLevel.ToLower() == "hard") {
                    challenge.DifficultyLevelNumber = 3;
                }
            }
        }

        public async Task AddChallenge(int teacherId) {
            TeacherId = teacherId;
            CreationDate = DateTime.Today;
            await _context.Challenge.AddAsync(this);
            await _context.SaveChangesAsync();
            await AddChallengeLanguage();
        }

        public async Task AddChallengeLanguage() {
            decimal challengeId = _context.Challenge
                            .OrderByDescending(p => (int)p.ChallengeId)
                            .Select(r => r.ChallengeId)
                            .First();
            ChallengeLanguage challengeLanguage = new ChallengeLanguage(_context);
            challengeLanguage.ChallengeId = challengeId;
            challengeLanguage.LanguageId = Language;



            await _context.ChallengeLanguage.AddAsync(challengeLanguage);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetStudentsAttemptingChallenge(decimal id) {
            if (await GetChallenge(id) == null) {
                throw new Exception("This challenge does not exits");
            }
            var studentList = await _context.CodeSubmission
                        .Where(c => c.ChallengeLanguage.ChallengeId == id && c.Status == true)
                        .Select(s => new Student { FirstName = s.Student.FirstName, LastName = s.Student.LastName })
                        .Distinct()
                        .ToListAsync();
            return studentList.Count().ToString();
        }

        public async Task<string> GetLastTimeChallengeWasAttempted(decimal id) {
            if (await GetChallenge(id) == null) {
                throw new Exception("This challenge does not exits");
            }

            CodeSubmission lastAttempted = await _context.CodeSubmission
                .Where(c => c.ChallengeLanguage.ChallengeId == id)
                .OrderByDescending(c => c.LastAttempted)
                .FirstOrDefaultAsync();

            if (lastAttempted == null) {
                return "This challenge has never been attempted";
            }

            return lastAttempted.LastAttempted.Date.ToString("MMMM d, yyyy");
        }

        public async Task<bool> DeleteChallenge(decimal id) {
            var challengeToRemove = await _context.Challenge
                                    .Where(c => c.ChallengeId == id)
                                    .FirstOrDefaultAsync();

            if (challengeToRemove == null) {
                return false;
            }

            var challengeLanguages = await _context.ChallengeLanguage
                                        .Where(cl => cl.ChallengeId == id)
                                        .ToListAsync();

            if (challengeLanguages.Any()) {
                foreach (var language in challengeLanguages) {
                    var codeSubmission = await _context.CodeSubmission
                                    .Where(c => c.ChallengeLanguageId == language.ChallengeLanguageId)
                                    .ToListAsync();

                    var testCases = await _context.TestCase
                                        .Where(cl => cl.ChallengeLanguageId == language.ChallengeLanguageId)
                                        .ToListAsync();

                    if (testCases.Any()) {
                        foreach (var testCase in testCases) {
                            var testCaseParameters = await new TestCaseParameter().GetTCPsForTestCase(testCase.TestCaseId);

                            if (testCaseParameters.Any()) {
                                foreach (var testCaseParam in testCaseParameters) {
                                    _context.Remove(testCaseParam);
                                }
                            }

                            var results = await _context.Result
                                            .Where(r => r.TestCaseId == testCase.TestCaseId)
                                            .ToListAsync();

                            if (results.Any()) {
                                foreach (var result in results) {
                                    _context.Remove(result);

                                }
                            }
                            _context.Remove(testCase);

                        }
                    }

                    if (codeSubmission.Any()) {
                        foreach (var submission in codeSubmission) {
                            _context.Remove(submission);
                        }
                    }
                    var parameters = await new Parameter().GetParametersForChallenge(id);

                    if (parameters.Any()) {
                        foreach (var param in parameters) {
                            _context.Remove(param);
                        }
                    }

                    _context.Remove(language);

                }
            }
            _context.Remove(challengeToRemove);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetChallengeTitle(decimal id) {
            Challenge challenge = await _context.Challenge
                .Where(c => c.ChallengeId == id)
                .FirstOrDefaultAsync();
            if (challenge == null) {
                throw new Exception("This challenge does not exits");
            }
            return challenge.Title;
        }

        public List<string> GetChallengeLanguageNameList() {
            List<string> nameList = new List<string>();
            foreach (var language in ChallengeLanguage) {
                nameList.Add(language.Language.LanguageName.ToLower());
            }
            return nameList;
        }

        public async Task<List<CodeSubmission>> GetAllAttemptedChallenges(decimal id) {
            List<CodeSubmission> codeSubmissions = await _context.CodeSubmission
                .Where(x => x.StudentId == id)
                .Include(x => x.ChallengeLanguage)
                .ThenInclude(x => x.Challenge)
                .Include(x => x.ChallengeLanguage)
                .ThenInclude(x => x.Language)
                .OrderBy(x => x.LastAttempted)
                .ToListAsync();
            return codeSubmissions;
        }
    }
}
