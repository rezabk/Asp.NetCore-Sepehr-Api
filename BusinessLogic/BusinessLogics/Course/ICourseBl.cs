using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO.Course;
using DAL.Model;
using DAL.Model.CourseModels;
using Data;

namespace BusinessLogic.BusinessLogics.Course
{
    public interface ICourseBl
    {
        Task<StandardResult> GetAllCourses();
        Task<StandardResult> GetCourseById(int courseId);

        Task<StandardResult> CreateCourse(CreateCourseDto dto,int id);  
        
        Task<StandardResult> UpdateCourse(CreateCourseDto dto,int userId, int courseid); 
        Task<StandardResult> RemoveCourse(int userId, int courseid);

        Task<StandardResult> Availability(int courseId, int userId);
    



    }
}
