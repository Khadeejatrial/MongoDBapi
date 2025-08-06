using MongoDBapi.Domain;

namespace MongoDBapi.Abstract
{
    public interface ISqlService
    {
        bool InsertEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);
        bool ReplaceEmployee(Employee employee);
        bool DeleteEmployee(int employeeId);
        Employee GetEmployeeById(int employeeId);
        List<Project> GetProjectsByEmployeeId(int employeeId);
    }
}
