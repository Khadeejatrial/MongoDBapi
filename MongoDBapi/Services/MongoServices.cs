using MongoDB.Driver;
using MongoDBapi.Abstract;
using MongoDBapi.Domain;
using Microsoft.Extensions.Configuration;

namespace MongoDB.Services
{
    public class MongoService : IMongoService
    {
        private readonly IMongoCollection<MongoEmployee> _collection;

        public MongoService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            var database = client.GetDatabase("EmployeeDB");
            _collection = database.GetCollection<MongoEmployee>("Employee");
        }

        public List<MongoEmployee> GetAllEmployees()
        {
            return _collection.Find(emp => true).ToList();
        }

        public MongoEmployee? GetEmployeeById(int employeeId)
        {
            return _collection.Find(emp => emp.EmployeeID == employeeId).FirstOrDefault();
        }

        public bool InsertEmployee(MongoEmployee employee)
        {
            _collection.InsertOne(employee);
            return true;
        }

        public bool UpdateEmployee(MongoEmployee employee)
        {
            var filter = Builders<MongoEmployee>.Filter.Eq(e => e.EmployeeID, employee.EmployeeID);
            var update = Builders<MongoEmployee>.Update
                .Set(e => e.FirstName, employee.FirstName)
                .Set(e => e.LastName, employee.LastName)
                .Set(e => e.Salary, employee.Salary)
                .Set(e => e.JoiningDate, employee.JoiningDate)
                .Set(e => e.Department, employee.Department)
                .Set(e => e.Gender, employee.Gender)
                .Set(e => e.IsActive, employee.IsActive)
                .Set(e => e.Projects, employee.Projects);

            var result = _collection.UpdateOne(filter, update);
            return result.ModifiedCount > 0;
        }

        public bool ReplaceEmployee(MongoEmployee employee)
        {
            var filter = Builders<MongoEmployee>.Filter.Eq(e => e.EmployeeID, employee.EmployeeID);
            var result = _collection.ReplaceOne(filter, employee, new ReplaceOptions { IsUpsert = true });
            return result.ModifiedCount > 0 || result.UpsertedId != null;
        }

        public bool DeleteEmployee(int employeeId)
        {
            var filter = Builders<MongoEmployee>.Filter.Eq(e => e.EmployeeID, employeeId);
            var result = _collection.DeleteOne(filter);
            return result.DeletedCount > 0;
        }

        
        public void ReplaceAllEmployees(List<MongoEmployee> employees)
        {
            _collection.DeleteMany(FilterDefinition<MongoEmployee>.Empty);
            _collection.InsertMany(employees);
        }

        public List<MongoEmployee> GetAll()
        {
            return _collection.Find(FilterDefinition<MongoEmployee>.Empty).ToList();
        }
    }
}
