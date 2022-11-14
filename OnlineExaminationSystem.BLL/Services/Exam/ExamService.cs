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

namespace OnlineExaminationSystem.BusinessLogic.Services.Exam
{
    public class ExamService : IExamService
    {
        IUnitOfWork _unitOfWork;
        ILogger<ExamService> _ilogger;

        public ExamService(IUnitOfWork unitOfWork, ILogger<ExamService> ilogger)
        {
            _unitOfWork = unitOfWork;
            _ilogger = ilogger;
        }

        public async Task<ExamViewModel> AddExamAsync(ExamViewModel examViewModel)
        {
            try
            {
                Exams obj = examViewModel.ConvertViewModel(examViewModel);
                await _unitOfWork.GenericRepository<Exams>().AddAsync(obj);
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                return null;
            }
            return examViewModel;
        }

        public PagedResult<ExamViewModel> GetAllExams(int pageNumber, int pageSize)
        {
            var model = new ExamViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                List<ExamViewModel> detailList = new List<ExamViewModel>();
                var modelList = _unitOfWork.GenericRepository<Exams>().GetAll()
                    .Skip(ExcludeRecords).Take(pageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<Exams>().GetAll().ToList();

                detailList = ExamListInfo(modelList);
                if (detailList != null)
                {
                    model.ExamList = detailList;
                    model.TotalCount = totalCount.Count();
                }
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            var result = new PagedResult<ExamViewModel>
            {
                Data = model.ExamList,
                TotalItems = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return result;
        }

        private List<ExamViewModel> ExamListInfo(List<Exams> modelList)
        {
            return modelList.Select(o => new ExamViewModel(o)).ToList();
        }

        public IEnumerable<Exams> GetAllExams()
        {
            try
            {
                var exams = _unitOfWork.GenericRepository<Exams>().GetAll();
                return exams;
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            return Enumerable.Empty<Exams>();
        }
    }
}
