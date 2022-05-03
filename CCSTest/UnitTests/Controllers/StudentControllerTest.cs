using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCS.Controllers;
using CCS.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CCSTest.UnitTests.Controllers {
    public class ChallengeModelTest {
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
            StudentController mockController = new(mockSession.Object);
            ViewResult result = await mockController.Index() as ViewResult;
            Assert.True(string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public async Task IndexGet_ReturnsTeacherView_UserIsTeacher_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            StudentController mockController = new(mockSession.Object);
            RedirectToActionResult result = (RedirectToActionResult) await mockController.Index();
            string expected = "Teacher/Index";
            string resultUrl = $"{result.ControllerName}/{result.ActionName}";
            Assert.Equal(expected, resultUrl);
        }

        [Fact]
        public async Task IndexGet_ReturnsLogin_UserIsNotAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession();
            StudentController mockController = new(mockSession.Object);
            RedirectToActionResult result = (RedirectToActionResult) await mockController.Index();
            string expected = "Login/Index";
            string resultUrl = $"{result.ControllerName}/{result.ActionName}";
            Assert.Equal(expected, resultUrl);
        }

        [Fact]
        public async Task AllChallenges_ReturnsTeacherIndex_UserIsTeacher_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            StudentController mockController = new(mockSession.Object);
            RedirectToActionResult result = (RedirectToActionResult)await mockController.Index();
            string expected = "Teacher/Index";
            string resultUrl = $"{result.ControllerName}/{result.ActionName}";
            Assert.Equal(expected, resultUrl);
        }

        [Fact]
        public async Task ToggleOrder_ReturnsAllChallengesView_UserIsStudent_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isStudent: true);
            StudentController mockController = new(mockSession.Object);
            ViewResult result = await mockController.ToggleOrder(true) as ViewResult;
            string expected = "Index";
            string resultUrl = $"{result.ViewName}";
            Assert.Equal(expected, resultUrl);
        }
    }
}
