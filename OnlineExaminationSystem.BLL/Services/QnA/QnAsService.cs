using Microsoft.Extensions.Logging;
using OnlineExaminationSystem.BusinessLogic.Services.Exam;
using OnlineExaminationSystem.DataAccess;
using OnlineExaminationSystem.DataAccess.UnitOfWork;
using OnlineExaminationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.BusinessLogic.Services.QnA
{
    public class QnAsService : IQnAsService
    {
        IUnitOfWork _unitOfWork;
        ILogger<QnAsService> _ilogger;

        public QnAsService(IUnitOfWork unitOfWork, ILogger<QnAsService> ilogger)
        {
            _unitOfWork = unitOfWork;
            _ilogger = ilogger;
        }

        public async Task<QnAsViewModel> AddQnAAsync(QnAsViewModel qnaViewModel)
        {
            try
            {
                QnAs obj = qnaViewModel.ConvertViewModel(qnaViewModel);
                await _unitOfWork.GenericRepository<QnAs>().AddAsync(obj);
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                return null;
            }
            return qnaViewModel;
        }

        public PagedResult<QnAsViewModel> GetAllQnAs(int pageNumber, int pageSize)
        {
            var model = new QnAsViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                List<QnAsViewModel> detailList = new List<QnAsViewModel>();
                var modelList = _unitOfWork.GenericRepository<QnAs>().GetAll()
                    .Skip(ExcludeRecords).Take(pageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<QnAs>().GetAll().ToList();

                detailList = QnAsListInfo(modelList);
                if (detailList != null)
                {
                    model.QnAsList = detailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            var result = new PagedResult<QnAsViewModel>
            {
                Data = model.QnAsList,
                TotalItems = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return result;
        }

        private List<QnAsViewModel> QnAsListInfo(List<QnAs> modelList)
        {
            return modelList.Select(o => new QnAsViewModel(o)).ToList();
        }

        public bool isExamAttend(int examId, int studentId)
        {
            try
            {
                var qnaRecord = _unitOfWork.GenericRepository<ExamResults>().GetAll()
                    .FirstOrDefault(x => x.ExamsId == examId && x.StudentsId == studentId);
                return qnaRecord == null ? false : true;
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            return false;
        }

        public IEnumerable<QnAsViewModel> GetAllQnAByExam(int examId)
        {
            try
            {
                var qnas = _unitOfWork.GenericRepository<QnAs>().GetAll().Where(x => x.ExamsId==examId);
                return QnAsListInfo(qnas.ToList());
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            return Enumerable.Empty<QnAsViewModel>();
        }
    }
}
