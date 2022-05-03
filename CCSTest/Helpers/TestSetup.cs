using CCS.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPTest.Helpers {
    public class TestSetup : IDisposable {
        private DbConnection connection;
        private readonly DbContextOptions<CCSContext> _options;

        public TestSetup() {
            connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            _options = new DbContextOptionsBuilder<CCSContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;

            using var context = new CCSContext(_options);

            context.Database.EnsureCreated();
            AddData(context);
            context.SaveChangesAsync();

        }
        public void AddData(CCSContext context) {
            AddTeachers(context);
            AddLanguages(context);
            AddDataTypes(context);
            AddChallenges(context);
            AddChallengeLanguages(context);
            AddStudents(context);
            AddCodeSubmission(context);
            AddTestCases(context);
            AddParameters(context);
            AddResults(context);

        }
        public void AddTeachers(CCSContext context) {
            context.Teacher.AddRange(
                new Teacher() { TeacherId = 1 },
                new Teacher() { TeacherId = 2 },
                new Teacher() { TeacherId = 3 }
            );
            context.SaveChanges();
        }

        public void AddLanguages(CCSContext context) {
            context.Language.AddRange(
                new Language() { LanguageId = 1, LanguageName = "Python" },
                new Language() { LanguageId = 2, LanguageName = "Javascript" },
                new Language() { LanguageId = 3, LanguageName = "C#" },
                new Language() { LanguageId = 4, LanguageName = "Java" },
                new Language() { LanguageId = 5, LanguageName = "PHP" }
            );
            context.SaveChanges();

        }

        /*
        * TODO: Add C#, Java, PHP data types
        */
        public void AddDataTypes(CCSContext context) {
            context.DataType.AddRange(
                // Python data types
                new DataType() { DataTypeId = 1, DataTypeName = "str", LanguageId = 1 },
                new DataType() { DataTypeId = 2, DataTypeName = "int", LanguageId = 1 },
                new DataType() { DataTypeId = 3, DataTypeName = "float", LanguageId = 1 },
                new DataType() { DataTypeId = 4, DataTypeName = "complex", LanguageId = 1 },
                new DataType() { DataTypeId = 5, DataTypeName = "list", LanguageId = 1 },
                new DataType() { DataTypeId = 6, DataTypeName = "tuple", LanguageId = 1 },
                new DataType() { DataTypeId = 7, DataTypeName = "range", LanguageId = 1 },
                new DataType() { DataTypeId = 8, DataTypeName = "dict", LanguageId = 1 },
                new DataType() { DataTypeId = 9, DataTypeName = "set", LanguageId = 1 },
                new DataType() { DataTypeId = 10, DataTypeName = "frozenset", LanguageId = 1 },
                new DataType() { DataTypeId = 11, DataTypeName = "bool", LanguageId = 1 },
                new DataType() { DataTypeId = 12, DataTypeName = "bytes", LanguageId = 1 },
                new DataType() { DataTypeId = 13, DataTypeName = "bytearray", LanguageId = 1 },
                new DataType() { DataTypeId = 14, DataTypeName = "memoryview", LanguageId = 1 },

                // JS data types
                new DataType() { DataTypeId = 15, DataTypeName = "number", LanguageId = 2 },
                new DataType() { DataTypeId = 16, DataTypeName = "bigInt", LanguageId = 2 },
                new DataType() { DataTypeId = 17, DataTypeName = "boolean", LanguageId = 2 },
                new DataType() { DataTypeId = 18, DataTypeName = "array", LanguageId = 2 },
                new DataType() { DataTypeId = 19, DataTypeName = "object", LanguageId = 2 },

                new DataType() { DataTypeId = 20, DataTypeName = "number", LanguageId = 2 },
                new DataType() { DataTypeId = 21, DataTypeName = "bigInt", LanguageId = 2 },
                new DataType() { DataTypeId = 22, DataTypeName = "boolean", LanguageId = 2 },
                new DataType() { DataTypeId = 23, DataTypeName = "array", LanguageId = 2 },
                new DataType() { DataTypeId = 24, DataTypeName = "object", LanguageId = 2 }
            );
            context.SaveChanges();
        }
        public void AddChallenges(CCSContext context) {
            context.Challenge.AddRange(
                new Challenge() {
                    ChallengeId = 1,
                    Title = "Simple addition",
                    Description = "Add two numbers together",
                    Example = "1+1 = 2",
                    FunctionName = "simpleAddition()",
                    TeacherId = 1,
                    Active = true,
                    DifficultyLevel = "Easy",
                    CreationDate = DateTime.Today.AddDays(-1),
                    ReturnTypeId = 1
                },
                new Challenge() {
                    ChallengeId = 2,
                    Title = "Simple multiplication",
                    Description = "Multiply two numbers together",
                    Example = "2 * 2 = 4",
                    FunctionName = "simpleMultiplication()",
                    TeacherId = 1,
                    Active = true,
                    DifficultyLevel = "Medium",
                    CreationDate = DateTime.Today.AddDays(-1),
                    ReturnTypeId = 2
                },
                new Challenge() {
                    ChallengeId = 3,
                    Title = "Simple division",
                    Description = "divide two numbers together",
                    Example = "2 / 2 = 1",
                    FunctionName = "simpleDivision()",
                    TeacherId = 1,
                    Active = false,
                    DifficultyLevel = "Hard",
                    CreationDate = DateTime.Today.AddDays(-1),
                    ReturnTypeId = 3,
                    
                }
            );
            context.SaveChanges();

        }


        public void AddChallengeLanguages(CCSContext context) {
            context.ChallengeLanguage.AddRange(
                new ChallengeLanguage() { ChallengeLanguageId = 1, ChallengeId = 1, LanguageId = 2 },
                new ChallengeLanguage() { ChallengeLanguageId = 2, ChallengeId = 2, LanguageId = 2 },
                new ChallengeLanguage() { ChallengeLanguageId = 3, ChallengeId = 3, LanguageId = 2 }
            );

            context.SaveChanges();
        }

        public void AddStudents(CCSContext context) {
            context.AddRange(
                new Student() { StudentId = 1, FirstName = "Noah", LastName = "Loomis"},
                new Student() { StudentId = 2, FirstName = "Abe", LastName = "Getachew"},
                new Student() { StudentId = 3, FirstName = "Emmanuelle", LastName = "Fontaine"}
            );
            context.SaveChanges();
        }

        public void AddCodeSubmission(CCSContext context) {
            context.CodeSubmission.AddRange(
                // These first 4 are code submissions from the same student
                new CodeSubmission() { CodeSubmissionId = 1, Code = "return \"1+1\"", StudentId = 1, ChallengeLanguageId = 1, Status = true, LastAttempted = DateTime.Today.AddDays(-1) },
                new CodeSubmission() { CodeSubmissionId = 2, Code = "return \"2+2\"", StudentId = 1, ChallengeLanguageId = 1, Status = true, LastAttempted = DateTime.Today.AddDays(-1) },
                new CodeSubmission() { CodeSubmissionId = 3, Code = "return \"3+3\"", StudentId = 1, ChallengeLanguageId = 1, Status = true, LastAttempted = DateTime.Today.AddDays(-1) },
                new CodeSubmission() { CodeSubmissionId = 4, Code = "return \"-1\"", StudentId = 1, ChallengeLanguageId = 1, Status = true, LastAttempted = DateTime.Today },


                // The following code submission is the previous challenge attempted by a different student
                new CodeSubmission() { CodeSubmissionId = 5, Code = "return \"3+3\"", StudentId = 2, ChallengeLanguageId = 1, Status = true, LastAttempted = DateTime.Today.AddDays(-1) },

                new CodeSubmission() { CodeSubmissionId = 6, Code = "return \"2*2\"", StudentId = 2, ChallengeLanguageId = 2, Status = true, LastAttempted = DateTime.Today },
                new CodeSubmission() { CodeSubmissionId = 7, Code = "return \"2/2\"", StudentId = 3, ChallengeLanguageId = 3, Status = false, LastAttempted = DateTime.Today },

                // The following code submission is not completed
                new CodeSubmission() { CodeSubmissionId = 8, Code = "return \"null\"", StudentId = 3, ChallengeLanguageId = 3, Status = false, LastAttempted = DateTime.Today }
            );
            context.SaveChanges();
        }

       

        // TODO: More test cases should be added as required
        public void AddTestCases(CCSContext context) {
            context.TestCase.AddRange(
                new TestCase() { TestCaseId = 1, ExpectedResult = "2", ChallengeLanguageId = 1 },
                new TestCase() { TestCaseId = 2, ExpectedResult = "4", ChallengeLanguageId = 2 },
                new TestCase() { TestCaseId = 3, ExpectedResult = "1", ChallengeLanguageId = 3 }
            );
            context.SaveChanges();
        }
        public void AddTestCaseParameters(CCSContext context) {
            context.TestCaseParameter.AddRange(
                new TestCaseParameter() { TestCaseParameterId = 1, TestCaseId = 1, ParameterId = 1, Value = "hi" },
                new TestCaseParameter() { TestCaseParameterId = 2, TestCaseId = 2, ParameterId = 2, Value = "hello" },
                new TestCaseParameter() { TestCaseParameterId = 3, TestCaseId = 3, ParameterId = 3, Value = "howdy" }
            );
            context.SaveChanges();
        }
        //public void AddParameters(CCSContext context) {
        //    context.Parameter.AddRange(
        //        new Parameter() { ParameterId = 1, TestCaseId = 1, DataTypeId = 2, Position = 1 },
        //        new Parameter() { ParameterId = 2, TestCaseId = 1, DataTypeId = 2, Position = 2 },
        //        new Parameter() { ParameterId = 3, TestCaseId = 2, DataTypeId = 2, Position = 1 },
        //        new Parameter() { ParameterId = 4, TestCaseId = 2, DataTypeId = 2, Position = 2 },
        //        new Parameter() { ParameterId = 5, TestCaseId = 3, DataTypeId = 2, Position = 1 },
        //        new Parameter() { ParameterId = 6, TestCaseId = 3, DataTypeId = 2, Position = 2 }
        //    );
        //    context.SaveChanges();
        //}

        public void AddParameters(CCSContext context) {
            context.Parameter.AddRange(
                new Parameter() { ParameterId = 1, DataTypeId = 2, ChallengeLanguageId = 1, Position = 1, DefaultValue = "a", ParameterName = "hi"},
                new Parameter() { ParameterId = 2, DataTypeId = 2, ChallengeLanguageId = 1, Position = 2, DefaultValue = "a", ParameterName = "hi" },
                new Parameter() { ParameterId = 3, DataTypeId = 2, ChallengeLanguageId = 1, Position = 1, DefaultValue = "a", ParameterName = "hi" },
                new Parameter() { ParameterId = 4, DataTypeId = 2, ChallengeLanguageId = 1, Position = 2, DefaultValue = "a", ParameterName = "hi" },
                new Parameter() { ParameterId = 5, DataTypeId = 2, ChallengeLanguageId = 1, Position = 1, DefaultValue = "a", ParameterName = "hi" },
                new Parameter() { ParameterId = 6, DataTypeId = 2, ChallengeLanguageId = 1, Position = 2, DefaultValue = "a", ParameterName = "hi" }
            );
            context.SaveChanges();
        }

        public void AddResults(CCSContext context) {
            context.Result.AddRange(
                new Result() { ResultId = 1, Passed = true, TestCaseId = 1, CodeSubmissionId = 1, CodeOutput = "2" },
                new Result() { ResultId = 2, Passed = false, TestCaseId = 1, CodeSubmissionId = 4, CodeOutput = "-1" },
                new Result() { ResultId = 3, Passed = true, TestCaseId = 3, CodeSubmissionId = 5, CodeOutput = "6" }
            );
            context.SaveChanges();
        }

        public void Dispose() => connection.Dispose();

        public CCSContext CreateContext() => new CCSContext(_options);
    }
}
