using AutoMapper;
using Backend.DTO;
using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetsController : ControllerBase
    {
        private readonly IGenericRepository<TimeSheet> _timeRepo;
        private readonly IMapper _mapper;

        public TimeSheetsController(IGenericRepository<TimeSheet> repo, IMapper mapper)
        {
            _timeRepo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTimeSheet()
        {
            try
            {
                var timeSheets = await _timeRepo.GetAllAsync();
                var timeSheetDTOs = _mapper.Map<List<TimeSheetDTO>>(timeSheets);
                return Ok(timeSheetDTOs);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimeSheetById(int id)
        {
            var timeSheet = await _timeRepo.GetByIdAsync(id);
            var timeSheetDTO = _mapper.Map<TimeSheetDTO>(timeSheet);
            return timeSheetDTO == null ? NotFound() : Ok(timeSheetDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTimeSheet(TimeSheetDTO timeSheetDTO)
        {
            try
            {
                var addTimeSheet = _mapper.Map<TimeSheet>(timeSheetDTO);
                var newId = await _timeRepo.AddAsync(addTimeSheet);
                var addedTimeSheet = await _timeRepo.GetByIdAsync(newId);
                var addedTimeSheetDTO = _mapper.Map<TimeSheetDTO>(addedTimeSheet);
                return addedTimeSheetDTO == null ? NotFound() : Ok(addedTimeSheetDTO);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimeSheet(int id, TimeSheetDTO timeSheetDTO)
        {
            try
            {
                var updateTimeSheet = _mapper.Map<TimeSheet>(timeSheetDTO);
                await _timeRepo.UpdateAsync(id, updateTimeSheet);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeSheetAsync(int id)
        {
            try
            {
                await _timeRepo.DeleteAsync(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
