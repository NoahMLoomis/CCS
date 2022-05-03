using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CCS.Models {
    public partial class TestCase {

        private readonly CCSContext _context;

        public TestCase() {
            _context = new CCSContext();
            TestCaseParameter = new HashSet<TestCaseParameter>();
            Result = new HashSet<Result>();
        }

        public decimal TestCaseId { get; set; }
        public string ExpectedResult { get; set; }
        public decimal ChallengeLanguageId { get; set; }
        public virtual ChallengeLanguage ChallengeLanguage { get; set; }
        public virtual ICollection<TestCaseParameter> TestCaseParameter { get; set; }
        public virtual ICollection<Result> Result { get; set; }

        [NotMapped]
        public List<string> ParameterValues { get; set; }

        public async Task<List<TestCase>> GetTestCasesForChallenge(decimal challengeId) {
            var cl_id = await new ChallengeLanguage().GetChallengeLanguageIdFromChallenge(challengeId);

            var testCases = await _context.TestCase
                                .Where(tc => tc.ChallengeLanguageId == cl_id)
                                .Include(x => x.TestCaseParameter)
                                .ThenInclude(x => x.Parameter)
                                .ThenInclude(d => d.DataType)
                                .ToListAsync();

            return testCases;
        }

        public async Task<bool> AddTestCase() {
            try {
                await _context.TestCase.AddAsync(this);
                await _context.SaveChangesAsync();
                return true;
            } catch (Exception e) {
                throw e;
            }
        }

        public async Task EditTestCase(TestCase tc) {
            _context.Update(tc);
            await _context.SaveChangesAsync();
        }
    }
}
