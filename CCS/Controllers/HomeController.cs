using CCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace CCS.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly ICurrentSession _cs;

        public HomeController(ICurrentSession currentSession, ILogger<HomeController> logger) {
            _cs = currentSession;
            _logger = logger;
        }

        public IActionResult Index() {
            try {
                if (_cs.IsAuthorized()) {
                    if (_cs.IsUserAStudent()) {
                        return RedirectToAction("Index", "Student");
                    }
                    if (_cs.IsUserATeacher()) {
                        return RedirectToAction("Index", "Teacher");
                    }
                    return View();
                } else {
                    return RedirectToAction("Index", "Login");
                }
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
