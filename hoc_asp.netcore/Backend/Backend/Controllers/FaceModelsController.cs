using AutoMapper;
using Backend.Models;
using Backend.Repository;
using Backend.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceModelsController : ControllerBase
    {
        private readonly IFaceModelsRepository _faceModelsRepository;
        private readonly IMapper _mapper;

        public FaceModelsController(IFaceModelsRepository faceModelsRepository, IMapper mapper) // Sửa đổi đây
        {
            _faceModelsRepository = faceModelsRepository;
            _mapper = mapper;
        }
        [HttpPost("save-images")]
        public async Task<IActionResult> SaveImages(List<string> imagesUrl, int employeeId) 
        {
            try
            {
                var result = await _faceModelsRepository.SaveImageUrlsAsync(imagesUrl, employeeId);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
