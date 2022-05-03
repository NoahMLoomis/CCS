using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCS.Controllers;
using CCS.Models;
using CTPTest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CCSTest.UnitTests.Models {
    public class TeacherModelTest {
        private readonly TestSetup _setup;

        public TeacherModelTest() {
            _setup = new();
        }

        [Fact]
        public async Task GetTeacher_TeacherExists() {
            using var context = _setup.CreateContext();
            Teacher _teacher = new Teacher(context);
            Teacher teacher = await _teacher.GetTeacherById(1);
            Assert.NotNull(teacher);
        }

        [Fact]
        public async Task GetTeacher_TeacherDoesntExist() {
            using var context = _setup.CreateContext();
            Teacher _teacher = new Teacher(context);
            Teacher teacher = await _teacher.GetTeacherById(0);
            Assert.Null(teacher);
        }

        [Fact]
        public async Task AddTeacher_Success() {
            using var context = _setup.CreateContext();
            Teacher _teacher = new Teacher(context);
            Teacher testTeacher = new Teacher();
            await _teacher.AddTeacher(testTeacher);
            int expected = 4;
            int result = context.Teacher.Count();
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task AddTeacher_Fail() {
            using var context = _setup.CreateContext();
            Teacher _teacher = new Teacher(context);
            Teacher testTeacher = new Teacher { TeacherId = 1};
            await Assert.ThrowsAsync<DbUpdateException>(() => _teacher.AddTeacher(testTeacher));
        }

        [Fact]
        public async Task FilterChallengeTitle_TeacherDoesNotExist() {
            using var context = _setup.CreateContext();
            Teacher _teacher = new Teacher(context);
            List<Challenge> challenges = await _teacher.FilterChallengeTitle(0, "A Challenge");
            int expected = 0;
            int result = challenges.Count();
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task FilterChallengeTitle_ChallengeTitleIsEmpty() {
            using var context = _setup.CreateContext();
            Teacher _teacher = new Teacher(context);
            List<Challenge> challenges = await _teacher.FilterChallengeTitle(1, "");
            int expected = 3;
            int result = challenges.Count();
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task FilterChallengeTitle_ReturnsRightChallenge() {
            using var context = _setup.CreateContext();
            Teacher _teacher = new Teacher(context);
            List<Challenge> challenges = await _teacher.FilterChallengeTitle(1, "Simple addition");
            int expected = 1;
            int result = challenges.Count();
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task FilterChallengeTitle_ReturnsNoChallenge() {
            using var context = _setup.CreateContext();
            Teacher _teacher = new Teacher(context);
            List<Challenge> challenges = await _teacher.FilterChallengeTitle(1, "Does not exist");
            int expected = 0;
            int result = challenges.Count();
            Assert.Equal(expected, result);
        }
    }
}
