using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCS.Models {
    public partial class TestCaseParameter {

        private readonly CCSContext _context;

        public TestCaseParameter() {
            _context = new CCSContext();
        }

        public decimal TestCaseParameterId { get; set; }
        public decimal TestCaseId { get; set; }
        public decimal ParameterId { get; set; }
        public string Value { get; set; }
        //public virtual ChallengeLanguage ChallengeLanguage { get; set; }
        [ForeignKey("TestCaseId")]
        public virtual TestCase TestCase { get; set; }
        [ForeignKey("ParameterId")]
        public virtual Parameter Parameter { get; set; }

        public async Task<List<TestCaseParameter>> GetTCPsForTestCase(decimal testCaseId) {
            return await _context.TestCaseParameter
                .Where(tcp => tcp.TestCaseId == testCaseId)
                .ToListAsync();
        }

        public async Task<bool> AddTestCaseParameter() {
            try {
                await _context.TestCaseParameter.AddAsync(this);
                await _context.SaveChangesAsync();
                return true;
            } catch (Exception e) {
                throw e;
            }
        }

        public async Task EditTestCaseParameter(TestCaseParameter tcp) {
            _context.Update(tcp);
            await _context.SaveChangesAsync();
        }
    }
}
