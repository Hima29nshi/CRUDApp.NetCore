using CRUDApp.Models;

namespace CRUDApp.DataAccess
{
    public class Data
    {
        public static List<EmployeeModel> employees = new List<EmployeeModel>()
        {
            new EmployeeModel
            {
                e_id = 4,
                first_name = "Steve",
                last_name = "Kerton",
                country = "United States",
                gender = "Male",
                salary = 359679
            },
            new EmployeeModel
            {
                e_id = 7,
                first_name = "Lance",
                last_name = "Zanetti",
                country = "Poland",
                gender = "Male",
                salary = 566830
            },
            new EmployeeModel
            {
                e_id = 8,
                first_name = "Tilda",
                last_name = "Mc Gaughey",
                country = "China",
                gender = "Female",
                salary = 844103
            },
            new EmployeeModel
            {
                e_id = 9,
                first_name = "Allyce",
                last_name = "Raulin",
                country = "Palestinian Territory",
                gender = "Polygender",
                salary = 229915
            },
        };
    }
        
}
