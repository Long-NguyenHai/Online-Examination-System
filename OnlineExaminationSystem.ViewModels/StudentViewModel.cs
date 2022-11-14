using Microsoft.AspNetCore.Http;
using OnlineExaminationSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnlineExaminationSystem.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Student Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Contact Number")]
        public string Contact { get; set; }
        [Display(Name = "CV")]

        public string CVFileName { get; set; }
        public string PictureFileName { get; set; }
        public int? GroupsId { get; set; }
        public IFormFile PictureFile { get; set; }
        public IFormFile CVFile { get; set; }

        public int TotalCount { get; set; }
        public List<StudentViewModel> StudentList { get; set; }

        public StudentViewModel(Students students)
        {
            Id = students.Id;
            Name = students.Name ?? "";
            UserName = students.UserName;
            Password = students.Password;
            Contact = students.Contact ?? "";
            CVFileName = students.CVFileName ?? "";
            PictureFileName = students.PictureFileName ?? "";
            GroupsId = students.GroupsId;
        }

        public StudentViewModel()
        {
        }

        public Students ConvertViewModel(StudentViewModel vm)
        {
            return new Students
            {
                Id = vm.Id,
                Name = vm.Name ?? "",
                UserName = vm.UserName,
                Password = vm.Password,
                Contact = vm.Contact,
                CVFileName = vm.CVFileName,
                PictureFileName = vm.PictureFileName,
                GroupsId = vm.GroupsId,
            };
        }
    }
}
