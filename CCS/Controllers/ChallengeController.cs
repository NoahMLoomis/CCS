using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CCS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CCS.Controllers {
    public class ChallengeController : Controller {

        private readonly Language _language;
        private readonly Challenge _challenge;
        private readonly DataType _dataType;
        private readonly ICurrentSession _cs;


        public ChallengeController(ICurrentSession cs) {
            _language = new Language();
            _challenge = new Challenge();
            _dataType = new DataType();
            _cs = cs;
        }

        public async Task<IActionResult> Index(int id) {
            // _language.GetAllLanguage() will need to be changed to a _challenge method to only get the languages for that challenge
            // Same thing for the other instances of this in the controller
            try {
                if (_cs.IsUserAStudent()) {
                    ViewData["ChallengeLanguages"] = new SelectList(await _language.GetAllLanguages(), "LanguageId", "LanguageName");
                    return View(await _challenge.GetChallenge(id));
                }
                return View("AccessDenied");
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create() {
            try {
                if (!_cs.IsUserAStudent()) {
                    ViewBag.Languages = await _language.GetAllLanguages();
                    ViewBag.DataTypes = await _dataType.GetAllDataTypes();
                    ViewBag.AllDifficulties = _challenge.GetDifficultiesDropdown();
                    return View();
                }
                return View("AccessDenied");
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });

            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChallengeDTO challengeDTO) {
            var challenge = new Challenge() {
                ChallengeId = challengeDTO.ChallengeId,
                Title = challengeDTO.Title,
                Description = challengeDTO.Description,
                Example = challengeDTO.Example,
                FunctionName = challengeDTO.FunctionName,
                TeacherId = challengeDTO.TeacherId,
                Active = challengeDTO.Active,
                DifficultyLevel = challengeDTO.DifficultyLevel,
                CreationDate = challengeDTO.CreationDate,
                Teacher = challengeDTO.Teacher,
                ChallengeLanguage = challengeDTO.ChallengeLanguage,
                DifficultyLevelNumber = challengeDTO.DifficultyLevelNumber,
                DifficultyLevelOrderedAsce = challengeDTO.DifficultyLevelOrderedAsce,
                Language = challengeDTO.Language,
                ReturnTypeId = challengeDTO.ReturnTypeId

            };

            challenge.SanitizeChallenge();
            ViewData["Languages"] = await _language.GetAllLanguages();
            ViewBag.AllDifficulties = _challenge.GetDifficultiesDropdown();
            ViewBag.DataTypes = await _dataType.GetAllDataTypes();
            if (!ModelState.IsValid || challenge.ReturnTypeId == 0 || challenge.Language == 0) {
                if (challenge.Language == 0)
                    ViewBag.ErrorLanguageMessage = "Please select the Language.";
                if (challenge.ReturnTypeId == 0)
                    ViewBag.ErrorReturnTypeMessage = "Please select the Return Type.";
                return View(challengeDTO);
            } else {
                ViewBag.DataTypes = await _dataType.GetDataTypesByLanguage(challenge.Language);
                try {
                    //        // Testing
                    //        challengeDTO.Title = "ChallengeDTO Test";
                    //        challengeDTO.Description = "This is a test.";
                    //        challengeDTO.Example = "This is an example.";
                    //        challengeDTO.FunctionName = "Function()";
                    //        challengeDTO.TeacherId = 2876;
                    //        challengeDTO.Active = true;
                    //        challengeDTO.DifficultyLevel = "Easy";
                    //        challengeDTO.CreationDate = new DateTime();
                    //        challengeDTO.DifficultyLevelNumber = 1;
                    //        challengeDTO.DifficultyLevelOrderedAsce = true;
                    //        challengeDTO.Language = 1;

                    //        challengeDTO.Parameters = new List<Parameter> {
                    //    new Parameter {
                    //        DataTypeId = 1,
                    //        ChallengeLanguageId = 2,
                    //        Position = 1,
                    //        DefaultValue = "Apple",
                    //        ParameterName = "Param1"
                    //    },
                    //    new Parameter {
                    //        DataTypeId = 1,
                    //        ChallengeLanguageId = 2,
                    //        Position = 2,
                    //        DefaultValue = "Pie",
                    //        ParameterName = "Param2"
                    //    }
                    //};
                    //        challengeDTO.TestCases = new List<TestCase> {
                    //    new TestCase {
                    //        ExpectedResult = "Apple Pie",
                    //        ChallengeLanguageId = 2
                    //    }
                    //};
                    //        challengeDTO.TestCases[0].ParameterValues = new List<string> {
                    //    "Apple",
                    //    "Pie"
                    //};

                    await challenge.AddChallenge(_cs.GetEmployeeId());
                    var cl_id = await new ChallengeLanguage().GetChallengeLanguageIdFromChallenge(challenge.ChallengeId);

                    if (challengeDTO.Parameters[0].ParameterName != null) {
                        // Adding Parameter records to Database
                        foreach (Parameter p in challengeDTO.Parameters) {
                            p.ChallengeLanguageId = cl_id;
                            await p.AddParameter();
                        }
                    }

                    if (challengeDTO.TestCases[0].ExpectedResult != null) {
                        // Adding TestCase records to Database
                        foreach (TestCase tc in challengeDTO.TestCases) {
                            tc.ChallengeLanguageId = cl_id;
                            await tc.AddTestCase();
                        }
                    }

                    if (challengeDTO.Parameters[0].ParameterName != null && challengeDTO.TestCases[0].ExpectedResult != null) {
                        // Adding TestCaseParameter records to Database
                        foreach (TestCase tc in challengeDTO.TestCases) {
                            for (int i = 0; i < tc.ParameterValues.Count; i++) {
                                var tcp = new TestCaseParameter {
                                    TestCaseId = tc.TestCaseId,
                                    ParameterId = challengeDTO.Parameters[i].ParameterId,
                                    Value = tc.ParameterValues[i]
                                };

                                await tcp.AddTestCaseParameter(); //adds itself to DB
                            }
                        }
                    }
                    return RedirectToAction("Index", "Teacher");
                } catch (Exception e) {
                    return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });

                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(decimal id) {
            try {
                if (!_cs.IsUserAStudent()) {
                    var challengeToEdit = await _challenge.GetChallenge(id);
                    ViewBag.Languages = await _language.GetAllLanguages();
                    ViewBag.AllDifficulties = _challenge.GetDifficultiesDropdown();

                    // Get Parameters and TestCases for the Challenge (TestCaseParameters are included in TestCases)
                    ViewBag.Parameters = await new Parameter().GetParametersForChallenge(id);
                    ViewBag.TestCases = await new TestCase().GetTestCasesForChallenge(id);

                    ViewBag.StudentAttempting = await _challenge.GetStudentsAttemptingChallenge(id);
                    ViewBag.LastAttempted = await _challenge.GetLastTimeChallengeWasAttempted(id);
                    ViewBag.AllDataTypes = await _dataType.GetAllDataTypesByLanguageId(challengeToEdit.DataType.LanguageId);
                    ViewBag.DataTypes = await _dataType.GetDataTypesByLanguage(id);

                    // Create challengeDTO object to pass in to edit challenge page
                    ChallengeDTO challengeDTO = new ChallengeDTO(challengeToEdit);
                    challengeDTO.Parameters = ViewBag.Parameters;
                    challengeDTO.TestCases = ViewBag.TestCases;

                    return View(challengeDTO);
                }
                return View("AccessDenied");
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChallengeDTO challengeDTO) {
            var challenge = new Challenge() {
                ChallengeId = challengeDTO.ChallengeId,
                Title = challengeDTO.Title,
                Description = challengeDTO.Description,
                Example = challengeDTO.Example,
                FunctionName = challengeDTO.FunctionName,
                TeacherId = challengeDTO.TeacherId,
                Active = challengeDTO.Active,
                DifficultyLevel = challengeDTO.DifficultyLevel,
                CreationDate = challengeDTO.CreationDate,
                Teacher = challengeDTO.Teacher,
                ChallengeLanguage = challengeDTO.ChallengeLanguage,
                DifficultyLevelNumber = challengeDTO.DifficultyLevelNumber,
                DifficultyLevelOrderedAsce = challengeDTO.DifficultyLevelOrderedAsce,
                Language = challengeDTO.Language,
                ReturnTypeId = challengeDTO.ReturnTypeId
            };

            var cl_id = await new ChallengeLanguage().GetChallengeLanguageIdFromChallenge(challenge.ChallengeId);


            if (challengeDTO.Parameters[0].ParameterName != null) {
                // Editing TestCase records in Database
                foreach (TestCase tc in challengeDTO.TestCases) {
                    tc.ChallengeLanguageId = cl_id;
                    await new TestCase().EditTestCase(tc);
                }
            }

            if (challengeDTO.TestCases[0].ExpectedResult != null) {
                // Editing Parameter records in Database
                foreach (Parameter p in challengeDTO.Parameters) {
                    p.ChallengeLanguageId = cl_id;
                    await new Parameter().EditParameter(p);
                }
            }

            if (challengeDTO.Parameters[0].ParameterName != null && challengeDTO.TestCases[0].ExpectedResult != null) {
                // Editing TestCaseParameter records in Database
                foreach (TestCase tc in challengeDTO.TestCases) {
                    foreach (var tcp in tc.TestCaseParameter) {
                        await new TestCaseParameter().EditTestCaseParameter(tcp);
                    }
                }
            }

            challenge.SanitizeChallenge();
            ViewBag.Languages = await _language.GetAllLanguages();
            ViewBag.AllDifficulties = _challenge.GetDifficultiesDropdown();
            //ViewBag.AllParameters = await _challenge.GetChallengeParameters(challenge.ChallengeId);
            ViewBag.StudentAttempting = await _challenge.GetStudentsAttemptingChallenge(challenge.ChallengeId);
            ViewBag.LastAttempted = await _challenge.GetLastTimeChallengeWasAttempted(challenge.ChallengeId);
            ViewBag.DataTypes = await _dataType.GetDataTypesByLanguage(challenge.ChallengeId);
            try {
                if (!ModelState.IsValid) {
                    return View(challenge);
                } else {
                    await _challenge.EditChallenge(challenge);
                    TempData["UpdateMsg"] = "Successfuly updated '" + challenge.Title + "'";
                    return RedirectToAction("Index", "Teacher");
                }
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(decimal id) {
            try {
                var title = await _challenge.GetChallengeTitle(id);
                var didDelete = await new Challenge().DeleteChallenge(id);
                if (didDelete) {
                    TempData["DeleteMsg"] = "Delete for challenge '" + title + "' was successful";
                } else {
                    TempData["DeleteMsg"] = "Delete for challenge " + title + " was unsuccessful";
                }
                return RedirectToAction("Index", "Teacher");
            } catch (Exception e) {
                TempData["DeleteMsg"] = "An error occurred: " + e.Message;
                return RedirectToAction("Index", "Teacher");
            }
        }

        public async Task<IActionResult> ToggleTeacherChallengeOrder(int teacherId, bool ascending) {
            try {
                Teacher teacher = await _challenge.SortTeacherChallenges(teacherId, ascending);
                return View("~/Views/Teacher/Index.cshtml", teacher);
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeActive(int id) {
            try {
                Challenge challenge = await _challenge.GetChallenge(id);
                if (challenge == null) {
                    return View("Error", new ErrorMessageModel() { ErrorMessage = "The challenge does not exist" });
                }
                challenge.ToggleChallenge();
                Teacher teacher = challenge.Teacher;
                return View("~/Views/Teacher/Index.cshtml", teacher);
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

        [HttpGet]
        public async Task<IEnumerable<DataType>> GetAllDataTypesByLanguageId(int languageId) {
            return await _dataType.GetAllDataTypesByLanguageId(languageId);
        }

    }
}
