using CCS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace CCS.Controllers {
    public class LoginController : Controller {
        private readonly ICurrentSession _cs;
        private readonly Student _student = new Student();
        private readonly Teacher _teacher = new Teacher();


        public LoginController(ICurrentSession currentSession) {
            _cs = currentSession;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index() {
            try {
                if (_cs.IsAuthorized()) {
                    if (_cs.IsUserAStudent())
                        return RedirectToAction("Index", "Student");
                    else if (_cs.IsUserATeacher())
                        return RedirectToAction("Index", "Teacher");
                }
                return View();
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(Login login) {
            try {
                if (ModelState.IsValid) {
                    await _cs.Login(login);
                    if (_cs.IsAuthorized()) {
                        if (_cs.IsUserAStudent()) {
                            Student student = new Student();
                            int studentId = _cs.GetStudentId();
                            var loggedStudent = await _student.GetStudent(studentId);

                            if (studentId == 0) {
                                loggedStudent = await _student.GetStudent(Convert.ToInt32(login.Username));
                            }

                            if (loggedStudent == null) {
                                student.StudentId = studentId;
                                student.FirstName = _cs.GetFirstName();
                                student.LastName = _cs.GetLastName();
                                if (student.FirstName != null && student.LastName != null) {
                                    await _student.AddStudent(student);
                                }
                            }
                            return RedirectToAction("Index", "Student");
                        } else if (_cs.IsUserATeacher()) {

                            Teacher teacher = new Teacher();
                            var loggedTeacher = await _teacher.GetTeacherById(_cs.GetEmployeeId());

                            if (loggedTeacher.TeacherId == 0) {
                                teacher.TeacherId = _cs.GetEmployeeId();
                                if (teacher.TeacherId != 0) {
                                    await _teacher.AddTeacher(teacher);
                                }
                            }

                            return RedirectToAction("Index", "Teacher");
                        } else {
                            ModelState.AddModelError(string.Empty, "The username or password you have entered is incorrect.");
                            return View();
                        }
                    } else {
                        ModelState.AddModelError(string.Empty, "The username or password you have entered is incorrect.");
                        return View();
                    }
                }
                return View();
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() {
            return View();
        }

        [AllowAnonymous]
        [Route("Logout")]
        public IActionResult Logout() {
            try {
                _cs.Logout();
                return RedirectToAction(nameof(Index));
            } catch (Exception e) {
                return View("Error", new ErrorMessageModel() { ErrorMessage = e.Message });
            }
        }
    }
}
