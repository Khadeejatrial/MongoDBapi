using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MongoDBapi.Abstract;
using MongoDBapi.Domain;
using System.Data;

namespace MongoDBapi.Services
{
    public class SQLService : ISqlService
    {
        private readonly string _connectionString;

        public SQLService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string not found.");
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("SELECT * FROM Employee", connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    EmployeeID = (int)reader["EmployeeId"],
                    FirstName = reader["FirstName"]?.ToString() ?? "",
                    LastName = reader["LastName"]?.ToString() ?? "",
                    Salary = Convert.ToDecimal(reader["Salary"]),
                    JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                    Department = reader["Department"]?.ToString() ?? "",
                    Gender = reader["Gender"]?.ToString() ?? "",
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                });
            }

            return employees;
        }

        public Employee? GetEmployeeById(int employeeId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("SELECT * FROM Employee WHERE EmployeeId = @EmployeeId", connection);

            command.Parameters.AddWithValue("@EmployeeId", employeeId);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Employee
                {
                    EmployeeID = (int)reader["EmployeeId"],
                    FirstName = reader["FirstName"]?.ToString() ?? "",
                    LastName = reader["LastName"]?.ToString() ?? "",
                    Salary = Convert.ToDecimal(reader["Salary"]),
                    JoiningDate = Convert.ToDateTime(reader["JoiningDate"]),
                    Department = reader["Department"]?.ToString() ?? "",
                    Gender = reader["Gender"]?.ToString() ?? "",
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                };
            }

            return null;
        }

        public bool InsertEmployee(Employee employee)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(@"
                INSERT INTO Employee (FirstName, LastName, Salary, JoiningDate, Department, Gender, IsActive)
                VALUES (@FirstName, @LastName, @Salary, @JoiningDate, @Department, @Gender, @IsActive)", connection);

            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Salary", employee.Salary);
            command.Parameters.AddWithValue("@JoiningDate", employee.JoiningDate);
            command.Parameters.AddWithValue("@Department", employee.Department);
            command.Parameters.AddWithValue("@Gender", employee.Gender);
            command.Parameters.AddWithValue("@IsActive", employee.IsActive);

            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool UpdateEmployee(Employee employee)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(@"
                UPDATE Employee 
                SET FirstName = @FirstName, LastName = @LastName, Salary = @Salary, 
                    JoiningDate = @JoiningDate, Department = @Department, Gender = @Gender, IsActive = @IsActive
                WHERE EmployeeId = @EmployeeId", connection);

            command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeID);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Salary", employee.Salary);
            command.Parameters.AddWithValue("@JoiningDate", employee.JoiningDate);
            command.Parameters.AddWithValue("@Department", employee.Department);
            command.Parameters.AddWithValue("@Gender", employee.Gender);
            command.Parameters.AddWithValue("@IsActive", employee.IsActive);

            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool ReplaceEmployee(Employee employee)
        {
            DeleteEmployee(employee.EmployeeID);
            return InsertEmployee(employee);
        }

        public bool DeleteEmployee(int employeeId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM Employee WHERE EmployeeId = @EmployeeId", connection);
            command.Parameters.AddWithValue("@EmployeeId", employeeId);

            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public List<Project> GetProjectsByEmployeeId(int employeeId)
        {
            var projects = new List<Project>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("SELECT * FROM Project WHERE EmployeeId = @EmployeeId", connection);
            command.Parameters.AddWithValue("@EmployeeId", employeeId);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                projects.Add(new Project
                {
                    ProjectDetailID = (int)reader["ProjectId"],
                    EmployeeDetailID = (int)reader["EmployeeId"],
                    ProjectName = reader["ProjectName"]?.ToString() ?? "",
                    
                });
            }

            return projects;
        }
    }
}
