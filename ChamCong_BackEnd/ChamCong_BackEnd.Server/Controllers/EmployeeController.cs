using ChamCong_BackEnd.Server.Data;
using ChamCong_BackEnd.Server.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ChamCong_BackEnd.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly NhanDienDbContext _context;

        public EmployeeController(NhanDienDbContext context) {
            _context=context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees()
        {
            var employeesWithFaceData = await _context.Employees
                .Include(e => e.FaceModels)
                .Include(e => e.Timekeepings)
                .ToListAsync();
            var employeeDtos = employeesWithFaceData.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                Name = e.Name,
                Gender = (bool)e.Gender,
                Email = e.Email,
                Department = e.Department,
                Position = e.Position,
                Phone = e.Phone,
                Base_Img = e.Base_Img,

                Faces = e.FaceModels.Select(fd => new FaceModelDTO
                {
                    Id = fd.Id,
                    Img = fd.Img,
                    EmployeeId = fd.Id,
                }).ToList(),

                TimeKeeping = e.Timekeepings.Select(t => new TimeKeepingDTO
                {
                    Id = t.Id,
                    CheckIin = t.CheckIn,
                    CheckOut = t.CheckOut,
                    Status = t.Status,
                    EmployeeId = t.Id,
                }).ToList(),

            });
            return Ok(employeeDtos);
        }
    }

}



