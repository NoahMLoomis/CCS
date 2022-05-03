namespace CCS.Models {
    public partial class Result {

        private readonly CCSContext _context;

        public decimal ResultId { get; set; }
        public bool Passed { get; set; }
        public decimal TestCaseId { get; set; }
        public decimal CodeSubmissionId { get; set; }
        public string CodeOutput { get; set; }
        public virtual CodeSubmission CodeSubmission { get; set; }
        public virtual TestCase TestCase { get; set; }

        public Result() {
            _context = new CCSContext();
        }
    }
}
