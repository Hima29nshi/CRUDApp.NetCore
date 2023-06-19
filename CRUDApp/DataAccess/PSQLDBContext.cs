using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;


//DB not created using Code First Approach
namespace CRUDApp.DataAccess
{
    //accessing the data through Entity Framework
    public class PSQLDBContext: DbContext
    {
        // IConfiguration interface is used to read Settings and Connection Strings from AppSettings
        protected readonly IConfiguration _configuration;
        public PSQLDBContext(IConfiguration configuration, DbContextOptions<PSQLDBContext> options):base(options) { 
            _configuration = configuration;
        }

        //providing the table name
        public DbSet<EmployeeModel> employee { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("EmployeeDBConn"));
        }

        // defines database model and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
