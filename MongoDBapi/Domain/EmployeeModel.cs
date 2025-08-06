using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBapi.Domain
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Project> Projects { get; set; }

    }
}
