using OnlineExaminationSystem.DataAccess;
using OnlineExaminationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.BusinessLogic.Services.QnA
{
    public interface IQnAsService
    {
        PagedResult<QnAsViewModel> GetAllQnAs(int pageNumber, int pageSize);
        Task<QnAsViewModel> AddQnAAsync(QnAsViewModel qnaViewModel);
        IEnumerable<QnAsViewModel> GetAllQnAByExam(int examId);
        bool isExamAttend(int examId, int studentId);
    }
}
