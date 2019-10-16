using AutoMapper; 
using Telemedicine.API.Data; 
using Telemedicine.API.Helpers; 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Options;  
using Telemedicine.API.Dtos; 
using System.Threading.Tasks;  
using CloudinaryDotNet; 
using CloudinaryDotNet.Actions;
using System.Security.Claims; 
using Telemedicine.API.Models; 
using System.Linq; 
using System; 
using System.Collections.Generic;

//using Microsoft.AspNetCore.ApiControler; 
//using Microsoft.AspNetCore.Mvc.IActionResult; 


namespace Telemedicine.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/documents")]
    [ApiController]
    public class DocumentController: ControllerBase 
    {
        private readonly ITelemedRepository _repo; 

        private readonly IMapper _mapper; 

        private readonly IOptions<CloudinarySettings> _cloudinaryConfig; 

        private Cloudinary _cloudinary;

        public DocumentController(ITelemedRepository repo, IMapper mapper,
         IOptions<CloudinarySettings> cloudinaryConfig )
        {
            _cloudinaryConfig = cloudinaryConfig; 
            _mapper = mapper; 
            _repo = repo; 

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret 

            ); 

            _cloudinary = new Cloudinary(acc); 

        }

        [HttpGet("{id}", Name = "GetDocument")]

        public async Task<IActionResult> GetDocument(int id)
        {
            var documentFromRepo = await _repo.GetDocument(id);

            var document = _mapper.Map<DocumentForReturnDto>(documentFromRepo);

            return Ok(document); 
        }
        private readonly DocumentForCreationDto DocumentForCreationDto;

        [HttpPost]
        public async Task<IActionResult> AddDocumentForUser(int userId,
            [FromForm] DocumentForCreationDto docForCreationDto)
        {
           // [FromForm]PhotoForCreationDto = PhotoForCreationDto)

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.getUser(userId);

            var file = docForCreationDto.File; 

            var uploadResult = new ImageUploadResult(); 

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                          .Width(500).Height(500).Crop("fill").Gravity("face")
                    }; 

                    uploadResult = _cloudinary.Upload(uploadParams); 
                }
            }

            docForCreationDto.Url = uploadResult.Uri.ToString(); 
            docForCreationDto.PublicId = uploadResult.PublicId; 

            var document = _mapper.Map<Document>(docForCreationDto); 

            // if (!userFromRepo.Documents.Any(u => u.IsMain))
            //     photo.IsMain = true; 
        
            userFromRepo.Documents.Add(document); 

            //var photoToReturn = _mapper.Map<PhotosForReturnDto>(photo);

            if (await _repo.SaveAll())
            {
                var docToReturn = _mapper.Map<DocumentForReturnDto>(document);
                return CreatedAtRoute("GetDocument", new {id = document.id}, docToReturn ); 
            }

            return BadRequest("Could not add the photo");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized(); 
            
            var user = await _repo.getUser(userId); 

            if (!user.Documents.Any(p => p.id == id))
                return Unauthorized();

            var docFromRepo = await _repo.GetDocument(id);

            if(docFromRepo.PublicId != null) {
                var deleteParams = new DeletionParams(docFromRepo.PublicId);

                // checks that response and result comes back ok
                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok") {
                    _repo.Delete(docFromRepo);
                }
            } 

            if (docFromRepo.PublicId == null) {
                _repo.Delete(docFromRepo);
            }

            if (await _repo.SaveAll()) {
                return Ok();
            }

            return BadRequest("Failed to delete the photo");
        }

        // video 111 - not using main photos
        // [HttpPost("{id}/setMain")]
        // public async Task<IActionResult> SetMainPhoto(int userId, int id){

        //      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //         return Unauthorized(); 
            
        //     var user = await _repo.getUser(userId); 

        //     if (!user.Documents.Any(p => p.id == id))
        //         return Unauthorized();

        //     var photoFromRepo = await _repo.GetPhoto(id); 

        //     if (photoFromRepo.IsMain)
        //         return BadRequest("This is already the main photo"); 
            
        //     var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
        //     currentMainPhoto.IsMain = false; 

        //     photoFromRepo.IsMain = true; 

        //     if(await _repo.SaveAll())
        //         return NoContent(); 

        //     return BadRequest("Could not set photo to main");
        // }
    }
}