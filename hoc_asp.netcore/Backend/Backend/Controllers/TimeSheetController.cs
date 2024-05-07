using Backend.DTO;
using Backend.Models;
using Backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeSheetController : ControllerBase
    {
        private readonly ITimeSheetRepository _timeRepo;

        public TimeSheetController(ITimeSheetRepository repo) {
            _timeRepo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTimeSheet() {
            try
            {
                return Ok(await _timeRepo.getAllTimeSheetsAsync());
            } catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimeSheetById(int id)
        {
            var time = await _timeRepo.getTimeSheetsAsync(id);
            return time == null ? NotFound() : Ok(time);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewTimeSheet(TimeSheetDTO timeSheetDTO)
        {
            try
            {
                var addnew = await _timeRepo.AddTiemSheetAsync(timeSheetDTO);
                var add = await _timeRepo.getTimeSheetsAsync(addnew);
                return add == null ? NotFound() : Ok(add);

            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimeSheet( TimeSheetDTO timeSheetDTO, int id)
        {
            await _timeRepo.UpdateTimeSheetAsync( timeSheetDTO, id);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeSheetAsync(int id)
        {
            await _timeRepo.DeleteTimeSheetAsync( id);
            return Ok();
        }
    }
}
