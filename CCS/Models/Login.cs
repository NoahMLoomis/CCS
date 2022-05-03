using Ganss.XSS;
using System.ComponentModel.DataAnnotations;

namespace CCS.Models {
    public class Login {
        private readonly HtmlSanitizer sanitizer;

        [Display(Name = "Username:")]
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public Login(string username, string password) {
            sanitizer = new HtmlSanitizer();
            Username = sanitizer.Sanitize(username);
            Password = sanitizer.Sanitize(password);
        }

        public Login() {
        }
    }
}
