using CRUDApp.DataAccess;
using CRUDApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CRUDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IDataAccessProvider _dataAccessProvider;
        private ILogger _logger;

        public EmployeesController(IDataAccessProvider dataAccessProvider, ILoggerFactory logger)
        {
            _dataAccessProvider = dataAccessProvider;
            _logger = logger.CreateLogger("EmployeeDBCRUD");
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeRecord(EmployeeModel employee)
        {
            _logger.LogInformation($"Initiating addition of an employee named {employee.first_name}");
            int empId = await _dataAccessProvider.AddEmployeeRecordAsync(employee);
            if (empId != 0)
            {
                _logger.LogInformation($"{employee.first_name} added successfully at ID{empId}");
                return Ok($"Successfully inserted the data of {employee.first_name}");
            }
            else
            {
                _logger.LogError($"Error while inserting data/Employee already exists");
                return BadRequest($"Error while inserting data/Employee already exists");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployeeRecord(EmployeeModel employee)
        {
            _logger.LogInformation($"Initiating updation of an employee named {employee.first_name}");
            ///summary
            ///ModelState.IsValid indicates if it was possible to bind the incoming values 
            ///from the request to the model correctly and whether any explicitly specified 
            ///validation rules were broken during the model binding process.
            ///summary
            ///
            if(ModelState.IsValid)
            {
                try
                {
                    await _dataAccessProvider.UpdateEmployeeRecordAsync(employee);
                    _logger.LogInformation($"Data of {employee.first_name} updated successfully");
                    return Ok($"Successfully updated the data of {employee.first_name}");
                }
                catch (Exception)
                {
                    _logger.LogError("Employee not found");
                    return NotFound("Employee not found");
                }
            }
            return BadRequest("Error while updating the data");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            _logger.LogInformation("Retrieving all employees data");
            List<EmployeeModel> employees = await _dataAccessProvider.GetAllEmployeeRecordAsync();
            if (employees != null)
            {
                _logger.LogInformation("Data retrieved successfully");
                return Ok(employees);
            }
            else
            {
                _logger.LogError($"Error retrieving the data");
                return BadRequest($"Error while retrieving the data");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeRecordById(int id)
        {
            _logger.LogInformation($"Retrieving data of an employee ID:{id}");
            EmployeeModel emp = await _dataAccessProvider.GetEmployeeRecordByIdAsync(id);
            if (emp != null)
            {
                _logger.LogInformation($"Employee data retrieved");
                return Ok(emp);
            }
            else
            {
                _logger.LogError("Employee not found");
                return BadRequest($"Error while retrieving the data");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeeRecordById(int id)
        {
            _logger.LogInformation($"Initiating deletion of an employee ID:{id}");
            int res = await _dataAccessProvider.DeleteEmployeeRecordAsync(id);
            if ( res == 1)
            {
                _logger.LogInformation("Data deleted successfully");
                return Ok("Data deleted successfully");
            }
            else
            {
                _logger.LogError("Employee not found");
                return NotFound();
            }
        }
    }
}
