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
using CTPTest.Helpers;

namespace CCSTest.UnitTests.Controllers {
    public class TeacherControllerTest {

        private Mock<ICurrentSession> CreateSession(bool isAuthorized = false, bool isStudent = false, bool isTeacher = false) {
            Mock<ICurrentSession> session = new();
            session.Setup(s => s.IsAuthorized()).Returns(isAuthorized);
            session.Setup(s => s.IsUserAStudent()).Returns(isStudent);
            session.Setup(s => s.IsUserATeacher()).Returns(isTeacher);

            return session;
        }

        [Fact]
        public async Task IndexGet_ReturnsLoginIndex_UserIsNotAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession();
            TeacherController controller = new(mockSession.Object);
            RedirectToActionResult result = await controller.Index() as RedirectToActionResult;
            string expected = "Login/Index";
            string resultUrl = $"{result.ControllerName}/{result.ActionName}";
            Assert.Equal(expected, resultUrl);
        }

        [Fact]
        public async Task IndexGet_ReturnsStudentIndex_UserIsStudent_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isStudent: true);
            TeacherController controller = new(mockSession.Object);
            RedirectToActionResult result = await controller.Index() as RedirectToActionResult;
            string expected = "Student/Index";
            string resultUrl = $"{result.ControllerName}/{result.ActionName}";
            Assert.Equal(expected, resultUrl);
        }
        
        [Fact]
        public async Task IndexGet_ReturnsTeacherIndex_UserIsTeacher_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            TeacherController controller = new(mockSession.Object);
            ViewResult result = await controller.Index() as ViewResult;
            Assert.True(string.IsNullOrEmpty(result.ViewName));
        }

        [Fact]
        public async Task FilterChallengeTitlePost_ReturnsIndexWithTeacher_UserIsTeacher_UserIsAuthorized() {
            Mock<ICurrentSession> mockSession = CreateSession(isAuthorized: true, isTeacher: true);
            TeacherController controller = new(mockSession.Object);
            string expectedViewName = "Index";
            ViewResult result = await controller.FilterChallengeTitle(0, "simple") as ViewResult;
            Assert.True(result.ViewName == expectedViewName);
            Assert.IsType<Teacher>(result.Model);
        }
    }
}
