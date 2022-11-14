using OnlineExaminationSystem.DataAccess;
using OnlineExaminationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.BusinessLogic.Services.Student
{
    public interface IStudentService
    {
        PagedResult<StudentViewModel> GetAllStudents(int pageNumber, int pageSize);
        Task<StudentViewModel> AddStudentAsync(StudentViewModel studentViewModel);
        IEnumerable<Students> GetAllStudents();
        bool SetGroupIdToStudent(GroupViewModel groupViewModel);
        bool SetExamResult(AttendExamViewModel attendExamViewModel);
        IEnumerable<ResultViewModel> GetExamResults(int studentId);
        StudentViewModel GetStudentDetails(int studentId);
        Task<StudentViewModel> UpdateAsync(StudentViewModel studentViewModel);
    }
}
