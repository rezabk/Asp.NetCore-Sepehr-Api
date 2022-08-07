using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Lesson
{
    public class CreateLessonDto
    {

        [Required(ErrorMessage = "لطفا نام درس را وارد کنید")]
        public string LessonName { get; set; }

        [Required(ErrorMessage = "لطفا تاپیک های درس را وارد کنید")]
        public List<string> LessonTopics { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "لطفا وضعیت درس را تعیین کنید")]
        public bool IsActive { get; set; }

    }
}
