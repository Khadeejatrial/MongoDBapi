using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MongoDBapi.Domain;
public class Project
{
    public int ProjectDetailID { get; set; }
    public int EmployeeDetailID { get; set; }
    public string ProjectName { get; set; }
}
