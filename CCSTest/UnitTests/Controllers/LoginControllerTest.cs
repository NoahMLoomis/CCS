using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using CCS.Models;
using System.Text.Encodings.Web;
using CCS.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CCSTest.UnitTests.Controllers {
    public class LoginControllerTest {
        private Mock<ICurrentSession> CreateSession(bool isAuthorized = false, bool isStudent = false, bool isTeacher = false) {
            Mock<ICurrentSession> session = new();
            session.Setup(s => s.IsAuthorized()).Returns(isAuthorized);
            session.Setup(s => s.IsUserAStudent()).Returns(isStudent);
            session.Setup(s => s.IsUserATeacher()).Returns(isTeacher);
            return session;
        }

        [Fact]
        public void IndexGet_ReturnsErrorView_UserIsNotAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession();
            LoginController controller = new(mockSession.Object);
            ViewResult result = controller.Index() as ViewResult;
            Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Error");
        }

        [Fact]
        public void IndexGet_ReturnsStudentIndex_UserIsStudent_UserAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isStudent: true);
            LoginController controller = new(mockSession.Object);
            RedirectToActionResult result = (RedirectToActionResult)controller.Index();
            string expected = "Student/Index";
            string resultUrl = $"{result.ControllerName}/{result.ActionName}";
            Assert.Equal(expected, resultUrl);
        }

        [Fact]
        public void IndexGet_ReturnsTeacherIndex_UserIsTeacher_UserAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            LoginController controller = new(mockSession.Object);
            RedirectToActionResult result = (RedirectToActionResult)controller.Index();
            string expected = "Teacher/Index";
            string resultUrl = $"{result.ControllerName}/{result.ActionName}";
            Assert.Equal(expected, resultUrl);
        }

        [Fact]
        public async Task IndexPost_ReturnsLoginIndex_InvalidCredentials() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            LoginController controller = new(mockSession.Object);
            controller.ModelState.AddModelError("", "");
            Login loginCredentials = new() { Username = "Not valid" };
            IActionResult result = await controller.Index(loginCredentials);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            string expected = "Login/Index";
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == expected);
        }

        [Fact]
        public async Task IndexPost_ReturnsTeacherIndex_UserIsTeacher_ValidCredentials() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            LoginController controller = new(mockSession.Object);
            Login loginCredentials = new() { Username = "userte", Password = "cs@123test!" };

            IActionResult result = await controller.Index(loginCredentials);
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            string expected = "Teacher/Index";
            string resultUrl = $"{redirectToActionResult.ControllerName}/{redirectToActionResult.ActionName}";
            Assert.Equal(resultUrl, expected);

        }

        [Fact]
        public async Task IndexPost_ReturnsStudentIndex_UserIsStudent_ValidCredentials() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isStudent: true);
            LoginController controller = new(mockSession.Object);
            Login loginCredentials = new() { Username = "3333333", Password = "cs@123test!" };

            IActionResult result = await controller.Index(loginCredentials);
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            string expected = "Student/Index";
            string resultUrl = $"{redirectToActionResult.ControllerName}/{redirectToActionResult.ActionName}";
            Assert.Equal(resultUrl, expected);
        }

        [Fact]
        public void Logout_ReturnIndex() {
            Mock<ICurrentSession> mockSession = CreateSession();
            LoginController controller = new(mockSession.Object);
            RedirectToActionResult result = controller.Logout() as RedirectToActionResult;
            Assert.True(result.ActionName == "Index");
        }
    }
}
