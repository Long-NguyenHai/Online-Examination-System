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
    public class GroupViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Description")]
        public int UsersId { get; set; }

        public List<GroupViewModel> GroupList { get; set; }

        public int TotalCount { get; set; }
        public List<StudentCheckboxListViewModel> StudentCheckLists { get; set; }
        public int SelectedAnswer { get; set; }


        public GroupViewModel(Groups groups)
        {
            Id = groups.Id;
            Name = groups.Name ?? "";
            Description = groups.Description ?? "";
            UsersId = groups.UsersId;
        }

        public GroupViewModel()
        {
        }

        public Groups ConvertViewModel(GroupViewModel vm)
        {
            return new Groups
            {
                Id = vm.Id,
                Name = vm.Name ?? "",
                Description = vm.Description ?? "",
                UsersId = vm.UsersId,
            };
        }
    }
}
