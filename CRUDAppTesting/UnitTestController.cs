using CRUDApp.Controllers;
using CRUDApp.DataAccess;
using CRUDApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CRUDAppTesting
{
    public class UnitTestController
    {
        static Mock<IDataAccessProvider> mock = new Mock<IDataAccessProvider>();
        EmployeesController employeesController = new EmployeesController(mock.Object);
        
        [Fact]
        public async void GetByIdExistingReturnsCorrectResult()
        {
            var e = new EmployeeModel
            {
                e_id = 35,
                first_name = "Himanshi",
                last_name = "Brain",
                country = "Philippines",
                gender = "Bigender",
                salary = 705696
            };
            //setup expectations for mocked object methods or properties
            mock.Setup(p => p.GetEmployeeRecordByIdAsync(35)).ReturnsAsync(e);
            var empOkObject = await employeesController.GetEmployeeRecordById(35) as OkObjectResult;

            var res = empOkObject?.Value;
            Assert.Equal(e, res);
        }

        [Fact]
        public async void GetByIdNonExistingReturnsNotFoundResult()
        {
            var empNotFoundObject = await employeesController.GetEmployeeRecordById(656);
            Assert.IsType<NotFoundObjectResult>(empNotFoundObject);
        }

        [Fact]
        public async void GetReturnsAllCorrectResult()
        {
            var e = Data.employees;
            mock.Setup(p => p.GetAllEmployeeRecordAsync()).ReturnsAsync(e);
            var allEmps =  await employeesController.GetAllEmployees() as OkObjectResult;

            var result = allEmps?.Value as List<EmployeeModel>;
            Assert.Equal(e.Count,result?.Count);
        }

        [Fact]  
        public async void AddReturnOkResult()
        {
            var e = new EmployeeModel
            {
                first_name = "Nilesh",
                last_name = "Jain",
                country = "Philippines",
                gender = "Bigender",
                salary = 705696
            };
            mock.Setup(p => p.AddEmployeeRecordAsync(e)).ReturnsAsync(1);
            var addEmp = await employeesController.AddEmployeeRecord(e);

            Assert.IsType<OkObjectResult>(addEmp);
        }

        [Fact]
        public async void AddExistingReturnBadRequestResult()
        {
            var e = new EmployeeModel
            {
                e_id = 35,
                first_name = "Nilesh",
                last_name = "Jain",
                country = "Philippines",
                gender = "Bigender",
                salary = 705696
            };
            mock.Setup(p => p.AddEmployeeRecordAsync(e)).ReturnsAsync(0);
            var addEmp = await employeesController.AddEmployeeRecord(e);

            Assert.IsType<BadRequestObjectResult>(addEmp);
        }

        [Fact]
        public async void UpdateExistingReceivesOkResult()
        {
            var e = new EmployeeModel
            {
                e_id = 35,
                first_name = "Nilesh",
                last_name = "Jain",
                country = "Philippines",
                gender = "Bigender",
                salary = 705696
            };
            mock.Setup(p => p.UpdateEmployeeRecordAsync(e,e.e_id)).ReturnsAsync(1);
            var updateEmp = await employeesController.UpdateEmployeeRecord(e.e_id, e);

            Assert.IsType<OkObjectResult>(updateEmp);
        }

        [Fact]
        public async void UpdateNonExistingReceivesOkResult()
        {
            var e = new EmployeeModel
            {
                e_id = 656,
                first_name = "Nilesh",
                last_name = "Jain",
                country = "Philippines",
                gender = "Bigender",
                salary = 705696
            };
            mock.Setup(p => p.UpdateEmployeeRecordAsync(e, e.e_id)).ReturnsAsync(0);
            var updateEmp = await employeesController.UpdateEmployeeRecord(e.e_id, e);

            Assert.IsType<NotFoundObjectResult>(updateEmp);
        }

        [Fact]
        public async void DeleteNonExistingReceivesNotFoundResult()
        {
            mock.Setup(p => p.DeleteEmployeeRecordAsync(565)).ReturnsAsync(0);
            var deleteEmp = await employeesController.DeleteEmployeeRecordById(555);

            Assert.IsType<NotFoundObjectResult>(deleteEmp);
        }

        [Fact]
        public async void DeleteExistingReceivesNotFoundResult()
        {
            mock.Setup(p => p.DeleteEmployeeRecordAsync(35)).ReturnsAsync(1);
            var deleteEmp = await employeesController.DeleteEmployeeRecordById(35);

            Assert.IsType<OkObjectResult>(deleteEmp);
        }
    }
}