using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;


namespace CCS.Models {
    public class ChallengeDTO {

        public ChallengeDTO() {

        }

        public ChallengeDTO(Challenge c) {
            ChallengeId = c.ChallengeId;
            Title = c.Title;
            Description = c.Description;
            Example = c.Example;
            FunctionName = c.FunctionName;
            TeacherId = c.TeacherId;
            Active = c.Active;
            DifficultyLevel = c.DifficultyLevel;
            CreationDate = c.CreationDate;
            Teacher = c.Teacher;
            ChallengeLanguage = c.ChallengeLanguage;
            DifficultyLevelNumber = c.DifficultyLevelNumber;
            DifficultyLevelOrderedAsce = c.DifficultyLevelOrderedAsce;
            Language = c.Language;
        }

        public ChallengeDTO(decimal challengeId, string title, string description, string example, string functionName, decimal teacherId, bool active, string difficultyLevel, DateTime creationDate, Teacher teacher, ICollection<ChallengeLanguage> challengeLanguage, ICollection<CodeSubmission> codeSubmission, int difficultyLevelNumber, bool difficultyLevelOrderedAsce, decimal language, List<TestCase> testCases, List<Parameter> parameters) {
            ChallengeId = challengeId;
            Title = title;
            Description = description;
            Example = example;
            FunctionName = functionName;
            TeacherId = teacherId;
            Active = active;
            DifficultyLevel = difficultyLevel;
            CreationDate = creationDate;
            Teacher = teacher;
            ChallengeLanguage = challengeLanguage;
            CodeSubmission = codeSubmission;
            DifficultyLevelNumber = difficultyLevelNumber;
            DifficultyLevelOrderedAsce = difficultyLevelOrderedAsce;
            Language = language;
            TestCases = testCases;
            Parameters = parameters;
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
        public virtual ICollection<CodeSubmission> CodeSubmission { get; set; }

        [NotMapped]
        public int DifficultyLevelNumber { get; set; }

        [NotMapped]
        public bool DifficultyLevelOrderedAsce = true;

        [NotMapped]
        public decimal Language { get; set; }

        public List<TestCase> TestCases { get; set; }

        public List<Parameter> Parameters { get; set; }

        public List<TestCaseParameter> TestCaseParameters { get; set; }

        public List<string> GetChallengeLanguageNameList() {
            List<string> nameList = new List<string>();
            foreach (var language in ChallengeLanguage) {
                nameList.Add(language.Language.LanguageName.ToLower());
            }
            return nameList;
        }

        public async Task<Challenge> GetChallengeOnly() {
            Challenge challenge = await new Challenge().GetChallenge(this.ChallengeId);
            return challenge;
        }
    }
}