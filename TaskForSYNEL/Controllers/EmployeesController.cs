
using Microsoft.AspNetCore.Mvc;

using TaskForSYNEL.Managers;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace TaskForSYNEL.Controllers;

public class EmployeesController(EmployeeManager employeeManager) : Controller
{
    private readonly EmployeeManager _employeeManager = employeeManager;

    public async Task<IActionResult> AllEmployees()
    {
        var employees = await _employeeManager.GetAllEmployees();
        return View(employees);
    }
    

    [Microsoft.AspNetCore.Mvc.HttpPost]
    public async Task<IActionResult> UploadCsv(IFormFile csvFile)
    {

       var employees = await _employeeManager.AddEmployeeList(csvFile);

        return RedirectToAction("UploadCsv",employees);
    }

    public IActionResult Edit(Guid id)
    {
        throw new NotImplementedException();
    }
}