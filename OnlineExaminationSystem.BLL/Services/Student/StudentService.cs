using Microsoft.Extensions.Logging;
using OnlineExaminationSystem.DataAccess;
using OnlineExaminationSystem.DataAccess.UnitOfWork;
using OnlineExaminationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.BusinessLogic.Services.Student
{
    public class StudentService : IStudentService
    {
        IUnitOfWork _unitOfWork;
        ILogger <StudentService> _ilogger;

        public StudentService(IUnitOfWork unitOfWork, ILogger<StudentService> logger)
        {
            _unitOfWork = unitOfWork;
            _ilogger = logger;
        }

        public async Task<StudentViewModel> AddStudentAsync(StudentViewModel studentViewModel)
        {
            try
            {
                Students obj = studentViewModel.ConvertViewModel(studentViewModel);
                await _unitOfWork.GenericRepository<Students>().AddAsync(obj);
                _unitOfWork.Save();
            }
            catch(Exception)
            {
                return null;
            }
            return studentViewModel;
        }

        public PagedResult<StudentViewModel> GetAllStudents(int pageNumber, int pageSize)
        {
            var model = new StudentViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                List<StudentViewModel> detailList = new List<StudentViewModel>();
                var modelList = _unitOfWork.GenericRepository<Students>().GetAll()
                    .Skip(ExcludeRecords).Take(pageSize).ToList();
                var totalCount = _unitOfWork.GenericRepository<Students>().GetAll().ToList();

                detailList = GroupListInfo(modelList);
                if(detailList != null)
                {
                    model.StudentList = detailList;
                    model.TotalCount = totalCount.Count();
                }    
            }
            catch(Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            var result = new PagedResult<StudentViewModel>
            {
                Data = model.StudentList,
                TotalItems = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return result;
        }

        private List<StudentViewModel> GroupListInfo(List<Students> modelList)
        {
            return modelList.Select(o => new StudentViewModel(o)).ToList();
        }

        public IEnumerable<Students> GetAllStudents()
        {
            try
            {
                var students = _unitOfWork.GenericRepository<Students>().GetAll();
                return students;
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            return Enumerable.Empty<Students>();
        }

        public IEnumerable<ResultViewModel> GetExamResults(int studentId)
        {
            try
            {
                var examResults = _unitOfWork.GenericRepository<ExamResults>().GetAll()
                    .Where(a => a.StudentsId == studentId);
                var students = _unitOfWork.GenericRepository<Students>().GetAll();
                var exams = _unitOfWork.GenericRepository<ExamResults>().GetAll();
                var qnas = _unitOfWork.GenericRepository<QnAs>().GetAll();

                var requiredData = examResults.Join(
                    students,
                    er => er.StudentsId,
                    s => s.Id,
                    (er, st) => new { er, st })
                    .Join(exams, erj => erj.er.ExamsId, ex => ex.Id, (erj, ex) => new { erj, ex })
                    .Join(qnas, exj => exj.erj.er.QnAsId, q => q.Id, 
                    (exj, q) => new ResultViewModel(){

                    StudentID = studentId,
                    ExamName = exj.ex.Title,
                    TotalQuestion = examResults.Count(
                        a => a.StudentsId == studentId && a.ExamsId==exj.ex.Id), 
                        CorrectAnswer = examResults.Count(
                            a => a.StudentsId==studentId && a.ExamsId==exj.ex.Id && a.Answer==q.Answer),
                        WrongAnswer = examResults.Count(
                            a=> a.StudentsId == studentId && a.ExamsId==exj.ex.Id && a.Answer!=q.Answer),
                    });;
                return requiredData;
            }
            catch (Exception ex)    
            {
                _ilogger.LogError(ex.Message);
            }
            return Enumerable.Empty<ResultViewModel>();
        }

        public StudentViewModel GetStudentDetails(int studentId)
        {
            try
            {
                var student = _unitOfWork.GenericRepository<Students>().GetByID(studentId);
                return student != null ? new StudentViewModel(student) : null;
            }
            catch (Exception ex)
            {

                _ilogger.LogError(ex.Message, ex);  
            }
            return null;
        }

        public bool SetExamResult(AttendExamViewModel attendExamViewModel)
        {
            try
            {
                foreach(var item in attendExamViewModel.QnAs)
                {
                    ExamResults examResults = new ExamResults();
                    examResults.StudentsId = attendExamViewModel.StudentID;
                    examResults.QnAsId = item.Id;
                    examResults.ExamsId = item.ExamsId;
                    examResults.Answer = item.SelectedAnswer;
                    _unitOfWork.GenericRepository<ExamResults>().AddAsync(examResults);
                }
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message, ex);
            }
            return false;
        }

        public bool SetGroupIdToStudent(GroupViewModel groupViewModel)
        {
            try
            {
                foreach (var item in groupViewModel.StudentCheckLists)
                {
                    var student = _unitOfWork.GenericRepository<Students>().GetByID(item.Id);
                    if(item.Selected)
                    {
                        student.GroupsId = groupViewModel.Id;
                        _unitOfWork.GenericRepository<Students>().Update(student);
                    }
                    else
                    {
                        if(student.GroupsId == groupViewModel.Id)
                        {
                            student.GroupsId = null;
                        }
                    }
                    _unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {

                _ilogger.LogError(ex.Message, ex);
            }
            return false;
        }

        public async Task<StudentViewModel> UpdateAsync(StudentViewModel studentViewModel)
        {
            try
            {
                Students obj = _unitOfWork.GenericRepository<Students>().GetByID(studentViewModel.Id);
                obj.Name = studentViewModel.Name;
                obj.UserName = studentViewModel.UserName;
                obj.PictureFileName = studentViewModel.PictureFileName != null ? 
                    studentViewModel.PictureFileName : obj.PictureFileName;
                obj.CVFileName = studentViewModel.CVFileName != null ? 
                    studentViewModel.CVFileName : obj.CVFileName;
                obj.Contact = studentViewModel.Contact;
                await _unitOfWork.GenericRepository<Students>().UpdateAsync(obj);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
            }
            return studentViewModel;
        }
    }
}
