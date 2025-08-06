using Microsoft.AspNetCore.Mvc;
using MongoDBapi.Domain;
using MongoDBapi.Services;
using MongoDB.Services;

namespace MongoDBapi.Controllers
{
    [ApiController]
    [Route("employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly SQLService _sqlService;
        private readonly MongoService _mongoService;

        public EmployeeController(SQLService sqlService, MongoService mongoService)
        {
            _sqlService = sqlService;
            _mongoService = mongoService;
        }

        [HttpPost("migrate")]
        public IActionResult MigrateDataToMongo()
        {
            var sqlEmployees = _sqlService.GetAllEmployees();

            var mongoEmployees = sqlEmployees.Select(emp => new MongoEmployee
            {
                EmployeeID = emp.EmployeeID,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Salary = emp.Salary,
                JoiningDate = emp.JoiningDate,
                Department = emp.Department,
                Gender = emp.Gender,
                IsActive = emp.IsActive,
                Projects = _sqlService.GetProjectsByEmployeeId(emp.EmployeeID).ToList(),
            }).ToList();

            _mongoService.ReplaceAllEmployees(mongoEmployees);

            return Ok("Data migrated to MongoDB successfully.");
        }

        [HttpGet("mongo")]
        public IActionResult GetMongoEmployees()
        {
            return Ok(_mongoService.GetAllEmployees());
        }

        [HttpGet("sql/{id}")]
        public IActionResult GetSqlEmployeeById(int id)
        {
            var employee = _sqlService.GetEmployeeById(id);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        [HttpGet("mongo/{id}")]
        public IActionResult GetMongoEmployeeById(int id)
        {
            var employee = _mongoService.GetEmployeeById(id);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

        [HttpPost("sql")]
        public IActionResult InsertSqlEmployee([FromBody] Employee employee)
        {
            if (_sqlService.InsertEmployee(employee))
                return Ok("Inserted into SQL");
            return BadRequest("Insert failed");
        }

        [HttpPost("mongo")]
        public IActionResult InsertMongoEmployee([FromBody] MongoEmployee employee)
        {
            if (_mongoService.InsertEmployee(employee))
                return Ok("Inserted into Mongo");
            return BadRequest("Insert failed");
        }

        [HttpPut("sql")]
        public IActionResult UpdateSqlEmployee([FromBody] Employee employee)
        {
            if (_sqlService.UpdateEmployee(employee))
                return Ok("SQL update successful");
            return NotFound("Employee not found");
        }

        [HttpPut("mongo")]
        public IActionResult UpdateMongoEmployee([FromBody] MongoEmployee employee)
        {
            if (_mongoService.UpdateEmployee(employee))
                return Ok("Mongo update successful");
            return NotFound("Employee not found");
        }

        [HttpPut("mongo/replace")]
        public IActionResult ReplaceMongoEmployee([FromBody] MongoEmployee employee)
        {
            if (_mongoService.ReplaceEmployee(employee))
                return Ok("Mongo replace successful");
            return BadRequest("Replace failed");
        }

        [HttpDelete("sql/{id}")]
        public IActionResult DeleteSqlEmployee(int id)
        {
            if (_sqlService.DeleteEmployee(id))
                return Ok("SQL delete successful");
            return NotFound("Employee not found");
        }

        // ❌ Delete Mongo
        [HttpDelete("mongo/{id}")]
        public IActionResult DeleteMongoEmployee(int id)
        {
            if (_mongoService.DeleteEmployee(id))
                return Ok("Mongo delete successful");
            return NotFound("Employee not found");
        }
    }
}
