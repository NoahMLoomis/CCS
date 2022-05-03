using CCS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CCS.Controllers {
    public class TeacherController : Controller {
        private readonly ICurrentSession _cs;
        private readonly Teacher _teacher = new Teacher();
        private readonly Challenge _challenge = new Challenge();

        public TeacherController(ICurrentSession currentSession) {
            _cs = currentSession;
        }

        public async Task<IActionResult> Index() {
            try {
                if (_cs.IsAuthorized()) {
                    if (_cs.IsUserAStudent()) {
                        return RedirectToAction("Index", "Student");
                    } else if (_cs.IsUserATeacher()) {
                        var teacher = await _teacher.GetTeacherById(_cs.GetEmployeeId());
                        var sorted_teacher = await _challenge.SortTeacherChallenges((int)teacher.TeacherId, true);
                        return View(sorted_teacher);
                    }
                }
                return RedirectToAction("Index", "Login");
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> FilterChallengeTitle(int id, string searchChallengeTitle) {
            try {
                var teacher = await _teacher.GetTeacherById(id);
                Teacher templeTeacher = new Teacher { TeacherId = teacher.TeacherId };
                ViewBag.ChallengeTitle = searchChallengeTitle;
                templeTeacher.Challenge = await _teacher.FilterChallengeTitle(id, searchChallengeTitle);
                return View("Index", templeTeacher);
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

    }
}
