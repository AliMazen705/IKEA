using IKEA.BLL.Common.Services;
using IKEA.BLL.Model.Departments;
using IKEA.BLL.Services;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Presistance.Data;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IKEA.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
       

        #region Services
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDepartmentService _departmentService;


        //inject
        public EmployeeController(IEmployeeService employeeService,
            ILogger<EmployeeController> logger,
            IWebHostEnvironment webHostEnvironment
            , IDepartmentService departmentService
          


            )
        {
            _employeeService = employeeService;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _departmentService = departmentService; 
        }
        #endregion

        #region Index
        [HttpGet]//employee/index
        public async Task< IActionResult> Index(string Search)
        {
            var employees = await _employeeService.GetAllEmployeesAsync(Search);
            return View(employees);
        }
        #endregion

        #region Create
        #region Get
        [HttpGet]
        public async Task< IActionResult> Create([FromServices] IDepartmentService departmentService) 
        {
            //ViewData["Departments"]
            ViewData["Departments"] =  await departmentService.GetAllDepartmentsAsync();

            return View();

        }

        #endregion
        #region post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(CreatedEmployeeDto employee)
        {
            if (!ModelState.IsValid)
                return View(employee);
            var message = string.Empty;
            try
            {
                var result =await _employeeService.CreateEmployeeAsync(employee);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    message = "Sorry The Employee Has Not Been Create";
                    ModelState.AddModelError(string.Empty, message);
                   
                    return View(employee);
                }
               

            }

            catch (Exception ex)
            {//مش الطف طريق دي
             //1-log ex
             // _Logger.LogError(ex, ex.Message);
                _logger.LogError(ex, ex.Message);
                //2-set frindly message
                if (_webHostEnvironment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(employee);
                }
                else
                {
                    message = "Sorry The Department Has Not Been Create";
                     //ModelState.AddModelError(string.Empty,message);
                    return View("Error ", message);
                }

            }
        }



        #endregion
        #endregion
        #region Details
        [HttpGet]
        public async Task< IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest(); // 400
            }


            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
            {
                return NotFound(); // 404
            }

            return View(employee);
        }
        #endregion

        #region Edit
        #region get
        [HttpGet]//dep/edit/id?
        public async Task< IActionResult> Edit(int? id) //[FromServices]IDepartmentService departmentService
        {

            if (id is null)
            {
                return BadRequest();//400
            }
            var Employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (Employee is null)
            {
                return NotFound();//404
            }
            ViewData["Departments"] =await _departmentService.GetAllDepartmentsAsync(); //عشان تجبلي كل الاقسام قدامي واعرف اختار منهم جوا الفيو نفسه
            return View(new UpdatedEmployeeDto()
            {
               
                Name = Employee.Name,
               Address = Employee.Address,
               Email= Employee.Email,
               Age = Employee.Age,
               Salary = Employee.Salary,
               PhoneNumber = Employee.PhoneNumber,
               IsActive = Employee.IsActive,
               EmployeeType=Employee.EmployeeType,
               Gender= Employee.Gender,
               HiringDate=Employee.HiringDate,
            });
        }


        #endregion
        #region post
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task< IActionResult> Edit([FromRoute] int id, UpdatedEmployeeDto employee)
        {
            if(!ModelState.IsValid) //server side validation
                return View(employee);

            var message = string.Empty;

            try
            {
                var updated =await _employeeService.UpdateEmployeeAsync(employee) > 0;
                if (updated)
                    return RedirectToAction(nameof(Index));
                message = "Employee is not updated";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                if (_webHostEnvironment.IsDevelopment())
                    message = ex.Message;
                else
                    message = "Employee is not craeted";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(employee);
        }
        #endregion
        #endregion
        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var message = string.Empty;
            try
            {

                var deleted = await _employeeService.DeleteEmployeeAsync(id);
                if (deleted)
                    return RedirectToAction(nameof(Index));
                message = "An Error Occured  During The Deleting Of Employee";
            }
            catch (Exception ex)
            {
                //1- Log Exception
                _logger.LogError(ex, ex.Message);
                //2- Set Message
                message = _webHostEnvironment.IsDevelopment() ? ex.Message : "Sorry An Error Occured During Deleting  The Department :(";
            }
            return RedirectToAction(nameof(Index));


        }

        #endregion


    }
}
