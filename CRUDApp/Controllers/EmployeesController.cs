using CRUDApp.DataAccess;
using CRUDApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IDataAccessProvider _dataAccessProvider;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IDataAccessProvider dataAccessProvider,ILogger<EmployeesController> logger)
        {
            _dataAccessProvider = dataAccessProvider;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeRecord(EmployeeModel employee)
        {
            _logger.LogInformation($"{DateTime.Now}: Initiating addition of an employee named {employee.first_name}");

            if (ModelState.IsValid)
            {
                int empAddedSuccess = await _dataAccessProvider.AddEmployeeRecordAsync(employee);
                if (empAddedSuccess != 0)
                {
                    _logger.LogInformation($"{DateTime.Now}: {employee.first_name} added successfully");
                    return new OkObjectResult(employee) { StatusCode = StatusCodes.Status201Created };
                }
                else
                {
                    _logger.LogError($"{DateTime.Now}: Error while inserting data");
                    return BadRequest($"Error while inserting data {StatusCodes.Status500InternalServerError}");
                }
            }
            else
            {
                _logger.LogError($"{DateTime.Now}: Invalid POST request");
                return BadRequest($"Error while inserting data {StatusCodes.Status500InternalServerError}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeRecord(int id, EmployeeModel employee)
        {
            _logger.LogInformation($"{DateTime.Now}: Initiating updation of an employee named {employee.first_name}");
            ///summary
            ///ModelState.IsValid indicates if it was possible to bind the incoming values 
            ///from the request to the model correctly and whether any explicitly specified 
            ///validation rules were broken during the model binding process.
            ///summary
            /// 
            int res = await _dataAccessProvider.UpdateEmployeeRecordAsync(employee, id);
            if (res != 0)
            {
                _logger.LogInformation($"{DateTime.Now}: Data of {employee.first_name} updated successfully");
                return Ok($"Data of {employee.first_name} updated successfully");
            }
            else
            {
                _logger.LogError($"{DateTime.Now}: Employee with {id} not found");
                return NotFound($"Employee with {id} not found");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            _logger.LogInformation($"{DateTime.Now}: Retrieving all employees data");

            //Example of error handling
            //throw new Exception();

            List<EmployeeModel> employees = await _dataAccessProvider.GetAllEmployeeRecordAsync();
            if (employees != null)
            {
                _logger.LogInformation($"{DateTime.Now}: Data retrieved successfully");
                return Ok(employees);
            }
            else
            {
                _logger.LogError($"{DateTime.Now}: Error retrieving the data");
                return BadRequest($"Error while retrieving the data");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeRecordById(int id)
        {
            _logger.LogInformation($"{DateTime.Now}: Retrieving data of an employee ID:{id}");

            EmployeeModel emp = await _dataAccessProvider.GetEmployeeRecordByIdAsync(id);
            if (emp != null)
            {
                _logger.LogInformation($"{DateTime.Now}: Employee data retrieved");
                return Ok(emp);
            }
            else
            {
                _logger.LogError($"{DateTime.Now}: Employee not found");
                return NotFound($"Employee not found");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeRecordById(int id)
        {
            _logger.LogInformation($"{DateTime.Now}: Initiating deletion of an employee ID:{id}");
            
            int res = await _dataAccessProvider.DeleteEmployeeRecordAsync(id);
            if (res == 1)
            {
                _logger.LogInformation($"{DateTime.Now}: Data deleted successfully");
                return Ok("Data deleted successfully");
            }
            else
            {
                _logger.LogError($"{DateTime.Now}: Employee not found");
                return NotFound("Employee not found");
            }
        }
    }
}
