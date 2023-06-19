using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.DataAccess
{
    public interface IDataAccessProvider
    {
        Task<int> AddEmployeeRecordAsync(EmployeeModel employee);
        Task<int> UpdateEmployeeRecordAsync(EmployeeModel employee);
        Task<int> DeleteEmployeeRecordAsync(int id);
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
            var emp = await _dbContext.employee.FirstOrDefaultAsync(t => t.e_id == employee.e_id);
            if (emp == null)
            {
                _dbContext.employee.Add(employee);
                await _dbContext.SaveChangesAsync();
                return 1;
            }
            return 0;
        }
        
        public async Task<int> UpdateEmployeeRecordAsync(EmployeeModel employee) {
            if(employee != null) 
            { 
                _dbContext.Update(employee);
                await _dbContext.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        public async Task<int> DeleteEmployeeRecordAsync(int id) { 
            var emp = _dbContext.employee.FirstOrDefault(t=>t.e_id == id);
            if (emp != null)
            {
                _dbContext.employee.Remove(emp);
                await _dbContext.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        public async Task<EmployeeModel> GetEmployeeRecordByIdAsync(int id) {
            var result = await _dbContext.employee.FirstOrDefaultAsync(t => t.e_id == id);
#pragma warning disable CS8603 // Possible null reference return.
            return result;
        }

        public async Task<List<EmployeeModel>> GetAllEmployeeRecordAsync() {
            return await _dbContext.employee.ToListAsync();
        }

    }
}
