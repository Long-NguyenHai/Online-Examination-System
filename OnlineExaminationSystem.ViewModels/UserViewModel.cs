using OnlineExaminationSystem.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(Users users)
        {
            Id = users.Id;
            Name = users.Name ?? "";
            UserName = users.UserName;
            Password = users.Password;
            Role = users.Role;
        }

        public UserViewModel()
        {
        }

        public Users ConvertViewModel(UserViewModel vm)
        {
            return new Users
            {
                Id = vm.Id,
                Name = vm.Name ?? "",
                UserName = vm.UserName,
                Password = vm.Password,
                Role = vm.Role,
            };
        }

        public int Id { get; set; }
        [Required]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public int Role { get; set; }
        public List<UserViewModel> UserList { get; set; }
        public int TotalCount { get; set; }
    }
}