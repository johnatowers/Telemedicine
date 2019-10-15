using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telemedicine.API.Data;
using Telemedicine.API.Dtos;
using Telemedicine.API.Models;

namespace Telemedicine.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        public AdminController(DataContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var userList = await _context.Users
            .OrderBy(x => x.UserName)
            .Select(user => new 
            {
                Id = user.Id,
                UserName = user.UserName,
                Role = user.UserRole.Role.ToString()
                
            })
            .ToListAsync();

            return Ok(userList);
        }

        //[Authorize(Policy = "RequireAdminRole")]
        // edit roles for a user
        //[HttpPost("editRoles/{username}")]
        //public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        //{
            //var user = await _userManager.FindByNameAsync(userName);
            //var userRoles = await _userManager.GetRolesAsync(user);
            //var selectedRole = roleEditDto.RoleName;
            // User could be removed from all roles
            // alternative code: 
            // selectedRoles = selectedRoles != null ? selectedRoles : new string[] {};
            //selectedRole = selectedRole ?? "";
            // add user to roles they're not already in
            //var result = await _userManager.AddToRolesAsync(user, selectedRole.Except(userRoles));
            //if (!result.Succeeded)
            //    return BadRequest("Failed to add to roles");

            // remove roles deselected
            //result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            //if (!result.Succeeded)
            //    return BadRequest("Failed to remove the roles");

            //return Ok(await _userManager.GetRolesAsync(user));
        //}

        // Kept here for help with adding something similar later
        //[Authorize(Policy = "PhotoRole")]
        //[HttpGet("photoRole")]
        //public IActionResult GetPhotosForAdmin() {
        //    return Ok("Admins can see this");
        //}
    }
}