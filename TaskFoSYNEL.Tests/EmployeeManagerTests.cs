using Microsoft.AspNetCore.Http;
using Moq;
using TaskForSYNEL.Entities;
using TaskForSYNEL.Managers;
using TaskForSYNEL.Models;
using TaskForSYNEL.Repositories;

namespace TaskFoSYNEL.Tests;

public class EmployeeManagerTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly EmployeeManager _employeeManager;

    public EmployeeManagerTests()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _employeeManager = new EmployeeManager(_employeeRepositoryMock.Object);
    }

    // Test for GetAllEmployees
    [Fact]
    public async Task GetAllEmployees_ShouldReturnListOfEmployees()
    {
        var employees = new List<Employee>
            {
                new Employee { PayrollNumber = "JACK13", Forenames = "Jerry", Surname = "Jackson" },
                new Employee { PayrollNumber = "COOP08", Forenames = "John", Surname = "William" }
            };

        _employeeRepositoryMock.Setup(repo => repo.GetAll())
                               .ReturnsAsync(employees);

        var result = await _employeeManager.GetAllEmployees();

        
        Assert.Equal(2, result.Count);
        Assert.Equal("JACK13", result[0].PayrollNumber);
    }

    // Test for GetEmployeeById
    [Fact]
    public async Task GetEmployeeById_ShouldReturnEmployee_WhenEmployeeExists()
    {
        var employeeId = Guid.NewGuid();
        var employee = new Employee { PayrollNumber = "JACK13", Forenames = "Jerry", Surname = "Jackson" };

        _employeeRepositoryMock.Setup(repo => repo.GetById(employeeId))
                               .ReturnsAsync(employee);

        var result = await _employeeManager.GetEmployeeById(employeeId);

        Assert.Equal("JACK13", result.PayrollNumber);
        Assert.Equal("Jerry", result.Forenames);
    }

    // Test for AddEmployeeList - valid CSV file
    [Fact]
    public async Task AddEmployeeList_ShouldReturnSuccessMessage_WhenCsvIsValid()
    {
        var mockFile = CreateMockCsvFile("PayrollNumber,Forenames,Surname\nJACK13,Jerry,Jackson");

        _employeeRepositoryMock.Setup(repo => repo.AddRange(It.IsAny<List<Employee>>()))
                               .Returns(Task.CompletedTask);

        var result = await _employeeManager.AddEmployeeList(mockFile);

        Assert.Equal("1 rows were successfully", result);
    }

    // Test for AddEmployeeList - invalid file type
    [Fact]
    public async Task AddEmployeeList_ShouldReturnErrorMessage_WhenFileTypeIsInvalid()
    {
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.ContentType).Returns("application/pdf"); // Invalid file type

        var result = await _employeeManager.AddEmployeeList(mockFile.Object);

        Assert.Equal("This file is not \"CSV\" type, please try again", result);
    }

    // Test for EditEmployee
    [Fact]
    public async Task EditEmployee_ShouldUpdateEmployee_WhenValidDataIsProvided()
    {
        var employeeId = Guid.NewGuid();
        var existingEmployee = new Employee { PayrollNumber = "JACK13", Forenames = "Jerry", Surname = "Jackson" };
        var editModel = new EditEmployeeModel { Forenames = "John", Surname = "Smith" };

        _employeeRepositoryMock.Setup(repo => repo.GetById(employeeId))
                               .ReturnsAsync(existingEmployee);

        _employeeRepositoryMock.Setup(repo => repo.Update(It.IsAny<Employee>()))
                               .Returns(Task.CompletedTask);

        var result = await _employeeManager.EditEmployee(employeeId, editModel);

        Assert.Equal("Successfully edited!", result);
    }

    // Helper function to create a mock CSV file
    private IFormFile CreateMockCsvFile(string content)
    {
        var fileName = "test.csv";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(content);
        writer.Flush();
        ms.Position = 0;

        var formFile = new FormFile(ms, 0, ms.Length, "id_from_form", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/vnd.ms-excel" // CSV content type
        };

        return formFile;
    }
}