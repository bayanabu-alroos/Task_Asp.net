using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Transactions;
using Task_ejabisoft.Data;
using Task_ejabisoft.Models;
using Task_ejabisoft.Models.Entity;
using Task_ejabisoft.Models.ViewModel;
using static Task_ejabisoft.Models.Enum.SystemEnums;

namespace Task_ejabisoft.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public HomeController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var employeeQuery = _context.Employees.Include(e => e.EmployeeJobs)
                                                  .ThenInclude(ej => ej.Job)
                                                  .AsQueryable();
            
            if (!String.IsNullOrEmpty(searchTerm))
            {
                employeeQuery = employeeQuery.Where(e =>
                    e.Name.Contains(searchTerm) ||  
                    e.EmployeeJobs.Any(ej => ej.Job.Name.Contains(searchTerm))); 
            }
            var employeesData = await employeeQuery.Select(e => new Employee
            {
                Id = e.Id,
                Name = e.Name,
                Birthdate = e.Birthdate,
                Personal_Photo = e.Personal_Photo,
                Phone = e.Phone,
                Employment_Date = e.Employment_Date,
                EmployeeJobs = e.EmployeeJobs,
                IsDeleted = e.IsDeleted
            }).ToListAsync();

            return View(employeesData);
        }


        [HttpGet]
        [Route("/EmployeeJobs")]
        public async Task<IActionResult> EmployeeJobs()
        {
            var employeeJobsQuery = await _context.EmployeeJobs
               .Include(ej => ej.Employee)
               .Include(e => e.Job) // Include the related job for each employee
               .ToListAsync();
            var jobs = _context.Jobs.ToList();
            var employee = _context.Employees.ToList();
            ViewBag.Jobs = jobs;
            ViewBag.Employees = employee;

            return View(employeeJobsQuery);
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateViewModel employee, IFormFile imageFile)
        {
            try
            {
                string imageName = "~/Image/" + UploadFile(imageFile);
                var employeeData = new Employee
                {
                    Name = employee.Name,
                    Birthdate=employee.Birthdate,
                    Personal_Photo = imageName, 
                    Phone = employee.Phone,
                    Employment_Date =employee.Employment_Date,                    Under_Probation = employee.Under_Probation,
                    Governorate = employee.Governorate,
                    CreationDate = DateTime.Now,
                    IsDeleted = false,
                };
                await _context.Employees.AddAsync(employeeData);
                await _context.SaveChangesAsync(); // Asynchronous save changes
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred while creating the job.");
            }
            TempData["SuccessMessage"] = "The Create New Employee is successful!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeJobs(EmployeeJobCreateViewModel employeeJob)
        {
            try
            {
                // Retrieve the selected job and employee from the database
                var job = await _context.Jobs.FindAsync(employeeJob.Job);
                var employee = await _context.Employees.FindAsync(employeeJob.Employee);

                if (job == null || employee == null)
                {
                    ModelState.AddModelError("", "Selected job or employee not found.");
                    return View(employeeJob); 
                }
                var employeeData = new EmployeeJob
                {
                    Job = job,
                    Employee = employee,
                    CreationDate = DateTime.Now,
                    IsDeleted = false
                };

                await _context.EmployeeJobs.AddAsync(employeeData);
                await _context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred while creating the job.");
            }

            TempData["SuccessMessage"] = "The Create New Employee Job was successful!";
            return RedirectToAction("EmployeeJobs");
        }


        public IActionResult Update()
        {
            return PartialView("Update");
        }


        [HttpPost]
        [Route("/Update")]
        public async Task<IActionResult> Update(EmployeeEditViewModel employee, IFormFile imageFile)
        {
            try
            {
                // Logic to update the employee record
                string imageName = "~/Image/" + UploadFile(imageFile);
                var employeeData = new Employee
                {
                    Name = employee.Name,
                    Birthdate = employee.Birthdate,
                    Personal_Photo = imageName,
                    Phone = employee.Phone,
                    Id = employee.Id,
                    Employment_Date = employee.Employment_Date,
                    Under_Probation = employee.Under_Probation,
                    Governorate = employee.Governorate,
                    IsDeleted = false,
                };
                _context.Employees.Update(employeeData);
                await _context.SaveChangesAsync(); // Asynchronous save changes
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred while updating the Employee.");
            }
            TempData["SuccessMessage"] = "The Employee has been updated successfully!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("/DeletedEmployee")]

        public async Task<IActionResult> DeletedEmployee(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            employee.IsDeleted = true;
            employee.UpdatDate = DateTime.Now;
            _context.Update(employee);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "The Employee has been Deleted successfully!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("/ActivateJobEmployee")]
        public async Task<IActionResult> ActivateJobEmployee(int jobEmployeeId)
        {
            var jobEmployee = await _context.EmployeeJobs.FindAsync(jobEmployeeId);
            if (jobEmployee == null)
            {
                return NotFound();
            }

            jobEmployee.IsDeleted = false; // Activate the job by setting IsDeleted to false
            jobEmployee.UpdatDate = DateTime.Now;
            _context.EmployeeJobs.Update(jobEmployee);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "The Activate Job has been successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("/DeactivateJobEmployee")]
        public async Task<IActionResult> DeactivateJobEmployee(int jobEmployeeId)
        {
            var jobEmployee = await _context.EmployeeJobs.FindAsync(jobEmployeeId);
            if (jobEmployee == null)
            {
                return NotFound();
            }

            jobEmployee.IsDeleted = true; // Deactivate the job by setting IsDeleted to true
            jobEmployee.UpdatDate = DateTime.Now;
            _context.EmployeeJobs.Update(jobEmployee);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "The Deactivate Job has been successfully!";
            return RedirectToAction("Index");
        }


        #region Upload File 
        public string UploadFile(IFormFile model)
        {
            string uploadFileName = string.Empty;
            try
            {
                if (model != null)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image");
                    uploadFileName = Guid.NewGuid().ToString() + "_" + model.FileName;
                    string fullPath = Path.Combine(uploadFolder, uploadFileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        model.CopyTo(fileStream);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while uploading the image.");
            }

            return uploadFileName;
        }
        #endregion


    }
}
