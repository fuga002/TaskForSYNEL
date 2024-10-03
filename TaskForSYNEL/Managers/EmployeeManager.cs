using CsvHelper;
using Microsoft.IdentityModel.Tokens;
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


    public async Task<string> AddEmployeeList(IFormFile file)
    {
        var check = file.ContentType == "application/vnd.ms-excel";
        if (check)
        {
            if (file is { Length: > 0 })
            {
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Context.RegisterClassMap<EmployeeModelMap>();
                var employees = csv.GetRecords<Employee>().ToList();
                await _employeeRepository.AddRange(employees);

                var rowsCount = 0;
                foreach (var employee in employees)
                {
                    rowsCount++;
                }

                return $"{rowsCount} rows were successfully";
            }
            else
            {
                return "File is empty, please try again";
            }
        }
        else
        {
            return "This file is not \"CSV\" type, please try again";
        }
    }

    public async Task<string> EditEmployee(Guid id,EditEmployeeModel model)
    {
        var employee = await _employeeRepository.GetById(id);
        (bool check,employee) = ParseToEntity(employee, model);

        if (check)
            await _employeeRepository.Update(employee);

        return "Successfully edited!";

    }

    private Tuple<bool, Employee> ParseToEntity(Employee employee ,EditEmployeeModel model)
    {
        bool check = false;


        if (!string.IsNullOrEmpty(model.PayrollNumber))
        {
            employee.PayrollNumber = model.PayrollNumber!;
            check = true;
        }
        if (!string.IsNullOrEmpty(model.Forenames))
        {
            employee.Forenames = model.Forenames!;
            check = true;
        }
        if (!string.IsNullOrEmpty(model.Surname))
        {
            employee.Surname = model.Surname!;
            check = true;
        }
        if (!string.IsNullOrEmpty(model.Telephone))
        {
            employee.Telephone = model.Telephone!;
            check = true;
        }
        if (!string.IsNullOrEmpty(model.Mobile))
        {
            employee.Mobile = model.Mobile!;
            check = true;
        }
        if (!string.IsNullOrEmpty(model.Address))
        {
            employee.Address = model.Address!;
            check = true;
        }
        if (!string.IsNullOrEmpty(model.Address2))
        {
            employee.Address2 = model.Address2!;
            check = true;
        }
        if (!string.IsNullOrEmpty(model.Postcode))
        {
            employee.Postcode = model.Postcode!;
            check = true;
        }
        if (!string.IsNullOrEmpty(model.EmailHome))
        {
            employee.EmailHome = model.EmailHome!;
            check = true;
        }

        return new(check, employee);
    }


}