using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskForSYNEL.Entities;
using TaskForSYNEL.Managers;
using TaskForSYNEL.Models;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace TaskForSYNEL.Controllers;

public class EmployeesController(EmployeeManager employeeManager) : Controller
{
    private readonly EmployeeManager _employeeManager = employeeManager;

    public async Task<IActionResult> AllEmployees()
    {
        List<Employee> employees = employees = await _employeeManager.GetAllEmployees();

        if (TempData["Result"] is not null)
        {
            var result = JsonConvert.DeserializeObject<string>(TempData["Result"]?.ToString()!);

            ViewBag.Result = result!;
        }

        return View(employees);
    }
    

    [Microsoft.AspNetCore.Mvc.HttpPost]
    public async Task<IActionResult> UploadCsv(IFormFile csvFile)
    {
       var result = await _employeeManager.AddEmployeeList(csvFile);
       TempData["Result"] = JsonConvert.SerializeObject(result);
       return RedirectToAction("AllEmployees");
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var employee = await _employeeManager.GetEmployeeById(id);

        var model = employee.Adapt<EditEmployeeModel>();

        ViewBag.Id = employee.Id;

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> EditPost(Guid id,EditEmployeeModel model)
    {
        var result = await _employeeManager.EditEmployee(id, model);

        TempData["Result"] = JsonConvert.SerializeObject(result);

        return RedirectToAction("AllEmployees");

    }
}