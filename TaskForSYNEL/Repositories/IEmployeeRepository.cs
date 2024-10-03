using TaskForSYNEL.Entities;

namespace TaskForSYNEL.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAll();
    Task<Employee> GetById(Guid id);
    Task AddRange(List<Employee> employeeList);
}