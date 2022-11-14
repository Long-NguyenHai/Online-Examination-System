using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.ViewModels
{
    public class AttendExamViewModel
    {
        public int StudentID { get; set; }
        public string ExamName { get; set; }
        public List<QnAsViewModel> QnAs { get; set; }
        public string Message { get; set; }
    }
}
