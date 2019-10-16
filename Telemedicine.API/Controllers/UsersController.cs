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

        [HttpPost("{id}/relationship/{recipientId}")]
        public async Task<IActionResult> CreateRelationship(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var relationship = await _repo.GetRelationship(id, recipientId);

            if (relationship != null)
                return BadRequest("You already have a relationship with this user");

            if (await _repo.getUser(recipientId) == null)
                return NotFound();

            relationship = new Relationship
            {
                PatientId = id,
                DoctorId = recipientId
            };

            _repo.Add<Relationship>(relationship);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to create relationship");
        }
    }
}