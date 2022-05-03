using CCS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CCS.Controllers {
    public class StudentController : Controller {
        private readonly ICurrentSession _cs;
        private readonly Challenge _challenge;
        public StudentController(ICurrentSession currentSession) {
            _cs = currentSession;
            _challenge = new Challenge();
        }

        public async Task<IActionResult> Index() {
            try {
                if (_cs.IsAuthorized()) {
                    if (_cs.IsUserAStudent()) {
                        return View(await _challenge.AllChallenges(true));
                    } else if (_cs.IsUserATeacher()) {
                        return RedirectToAction("Index", "Teacher");
                    }
                }
                return RedirectToAction("Index", "Login");
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

        public async Task<IActionResult> ToggleOrder(bool ascending) {
            try {
                return View("Index", await _challenge.AllChallenges(ascending));
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

    }
}