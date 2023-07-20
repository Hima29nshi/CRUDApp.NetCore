using System.ComponentModel.DataAnnotations;

namespace CRUDApp.Models
{
    public class EmployeeModel
    {
        [Required]
        [Key]
        public int e_id { get; set; }
       
        [Required]
        [StringLength(50)]
        public string first_name { get; set; }
       
        [Required]
        [StringLength(50)]
        public string last_name { get; set;}

        [Required]
        [StringLength(50)]
        public string country { get; set; }

        [Required]
        [StringLength(50)]
        public string gender { get; set; }

        [Required]
        public double salary { get; set; }
    }
}
