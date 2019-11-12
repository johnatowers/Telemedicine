using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemedicine.API.Data;
using Telemedicine.API.Dtos;
using Telemedicine.API.Helpers;
using Telemedicine.API.Models;

namespace Telemedicine.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] // anytime these methods are called, Last Action is updated
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class AppointmentController: ControllerBase
    {
        private readonly ITelemedRepository _repo; 

        private readonly IMapper _mapper;

        public AppointmentController(ITelemedRepository repo, IMapper mapper) {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetAppointment")]
        public async Task<IActionResult> GetAppointment(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var appointmentFromRepo = await _repo.GetAppointment(id);
            if (appointmentFromRepo == null)
                return NotFound();
            
            return Ok(appointmentFromRepo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(int userId, AppointmentForCreationDto apptForCreation) {
            var apptCreator = await _repo.getUser(userId);

            if (apptCreator.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var apptRecipient;
            if (apptCreator.UserRole.RoleId.Equals(1)) {
                apptForCreation.PatientId = userId;

                apptRecipient = await _repo.getUser(apptForCreation.DoctorId);
                if (apptRecipient == null) {
                    return BadRequest("Could not find Doctor");
                }
            }
            else if (apptCreator.UserRole.RoleId.Equals(3)) {
                apptForCreation.DoctorId = userId;

                apptRecipient = await _repo.getUser(apptForCreation.PatientId);
                if (apptRecipient == null) {
                    return BadRequest("Could not find Patient");
                }
            }
            //messageForCreationDto.SenderId = userId;
            // var apptRecipient = await _repo.getUser(apptForCreation.RecipientId);
            // if (recipient == null) {
            //     return BadRequest("Could not find user");
            // }

            var appointment = _mapper.Map<Appointment>(apptForCreation);
            _repo.Add(appointment);
            
            if (await _repo.SaveAll()) {
                //var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                return CreatedAtRoute("GetAppointment", new {id = appointment.Id}, appointment); //, messageToReturn);
            }
            throw new Exception("Creating the appointment failed on save");
        }
        
    }
}