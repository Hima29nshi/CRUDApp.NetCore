using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.DataAccess
{
    public interface IDataAccessProvider
    {
        Task<int> AddEmployeeRecordAsync(EmployeeModel employee);
        Task UpdateEmployeeRecordAsync(EmployeeModel employee);
        Task DeleteEmployeeRecordAsync(int id);
        Task<EmployeeModel> GetEmployeeRecordByIdAsync(int id);
        Task<List<EmployeeModel>> GetAllEmployeeRecordAsync();
    }
    public class DataAccessProvider:IDataAccessProvider
    {
        private readonly PSQLDBContext _dbContext;

        public DataAccessProvider(PSQLDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddEmployeeRecordAsync(EmployeeModel employee) {
            _dbContext.employee.Add(employee);
            await _dbContext.SaveChangesAsync();
            if (employee != null)
            {
                return employee.e_id;
            }
            return 0;
        }
        
        public async Task UpdateEmployeeRecordAsync(EmployeeModel employee) {
            _dbContext.employee.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeRecordAsync(int id) { 
            var emp = _dbContext.employee.FirstOrDefault(t=>t.e_id == id);
            if (emp != null)
            {
                _dbContext.employee.Remove(emp);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Employee Not Found!");
            }
        }

        public async Task<EmployeeModel> GetEmployeeRecordByIdAsync(int id) {
            var result = await _dbContext.employee.FirstOrDefaultAsync(t => t.e_id == id);
            return result;
        }

        public async Task<List<EmployeeModel>> GetAllEmployeeRecordAsync() {
            return await _dbContext.employee.ToListAsync();
        }

    }
}
