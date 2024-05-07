using ChamCong_BackEnd.Server.DTO;
using ChamCong_BackEnd.Server.Models;
using ChamCong_BackEnd.Server.Repository;
using ChamCong_BackEnd.Server.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
namespace ChamCong_BackEnd.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        private readonly ITimeRepository _timeRepository;
        private readonly IMapper _mapper;

        public TimeController(ITimeRepository timeRepository, IMapper mapper)
        {
            _timeRepository = timeRepository;
            _mapper = mapper; 
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTime()
        {
            try
            {
                return Ok(_timeRepository.GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTime(TimeDTO timeDTO)
        {
            try
            {
                var addedTimeDTO = _timeRepository.Add(timeDTO); 
                return Ok(addedTimeDTO);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}")]
        [Authorize] 
        public async Task<IActionResult> GetTimeById(int id)
        {
            try
            {
                var time = _timeRepository.GetByID(id);
                if (time != null)
                {
                    return Ok(_timeRepository.GetByID(id));
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var time = _timeRepository.GetByID(id);
                if (time != null)
                {
                    _timeRepository.Delete(id);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Authorize]  
        public async Task<IActionResult> Update(int id, TimeDTO timeDTO)
        {
            if (id != timeDTO.Id)
            {
                return BadRequest();
            }
            try
            {
                _timeRepository.Update(timeDTO);
                return Content("cap nhat thanh cong " + timeDTO.LunchBreak + " " + timeDTO.StartTime + " " + timeDTO.EndTime);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}

