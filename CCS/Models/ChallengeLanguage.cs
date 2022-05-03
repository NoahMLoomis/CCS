using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CCS.Models {
    public partial class ChallengeLanguage {
        private readonly CCSContext _context;
        public ChallengeLanguage() {
            TestCase = new HashSet<TestCase>();
            CodeSubmission = new HashSet<CodeSubmission>();
            _context = new CCSContext();
        }

        public ChallengeLanguage(CCSContext context) {
            TestCase = new HashSet<TestCase>();
            CodeSubmission = new HashSet<CodeSubmission>();
            _context = context;
        }
        public decimal ChallengeLanguageId { get; set; }
        public decimal LanguageId { get; set; }
        public decimal ChallengeId { get; set; }

        public virtual Challenge Challenge { get; set; }
        public virtual Language Language { get; set; }
        public virtual ICollection<TestCase> TestCase{ get; set; }
        public virtual ICollection<Parameter> Parameters { get; set; }
        public virtual ICollection<CodeSubmission> CodeSubmission { get; set; }

        public async Task<ChallengeLanguage> GetChallengeLanguage(decimal challengeLanguageId) {
            return await _context.ChallengeLanguage
                .FindAsync(challengeLanguageId);
        }

        public async Task<decimal> GetChallengeIdFromCL(decimal challengeLanguageId) {
            var cl = await GetChallengeLanguage(challengeLanguageId);

            return cl.ChallengeId;
        }

        public async Task<decimal> GetChallengeLanguageIdFromChallenge(decimal challengeId) {
            var cl = await _context.ChallengeLanguage.Where(cl => cl.ChallengeId == challengeId).FirstAsync();

            return cl.ChallengeLanguageId;
        }
    }
}
