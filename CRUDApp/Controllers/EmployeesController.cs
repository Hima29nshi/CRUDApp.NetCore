using CRUDApp.DataAccess;
using CRUDApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private IDataAccessProvider _dataAccessProvider;

        public EmployeesController(IDataAccessProvider dataAccessProvider)
        {
            _dataAccessProvider = dataAccessProvider;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeRecord(EmployeeModel employee)
        {
            int empId = await _dataAccessProvider.AddEmployeeRecordAsync(employee);
            if (empId != 0) { 
                return Ok($"Successfully inserted the data of {employee.first_name}");
            }
            else
            {
                return BadRequest($"Error while inserting the data");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployeeRecord(EmployeeModel employee)
        {
            ///summary
            ///ModelState.IsValid indicates if it was possible to bind the incoming values 
            ///from the request to the model correctly and whether any explicitly specified 
            ///validation rules were broken during the model binding process.
            ///summary
            if (ModelState.IsValid)
            {
                await _dataAccessProvider.UpdateEmployeeRecordAsync(employee);
                return Ok($"Successfully updated the data of {employee.first_name}");
            }
            else
            {
                return BadRequest($"Error while updating the data");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            List<EmployeeModel> employees = await _dataAccessProvider.GetAllEmployeeRecordAsync(); 
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeRecordById(int id)
        {
            EmployeeModel emp = await _dataAccessProvider.GetEmployeeRecordByIdAsync(id);
            return Ok(emp);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployeeRecordById(int id)
        {
            await _dataAccessProvider.DeleteEmployeeRecordAsync(id);
            return Ok("Data deleted successfully");
        }
    }
}
