using Microsoft.AspNetCore.Mvc;
using Telemedicine.API.Data;
using Telemedicine.API.Models;
using System.Threading.Tasks;
using Telemedicine.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper; 

namespace Telemedicine.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        private readonly IConfiguration _config;

        private readonly IMapper _mapper; 
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper )
        {
            _mapper = mapper; 
            _repo = repo;
            _config = config;
        } 

       [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("Username already exists");
        
            // Maybe add to a separate doctorRegister method? Or add back for doctor registering
             if (userForRegisterDto.DeaId == null)
                userForRegisterDto.DeaId = "0"; // If there is no DEA Id, assign default 0
            else
                if (await _repo.DoctorExists(userForRegisterDto.DeaId))
                    return BadRequest("The specific DEA ID has already been registered"); 
            // For now we'll assume its a patient with no DEA Id and assign it to 0
            userForRegisterDto.DeaId = "0";
            
            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password, userForRegisterDto.DeaId);

            return StatusCode(201);
        }
 
         [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            //throw new Exception("Computer says no!");

            // Check that user has username and password in database
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            // Check if a user has been found
            if (userFromRepo == null)
                // we don't want to let them know the username or password is correct for security
                return Unauthorized(); 
            
            // claims are user's id and user's username
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            // Key to sign our token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            // Singing credentials with encoded key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // May need to change this to our needs
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            // Contains JWT token to return to client
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //114
            var user = _mapper.Map<UserForListDto>(userFromRepo); 

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        } 
    }
}