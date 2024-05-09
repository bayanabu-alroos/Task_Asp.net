using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_ejabisoft.Data;
using Task_ejabisoft.Models.Entity;
using Task_ejabisoft.Models.ViewModel;
using static Task_ejabisoft.Models.Enum.SystemEnums;

namespace Task_ejabisoft.Controllers
{
    public class JobController : Controller
    {
        private readonly AppDbContext _context;

        public JobController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var jobs = _context.Jobs.Include(j => j.EmployeeJobs)
                                         .ThenInclude(ej => ej.Employee)
                                         .ToList();

            return View(jobs);
        }
        public IActionResult Create()
        {
            return PartialView("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Create(JobCreateViewModel job)
        {
            try
            {
                var jobData = new Job
                {
                    Name = job.Name,
                    Category = job.Category,
                    CreationDate = DateTime.Now,
                };
                await _context.Jobs.AddAsync(jobData);
                await _context.SaveChangesAsync(); // Asynchronous save changes
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred while creating the job.");
            }
            TempData["SuccessMessage"] = "The Create New Job is successful!";
            return RedirectToAction("Index");
        }
        public IActionResult Update()
        {
            return PartialView("Update");
        }


        [HttpPost]
        [Route("/Job/Update")]

        public async Task<IActionResult> Update(JobEditViewModel job)
        {
            try
            {
                var jobData = new Job
                {
                    Id = job.Id,
                    Name = job.Name,
                    Category = job.Category,
                    UpdatDate = DateTime.Now,
                };
                _context.Jobs.Update(jobData);
                await _context.SaveChangesAsync(); // Asynchronous save changes
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An unexpected error occurred while updating the Job.");
            }
            TempData["SuccessMessage"] = "The Job has been updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetEmployees(int jobId)
        {
            var employees = _context.Employees
                .Include(e => e.EmployeeJobs)
                .ThenInclude(ej => ej.Job)
                .Where(e => e.EmployeeJobs.Any(ej => ej.Id == jobId))
                .ToList();

            var jobs = _context.Jobs.ToList();

            // Create an instance of ListJobsEmployeeViewModel and pass the employees and jobs to it
            var viewModel = new ListJobsEmployeeViewModel
            {
                Employees = employees,
                Jobs = jobs
            };

            // Pass the viewModel to the partial view
            return View("Index", viewModel);
        }

        // Action method to activate a job
        [HttpPost]
        public async Task<IActionResult> ActivateJob(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return NotFound();
            }
            job.IsDeleted = false;
            job.UpdatDate = DateTime.Now;   
            _context.Update(job);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "The Activate Job has been  successfully!";
            return RedirectToAction("Index");
        }

        // Action method to deactivate a job
        [HttpPost]
        public async Task<IActionResult> DeactivateJob(int jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return NotFound();
            }

            job.IsDeleted = true;
            job.UpdatDate = DateTime.Now;
            _context.Update(job);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "The Deactivate Job has been  successfully!";
            return RedirectToAction("Index");
        }
    }
}
