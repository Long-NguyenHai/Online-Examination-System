using Microsoft.Extensions.Logging;
using OnlineExaminationSystem.BusinessLogic.Services.Student;
using OnlineExaminationSystem.DataAccess;
using OnlineExaminationSystem.DataAccess.UnitOfWork;
using OnlineExaminationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.BusinessLogic.Services.Account
{
    public class AccountService : IAccountService
    {
        IUnitOfWork _unitOfWork;
        ILogger<StudentService> _ilogger;

        public AccountService(IUnitOfWork unitOfWork, ILogger<StudentService> ilogger)
        {
            _unitOfWork = unitOfWork;
            _ilogger = ilogger;
        }

        public bool AddTeacher(UserViewModel userViewModel)
        {
            try
            {
                Users obj = new Users()
                {
                    Name = userViewModel.UserName,
                    UserName = userViewModel.UserName,
                    Password = userViewModel.Password,
                    Role = (int)EnumRoles.Teacher
                };
                _unitOfWork.GenericRepository<Users>().AddAsync(obj);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
                return false;
            }
            return true;
        }

        public PagedResult<UserViewModel> GetAllTeachers(int pageNumber, int pageSize)
        {
            var model = new UserViewModel();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;
                List<UserViewModel> detailList = new List<UserViewModel>();
                var modelList = _unitOfWork.GenericRepository<Users>().GetAll()
                    .Where(x => x.Role == (int)EnumRoles.Teacher)
                    .Skip(ExcludeRecords).Take(pageSize).ToList();

                detailList = ListInfo(modelList);
                if (detailList != null)
                {
                    model.UserList = detailList;
                    model.TotalCount = _unitOfWork.GenericRepository<Users>().GetAll()
                        .Count(x => x.Role == (int)EnumRoles.Teacher);
                }
            }
            catch (Exception ex)
            {

                _ilogger.LogError(ex.Message);
            }
            var result = new PagedResult<UserViewModel>
            {
                Data = model.UserList,
                TotalItems = model.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return result;

        }

        private List<UserViewModel> ListInfo(List<Users> modelList)
        {
            return modelList.Select(o => new UserViewModel(o)).ToList();
        }

        public LoginViewModel Login(LoginViewModel loginViewModel)
        {
            if(loginViewModel.Role == (int)EnumRoles.Admin || loginViewModel.Role == (int)EnumRoles.Teacher || loginViewModel.Role == (int)EnumRoles.HeadOfSubject)
            {
                var user = _unitOfWork.GenericRepository<Users>().GetAll()
                    .FirstOrDefault(a => a.UserName == loginViewModel.UserName.Trim() &&
                    a.Password == loginViewModel.Password.Trim() && 
                    a.Role == loginViewModel.Role);
                if(user != null)
                {
                    loginViewModel.Id = user.Id;
                    return loginViewModel;
                }
            }
            else
            {
                var student = _unitOfWork.GenericRepository<Students>().GetAll()
                    .FirstOrDefault(a => a.UserName == loginViewModel.UserName.Trim() &&
                    a.Password == loginViewModel.Password.Trim());
                if (student != null)
                {
                    loginViewModel.Id = student.Id;
                }
                return loginViewModel;
            }
            return null;
        }
    }
}
