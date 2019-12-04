using System;
using System.Collections.Generic;
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

            var apptRecipient = (User)null;
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
            appointment.Patient = await _repo.getUser(appointment.PatientId);
            appointment.Doctor = await _repo.getUser(appointment.DoctorId);


            // CHECK IF APPOINTMENT ALREADY EXISTS ON THIS DATE
            UserParams userParams = new UserParams();
            userParams.UserId = appointment.PatientId;
            var patientApptsGet = await _repo.GetAppointmentsForUser(userParams);
            for (int i = 0; i < patientApptsGet.Count; i++) {
                var appt = _mapper.Map<AppointmentToReturnDto>(appointment);
                if (patientApptsGet[i].StartDate.CompareTo(appt.StartDate) == 0) {
                    return BadRequest("This date is unavailable.");
                }
            }

            userParams.UserId = appointment.DoctorId;
            var doctorApptsGet = await _repo.GetAppointmentsForUser(userParams);
            for (int i = 0; i < doctorApptsGet.Count; i++) {
                var appt = _mapper.Map<AppointmentToReturnDto>(appointment);
                if (doctorApptsGet[i].StartDate.CompareTo(appt.StartDate) == 0) {
                    return BadRequest("This date is unavailable.");
                }
            }




            _repo.Add(appointment);
            
            if (await _repo.SaveAll()) {
                var apptToReturn = _mapper.Map<AppointmentToReturnDto>(appointment);
                return CreatedAtRoute("GetAppointment", new {id = appointment.Id}, apptToReturn); //, messageToReturn);
            }
            throw new Exception("Creating the appointment failed on save");
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentsForUser(int userId, [FromQuery]UserParams userParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            userParams.UserId = userId;

            var apptsFromRepo = await _repo.GetAppointmentsForUser(userParams);

            var appts = _mapper.Map<IEnumerable<AppointmentToReturnDto>>(apptsFromRepo);

            Response.AddPagination(apptsFromRepo.CurrentPage, apptsFromRepo.PageSize, 
            apptsFromRepo.TotalCount, apptsFromRepo.TotalPages);

            return Ok(appts);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id, int userId)
        {
            // if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            // {
            //     return Unauthorized();
            // }

            var appointmentFromRepo = await _repo.GetAppointment(id);

            if (!userId.Equals(appointmentFromRepo.PatientId) && !userId.Equals(appointmentFromRepo.DoctorId)) {
                return Unauthorized();
            }

            // if (messageFromRepo.SenderId == userId)
            //     messageFromRepo.SenderDeleted = true;

            // if (messageFromRepo.RecipientId == userId)
            //     messageFromRepo.RecipientDeleted = true;
            
            // Only actually delete message if both sides of the conversation delete it
            // if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
            _repo.Delete(appointmentFromRepo);

            if (await _repo.SaveAll()) 
                return NoContent();
            
            throw new Exception("Error deleting the appointment");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, int userId, AppointmentForUpdateDto apptForUpdateDto)
        {
            var appointmentFromRepo = await _repo.GetAppointment(id);

            if (!userId.Equals(appointmentFromRepo.PatientId) && !userId.Equals(appointmentFromRepo.DoctorId)) {
                return Unauthorized();
            }
            // if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //     return Unauthorized();

            // var userFromRepo = await _repo.getUser(id);

            _mapper.Map(apptForUpdateDto, appointmentFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Update appointment {id} failed on save");       
        }
    }
}