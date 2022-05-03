using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCS.Models {
    public partial class Language {
        private readonly CCSContext _context;
        public Language() {
            ChallengeLanguage = new HashSet<ChallengeLanguage>();
            DataType = new HashSet<DataType>();
            _context = new CCSContext();
        }

        public decimal LanguageId { get; set; }
        public string LanguageName { get; set; }

        public virtual ICollection<ChallengeLanguage> ChallengeLanguage { get; set; }
        public virtual ICollection<DataType> DataType { get; set; }

        public async Task<IEnumerable<Language>> GetAllLanguages() {
            return await _context.Language.ToListAsync();
        }
    }
}
