
using MongoDBapi.Domain;

namespace MongoDBapi.Abstract
{
    public interface IMongoService
    {
        bool InsertEmployee(MongoEmployee employee);
        bool UpdateEmployee(MongoEmployee employee);
        bool ReplaceEmployee(MongoEmployee employee);
        void ReplaceAllEmployees(List<MongoEmployee> employees);
        List<MongoEmployee> GetAll();
        bool DeleteEmployee(int employeeId);
        MongoEmployee GetEmployeeById(int employeeId);
        List<MongoEmployee> GetAllEmployees();

    }
}
