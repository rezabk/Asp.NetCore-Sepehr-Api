using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Lesson
{
    public class ShowLessonDto
    {
        public int Id { get; set; }
        public string LessonName { get; set; }

        public List<string> LessonTopics { get; set; }

        public string Description { get; set; }

        public string CreationDate { get; set; }

        public bool IsActive { get; set; }
    }
}
