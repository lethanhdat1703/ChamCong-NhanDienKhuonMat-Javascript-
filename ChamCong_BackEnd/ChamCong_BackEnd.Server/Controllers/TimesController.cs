using ChamCong_BackEnd.Server.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChamCong_BackEnd.Server.Controllers
{
    [Route("api/[times]")]
    [ApiController]
    public class TimesController<T,TDto> : ControllerBase where T : class where TDto: class
    {
        private readonly IGenericRepository<T,TDto> _repository;

        public TimesController(IGenericRepository<T, TDto> repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTime()
        {
            try
            {
                var time = _repository.GetAll();
                return Ok(time);
            }catch  {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddTime(TDto timeDTO)
        {
            try
            {
                var addTimeDTO = _repository.Add(timeDTO);
                return Ok(addTimeDTO);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimeById(int id)
        {
            try
            {
                var time = _repository.GetById(id);
                if(time != null)
                {
                    return Ok(time);
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
    }
}
