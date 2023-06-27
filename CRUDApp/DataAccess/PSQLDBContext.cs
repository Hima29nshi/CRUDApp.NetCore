using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;


//DB created using DB First Approach
namespace CRUDApp.DataAccess
{
    //accessing the data through Entity Framework
    public class PSQLDBContext: DbContext
    {
        public PSQLDBContext(DbContextOptions<PSQLDBContext> options):base(options) { 
           
        }

        //providing the table name
        public DbSet<EmployeeModel> employee { get; set; }
    }
}
