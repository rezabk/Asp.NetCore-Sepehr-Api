using System.Runtime.CompilerServices;
using BusinessLogic.BusinessLogics;
using DAL.DTO;
using DAL.DTO.Admin;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;

namespace FinalPorject.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminBl _bl;
       
        public AdminController(IAdminBl bl)
        {
            _bl = bl;
        }


        [Route("api/admin/user/getall")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allUser = await _bl.GetAllUsers();
          return StatusCode(allUser.StatusCode, allUser);
          
        }
        [Route("api/admin/user/getbyid/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _bl.GetUserById(id);
            return StatusCode(res.StatusCode, res);

        }

        [Route("api/admin/user/create")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(AdminCreateUserDto dto)
        {
            var res = await _bl.CheckCreateUser(dto);
            return StatusCode(res.StatusCode, res);
        }

        [Route("api/admin/user/edit/{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser(AdminUpdateUserDto dto, int id)
        {
            var res = await _bl.CheckUpdateUser(dto, id);
            return StatusCode(res.StatusCode, res);
        }

        [Route("api/admin/user/delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveUser(int id)
        {
            var res = await _bl.CheckRemoveUser(id);
            return StatusCode(res.StatusCode, res);
        }

        [Route("api/admin/user/becomemaster/{id}")]
        [HttpPatch]

        [Route("api/admin/user/availablity/{id}")]
        [HttpPatch]
        public async Task<IActionResult> Availablity(int id)
        {
            var res = await _bl.Availablity(id);
            return StatusCode(res.StatusCode, res);
        }


        [Route("api/admin/user/becomemaster/getall")]
        [HttpGet]
        public async Task<IActionResult> GetAllBecomeMasterRequests()
        {
            var res = await _bl.GetAllBecomeMasterRequests();
            return StatusCode(res.StatusCode, res);
        }


        [Route("api/admin/user/becomemaster/getbyid/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetAllBecomeMasterRequests(int id)
        {
            var res = await _bl.GetBecomeMasterRequestById(id);
            return StatusCode(res.StatusCode, res);
        }

        [Route("api/admin/user/becomemaster/{id}/approvement")]
        [HttpPost]
        public async Task<IActionResult> ApproveMasterById(int id)
        {
            var res = await _bl.ApproveMasterById(id);
            return StatusCode(res.StatusCode, res);
        }

        [Route("api/admin/course/create")]
        [HttpPost]
        public async Task<IActionResult> CreateCourse(AdminCreateCourseDto dto)
        {
            var res = await _bl.CreateCourse(dto);
            return StatusCode(res.StatusCode, res);
        }

        [Route("api/admin/course/edit/{courseid}")]
        [HttpPut]
        public async Task<IActionResult> UpdateCourse(AdminUpdateCourseDto dto, int courseid)
        {
            var res = await _bl.UpdateCourse(dto, courseid);
            return StatusCode(res.StatusCode, res);
        }



        [Route("api/admin/course/{courseid}/adduser/{userid}")]
        [HttpPatch]
        public async Task<IActionResult> AddUserToCourse(int courseid, int userid)
        {
            var res = await _bl.AddUserToCourse(courseid, userid);
            return StatusCode(res.StatusCode, res);
        }

        [Route("api/admin/course/{courseid}/removeuser/{userid}")]
        [HttpPatch]
        public async Task<IActionResult> RemoveUserFromCourse(int courseid, int userid)
        {
            var res = await _bl.RemoveUserFromCourse(courseid, userid);
            return StatusCode(res.StatusCode, res);
        }

        [Route("api/admin/setting")]
        [HttpGet]
        public async Task<IActionResult> GetAllSettings()
        {
            var res = await _bl.GetAllSettings();
            return StatusCode(res.StatusCode, res);
        }


        [Route("api/admin/setting/edit")]
        [HttpPut]
        public async Task<IActionResult> GetAllSettings(UpdateSettingsDto dto)
        {
            var res = await _bl.UpdateSettings(dto);
            return StatusCode(res.StatusCode, res);
        }

    }
}
