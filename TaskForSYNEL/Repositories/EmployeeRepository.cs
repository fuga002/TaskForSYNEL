using Microsoft.EntityFrameworkCore;
using TaskForSYNEL.Context;
using TaskForSYNEL.Entities;
using TaskForSYNEL.Exceptions;

namespace TaskForSYNEL.Repositories;

public class EmployeeRepository(AppDbContext appDbContext) : IEmployeeRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;
    public async Task<List<Employee>> GetAll()
    {
       var employees = await _appDbContext.
           Employees.
           OrderBy(p => p.Surname).
           ToListAsync();
       return employees;
    }

    public async Task<Employee> GetById(Guid id)
    {
        var employee = await _appDbContext.Employees.
            SingleOrDefaultAsync(e => e.Id == id);

        if (employee is null)
            throw new NotFoundEmployeeException();

        return employee;
    }

    public async Task AddRange(List<Employee> employeeList)
    {
        await _appDbContext.Employees.AddRangeAsync(employeeList);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Update(Employee employee)
    {
        _appDbContext.Employees.Update(employee);
        await _appDbContext.SaveChangesAsync();
    }
}