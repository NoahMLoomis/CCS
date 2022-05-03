using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using LoginService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CCS.Models {
    [ExcludeFromCodeCoverage]
    public class CurrentSession : ICurrentSession {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoginSoapClient _client = new LoginSoapClient(LoginSoapClient.EndpointConfiguration.LoginSoap);
        ISession Session;

        public CurrentSession(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
            Session = _httpContextAccessor.HttpContext.Session;
        }

        public async Task Login(Login login) {

            AuthenticateResponse authenticateResponse = await _client.AuthenticateAsync(login.Username, login.Password);

            if (authenticateResponse.Body.AuthenticateResult) {
                AuthorizeResponse authorizeResponse = await _client.AuthorizeAsync(login.Username, "CCS");

                UserBLL userBLL = authorizeResponse.Body.AuthorizeResult;

                if (!userBLL.HasError) {
                    // default roles to false
                    Session.SetString("IsTeacher", bool.FalseString);
                    Session.SetString("IsStudent", bool.FalseString);

                    foreach (var item in userBLL.RoleList) {
                        string itemCode = item.Code.Trim();

                        // set the roles depending on valid rules
                        if (itemCode.Equals("TE")) { // Teacher
                            Session.SetString("IsTeacher", bool.TrueString);
                            Session.SetString("EmployeeId", userBLL.EmployeeId.ToString());
                        } else if (itemCode.Equals("ST")) { /// Student
                            Session.SetString("StudentId", userBLL.StudentId.ToString());
                            Session.SetString("IsStudent", bool.TrueString);
                        }

                    }
                    if (IsUserAStudent() || IsUserATeacher()) {
                        Session.SetString("IsAuthorized", bool.TrueString);
                        Session.SetString("Username", userBLL.Username);
                        Session.SetString("FirstName", userBLL.FirstName);
                        Session.SetString("LastName", userBLL.LastName);
                        await SetClaims(userBLL);
                    } else {
                        Session.SetString("IsAuthorized", bool.FalseString);
                    }
                }
            }
        }

        public void SetSemesterName(string semesterName) {
            Session.SetString("SemesterName", semesterName);
        }

        public string GetSemesterName() {
            return Session.GetString("SemesterName");
        }

        public string GetSemesterId() {
            return Session.GetString("SemesterId");
        }

        public void SetSemesterId(string semesterId) {
            Session.SetString("SemesterId", semesterId);
        }

        public bool IsAuthorized() {
            if (Session.GetString("IsAuthorized") != null)
                return Convert.ToBoolean(Session.GetString("IsAuthorized"));
            else
                return false;
        }

        public bool IsUserAStudent() {
            return Convert.ToBoolean(Session.GetString("IsStudent"));
        }

        public bool IsUserATeacher() {
            return Convert.ToBoolean(Session.GetString("IsTeacher"));
        }

        public string GetFullName() {
            if (Session.GetString("FirstName") == null || Session.GetString("LastName") == null)
                return "";
            return Session.GetString("FirstName") + " " + Session.GetString("LastName");
        }

        public string GetFirstName() {
            if (Session.GetString("FirstName") == null)
                return "";
            return Session.GetString("FirstName");
        }

        public string GetLastName() {
            if (Session.GetString("LastName") == null)
                return "";
            return Session.GetString("LastName");
        }

        public string GetUsername() {
            return Session.GetString("Username");
        }

        public void Logout() {
            _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public int GetEmployeeId() {
            return Convert.ToInt32(Session.GetString("EmployeeId"));
        }

        public int GetStudentId() {
            return Convert.ToInt32(Session.GetString("StudentId"));
        }

        private async Task SetClaims(UserBLL userBll, bool testing = false) {
            // set the user information in a list of claims
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Surname, userBll.FirstName),
                new Claim(ClaimTypes.GivenName, userBll.LastName),
                new Claim("id", IsUserATeacher() ? GetEmployeeId().ToString() : GetUsername())
            };
            // add the roles to the claims
            claims.AddRange(userBll.RoleList.Select(role => new Claim(ClaimTypes.Role, role.Code.Trim())));
            // specify the authentication type
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public string GetPreviousPage() {
            return Session.GetString("PreviousPage");
        }

        public void SetPreviousPage(string previousPage) {
            Session.SetString("PreviousPage", previousPage);
        }
    }
}
