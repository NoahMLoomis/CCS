
using Microsoft.EntityFrameworkCore;
﻿using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCS.Models {
    public partial class Student {
        private readonly CCSContext _context;

        public Student() {
            _context = new CCSContext();
            CodeSubmission = new HashSet<CodeSubmission>();
        }

        public Student(CCSContext context) {
            _context = context;
            CodeSubmission = new HashSet<CodeSubmission>();
        }

        public decimal StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<CodeSubmission> CodeSubmission { get; set; }

        public async Task<Student> GetStudent(int studentId) {
            return await _context.Student.Where(x => x.StudentId == studentId).FirstOrDefaultAsync();
        }

        public async Task AddStudent(Student student) {
            await _context.Student.AddAsync(student);
            await _context.SaveChangesAsync();
        }
    }
}
