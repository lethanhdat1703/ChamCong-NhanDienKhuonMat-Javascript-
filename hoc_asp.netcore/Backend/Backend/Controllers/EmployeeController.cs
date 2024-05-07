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
    public class EmployeeController : ControllerBase
    {
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IGenericRepository<Employee> employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var employeeDTOs = _mapper.Map<List<EmployeeDTO>>(employees);
            foreach (var employeeDTO in employeeDTOs)
            {
                var employee = await _employeeRepository.GetByIdAsync(employeeDTO.Id);
                employeeDTO.FaceModels = _mapper.Map<List<FaceModelsDTO>>(employee.FaceModels);
                employeeDTO.TimeKeepings = _mapper.Map<List<TimeKeepingDTO>>(employee.TimeKeepings);
            }
            return Ok(employeeDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
            return Ok(employeeDTO);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployee(EmployeeDTO employeeDTO)
        {
            var employee = _mapper.Map<Employee>(employeeDTO);
            try
            {
                var newId = await _employeeRepository.AddAsync(employee);
                var addedEmployee = await _employeeRepository.GetByIdAsync(newId);
                var addedEmployeeDTO = _mapper.Map<EmployeeDTO>(addedEmployee);
                return CreatedAtAction(nameof(GetEmployee), new { id = addedEmployeeDTO.Id }, addedEmployeeDTO);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDTO employeeDTO)
        {
            if (id != employeeDTO.Id)
            {
                return BadRequest();
            }

            var employee = _mapper.Map<Employee>(employeeDTO);

            try
            {
                await _employeeRepository.UpdateAsync(id, employee);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
