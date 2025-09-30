using AutoMapper;
using IKEA.BLL.Model.Departments;
using IKEA.BLL.Services;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Protocol.Plugins;

namespace IKEA.PL.Controllers
{
    [Authorize]
    //inherit: depController is controller
    //composition:depcontroller has a idepartmentservice
    public class DepartmentController : Controller
    {

        #region Services
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService,
            ILogger<DepartmentController> logger,
            IWebHostEnvironment environment,
            IMapper mapper           



            )
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
            _mapper = mapper;
        }
        #endregion
        #region index
        [HttpGet]//dep/index
        public async Task< IActionResult> Index()
        {
            //view storage : pass data from controller [action]
            //to view [from this view , =>Partial view or layout]
            //1-:view data: is a dictionary     
            ViewData["Message"] = "Hello Ali"; //set title for view
            //1-:ViewBag: is dynamic object
            ViewBag.Message = "Hello Ali"; //set title for view
            var Department =await _departmentService.GetAllDepartmentsAsync();
            return View(Department);

        }
        #endregion
        #region Create
        #region Get
        [HttpGet]
        public async Task< IActionResult> Create()
        {
            return View();
        }
        #endregion
        #region post
        [HttpPost]
        [ValidateAntiForgeryToken] //to prevent cross site request forgery
        public async Task< IActionResult> Create(CreatedDepartmentDTO departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;
            try
            {
               var createdDepartment = _mapper.Map<CreatedDepartmentDTO>(departmentVM);
                var result = await _departmentService.CreateDepartmentAsync(createdDepartment);
                //3-temp data : is a dictionary that is used to pass data from one request to another
                if (result > 0)
                {
                    TempData["Message"] = "Depatment is created";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Message"] = "Sorry The Department Has Not Been Create";
                    message = "Sorry The Department Has Not Been Create";
                    ModelState.AddModelError(string.Empty, message);
                    return View(createdDepartment);
                }
                ///////yes code 

            }

            catch (Exception ex)
            {//مش الطف طريق دي
             //1-log ex
             // _Logger.LogError(ex, ex.Message);
                _logger.LogError(ex, ex.Message);
                //2-set frindly message
                if (_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(departmentVM);
                }
                else
                {
                    message = "Sorry The Department Has Not Been Create";
                    // ModelState.AddModelError(string.Empty,message);
                    return View("Error ", message);
                }
                
            }
        }
        #endregion
        #endregion
        #region Details

        [HttpGet] //department/details/id
        public async Task< IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return BadRequest();//400
            }
            var department =await _departmentService.GetDepartmentsByIdAsync(id.Value);
            if (department is null)
            {
                return NotFound();//404
            }
            return View(department);
        }

        #endregion
        #region ُEdit
        #region get
        [HttpGet]//dep/edit/id?
        public async Task< IActionResult> Edit(int?id)
        {

           if(id is null)
            {
                return BadRequest();//400
            }
            var department =await _departmentService.GetDepartmentsByIdAsync(id.Value);
            if(department is null)
            {
                return NotFound();//404
            }
           var departmentVM = _mapper.Map<DepartmentDetailsToReturnDTO,DepartmentEditViewModel>(department);
            return View(departmentVM);

        
        }

        #endregion
        #region post
        [HttpPost]
        [ValidateAntiForgeryToken] //to prevent cross site request forgery
        public async Task< IActionResult> Edit(int id,DepartmentEditViewModel departmentVM)
        {
            if(!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            var message = string.Empty;
            try
            {
                //هي الحته دي عشان انا عملت فيو موديل يعني من غيره كان عادي؟
                //manual map
                //var updatedDepartment = new UpdatedDepartmentDTO()
                //{
                //    ID = id,
                //    Code = departmentVM.Code,
                //    Name = departmentVM.Name,
                //    Description = departmentVM.Description,
                //    CreationDate = departmentVM.CreationDate,
                //};
                var updatedDepartment = _mapper.Map< UpdatedDepartmentDTO>(departmentVM);
                var updated =await _departmentService.UpdateDepartmentAsync(updatedDepartment)>0;
                if(updated)
                {
                    return RedirectToAction(nameof(Index));

                }
                message = "Sorry , An Error Ocuured While Updaing The Department ";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "Sorry , An Error Ocuured While Updaing The Department";
            }
            ModelState.AddModelError(string.Empty,message);
            return View(departmentVM);

        }
        #endregion
        #endregion
        #region Delete
        #region get
        [HttpGet]
        public async Task< IActionResult> Delete(int?id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            var department = await _departmentService.GetDepartmentsByIdAsync(id.Value);
            if(department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        #endregion
        #region post
        [HttpPost]
        [ValidateAntiForgeryToken] //to prevent cross site request forgery
        public async Task< IActionResult> Delete(int id)
        {
            var message = string.Empty;
            try
            {
                
                var deleted =await _departmentService.DeleteDepartmentAsync(id);
                if (deleted)
                {
                    TempData["Message"] = "Department Is Deleted";
                    return RedirectToAction(nameof(Index));
                }
               
                message = "Sorry,An Error Ocuured During Deleting The Department ";
            }
            catch (Exception ex)
            {//1
                _logger.LogError(ex, ex.Message);
                //2
                message = _environment.IsDevelopment()?ex.Message : "Sorry,An Error Ocuured During Deleting The Department ";


            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #endregion
    } 
}
