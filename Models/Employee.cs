using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCDemo.Models
{
    public class Employee
    {
        [DisplayName("ID number")]
        public Guid ID { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First name required")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        [DisplayName("Gender")]
        [Required(ErrorMessage = "Gender required")]
        public string Gender { get; set; }

        [DisplayName("Salary")]
        [Required(ErrorMessage = "Salary required")]
        public int Salary { get; set; }
    }
}