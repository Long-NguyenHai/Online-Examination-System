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
    public class ExamViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Exam Name")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int Time { get; set; }
        public int GroupsId { get; set; }

        public List<ExamViewModel> ExamList { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Groups> GroupList { get; set; }

        public ExamViewModel(Exams exams)
        {
            Id = exams.Id;
            Title = exams.Title ?? "";
            Description = exams.Description ?? "";
            StartDate = exams.StartDate;
            Time = exams.Time;
            GroupsId = exams.GroupsId;
        }

        public ExamViewModel()
        {
        }

        public Exams ConvertViewModel(ExamViewModel vm)
        {
            return new Exams
            {
                Id = vm.Id,
                Title = vm.Title ?? "",
                Description = vm.Description ?? "",
                StartDate = vm.StartDate,
                Time = vm.Time,
                GroupsId = vm.GroupsId,
            };
        }
    }
}
