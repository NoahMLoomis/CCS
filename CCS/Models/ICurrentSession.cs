using System.Threading.Tasks;

namespace CCS.Models {
    public interface ICurrentSession {
        Task Login(Login login);
        bool IsAuthorized();
        bool IsUserAStudent();
        bool IsUserATeacher();
        void SetSemesterName(string semesterName);
        void SetSemesterId(string semesterId);
        int GetEmployeeId();
        int GetStudentId();
        string GetFullName();
        string GetUsername();
        string GetFirstName();
        string GetLastName();
        string GetSemesterId();
        string GetSemesterName();
        void Logout();
        string GetPreviousPage();
        void SetPreviousPage(string previousPage);
    }
}