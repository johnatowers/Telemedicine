using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.API.Data;
using AutoMapper;
using Telemedicine.API.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using System;
using Telemedicine.API.Helpers;
using Telemedicine.API.Models;

namespace Telemedicine.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] // anytime these methods are called, Last Action is updated
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ITelemedRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(ITelemedRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            userParams.UserId = currentUserId;

            var users = await _repo.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.getUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.getUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Update user {id} failed on save");       
        }

        [HttpPost("{id}/select/{recipientId}")]
        public async Task<IActionResult> SelectUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var select = await _repo.GetSelect(id, recipientId);

            if (select != null) {
                return BadRequest("You've already selected this user");
            }

            if (await _repo.getUser(recipientId) == null) {
                return NotFound();
            }

            var checkIfPatientFirstSelect = await _repo.GetSelect(recipientId, id);
            var SelectorUser = await _repo.getUser(id);
            var SelecteeUser = await _repo.getUser(recipientId);
            // // Check if a patient is trying to select a patient
            // if (SelectorUser.UserRole.RoleId == 1 && SelecteeUser.UserRole.RoleId == 1)
            //     return BadRequest("You can't select another patient");

            // // Check if a doctor is trying to select a patient
            // if (SelectorUser.UserRole.RoleId == 3 && SelecteeUser.UserRole.RoleId == 1)
            //     // Check if patient has selected the doctor first
            //     // If patient has not selected the doctor, then the doctor cannot select the patient
            //     if (checkIfPatientFirstSelect == null)
            //         return BadRequest("You can't select a patient first");

            select = new Select
            {
                SelectorId = id,
                SelecteeId = recipientId
            };

            _repo.Add<Select>(select);

            if (await _repo.SaveAll())
                return Ok();
            
            return BadRequest("Failed to select user");
            
        }
    }
}