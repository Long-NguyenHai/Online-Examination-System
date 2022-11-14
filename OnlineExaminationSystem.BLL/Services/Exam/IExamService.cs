using OnlineExaminationSystem.DataAccess;
using OnlineExaminationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.BusinessLogic.Services.Exam
{
    public interface IExamService
    {
        PagedResult<ExamViewModel> GetAllExams(int pageNumber, int pageSize);
        Task<ExamViewModel> AddExamAsync(ExamViewModel examViewModel);
        IEnumerable<Exams> GetAllExams();
    }
}
