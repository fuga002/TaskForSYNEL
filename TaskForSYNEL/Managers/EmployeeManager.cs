using CsvHelper;
using System.Globalization;
using TaskForSYNEL.Entities;
using TaskForSYNEL.Models;
using TaskForSYNEL.Repositories;

namespace TaskForSYNEL.Managers;

public class EmployeeManager(IEmployeeRepository employeeRepository)
{
    private readonly IEmployeeRepository _employeeRepository = employeeRepository;


    public async Task<List<Employee>> GetAllEmployees()
    {
        var employees = await _employeeRepository.GetAll();
        return employees;
    }


    public async Task<Employee> GetEmployeeById(Guid id)
    {
        var employee = await _employeeRepository.GetById(id);
        return employee;
    }


    public async Task<List<Employee>> AddEmployeeList(IFormFile file)
    {
        if (file is { Length: > 0 })
        {
            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<EmployeeModelMap>();
            var employees = csv.GetRecords<Employee>().ToList();
                

            await _employeeRepository.AddRange(employees);
        }

        var employeesList =  await _employeeRepository.GetAll();
       return employeesList;
    }


}