using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCS.Models {
    public partial class CodeSubmission {
        private readonly CCSContext _context;
        private readonly HtmlSanitizer sanitizer;

        public CodeSubmission() {
            _context = new CCSContext();
            Result = new HashSet<Result>();
            sanitizer = new HtmlSanitizer();
        }

        public decimal CodeSubmissionId { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Code submission must be between 2 and 1000 characters")]
        public string Code { get; set; }
        public decimal StudentId { get; set; }
        public decimal ChallengeLanguageId { get; set; }
        public bool Status { get; set; }
        public DateTime LastAttempted { get; set; }

        public virtual ChallengeLanguage ChallengeLanguage { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<Result> Result { get; set; }

        public void SanitizeCodeSubmission() {
            Code = sanitizer.Sanitize(Code);
        }
    }
}
