using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MongoDBapi.Domain;
public class MongoEmployee
{
    [BsonId]
    public int EmployeeID { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Salary { get; set; }
    public DateTime JoiningDate { get; set; }
    public string Department { get; set; }
    public string Gender { get; set; }

    public List<Project> Projects { get; set; }
    public bool IsActive { get; set; } = true;

}
