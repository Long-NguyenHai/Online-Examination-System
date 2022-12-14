using OnlineExaminationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.BusinessLogic.Services.Account
{
    public interface IAccountService
    {
        LoginViewModel Login(LoginViewModel loginViewModel);
        bool AddTeacher(UserViewModel userViewModel);
        PagedResult<UserViewModel> GetAllTeachers(int pageNumber, int pageSize);
    }
}
