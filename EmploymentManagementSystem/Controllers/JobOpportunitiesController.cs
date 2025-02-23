using EmploymentManagementSystem.Models;
using EmploymentManagementSystem.Services;
using EmploymentManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmploymentManagementSystem.Controllers
{
    // @CONTROLLER(Layered Architectural Design)
    // CONTROLLER(Model-View-Controller)
    // CONTROLLER(GRASP Patterns)
    public class JobOpportunitiesController : Controller
    {
        private readonly IJobOpportunityService _jobOpportunityService;

        public JobOpportunitiesController(IJobOpportunityService jobOpportunityService)
        {
            _jobOpportunityService = jobOpportunityService;
        }

        public async Task<IActionResult> Index(
            string searchTerm,
            string company,
            string location,
            string sortBy,
            string sortOrder
        )
        {
            // INDIRECTION (GRASP Patterns)
            // DONT TALK TO STRANGERS(GRASP Patterns)
            var jobOpportunities = await _jobOpportunityService.GetFilteredJobOpportunitiesAsync(
                searchTerm,
                company,
                location,
                sortBy,
                sortOrder
            );

            ViewBag.CurrentFilter = searchTerm;
            ViewBag.CompanyFilter = company;
            ViewBag.LocationFilter = location;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;

            var companyNames = await _jobOpportunityService.GetAllCompanyNamesAsync();
            ViewBag.Companies = new SelectList(companyNames);

            return View(jobOpportunities);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOpportunity = await _jobOpportunityService.GetJobOpportunityByIdAsync(id.Value);
            if (jobOpportunity == null)
            {
                return NotFound();
            }
            return View(jobOpportunity);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateJobOpportunityViewModel();

            viewModel.ClosingDate = DateOnly.FromDateTime(DateTime.Now);
            viewModel.PostedDate = DateOnly.FromDateTime(DateTime.Now);

            var companies = await _jobOpportunityService.GetAllCompaniesAsync();
            viewModel.Companies = companies
                .Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Text })
                .ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobOpportunityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var jobOpportunity = new JobOpportunity
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    CompanyId = viewModel.CompanyId,
                    Location = viewModel.Location,
                    Salary = viewModel.Salary,
                    ClosingDate = viewModel.ClosingDate,
                    PostedDate = DateOnly.FromDateTime(DateTime.Now),
                    IsActive = viewModel.IsActive,
                };

                var company = await _jobOpportunityService.GetCompanyByIdAsync(
                    jobOpportunity.CompanyId
                );
                if (company == null)
                {
                    return NotFound();
                }
                jobOpportunity.Company = company.Name;
                await _jobOpportunityService.CreateJobOpportunityAsync(jobOpportunity);
                return RedirectToAction(nameof(Index));
            }

            var companies = await _jobOpportunityService.GetAllCompaniesAsync();
            viewModel.Companies = companies
                .Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Text })
                .ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOpportunity = await _jobOpportunityService.GetJobOpportunityByIdAsync(id.Value);
            if (jobOpportunity == null)
            {
                return NotFound();
            }

            var viewModel = new EditJobOpportunityViewModel
            {
                Id = jobOpportunity.Id,
                Title = jobOpportunity.Title,
                Description = jobOpportunity.Description,
                CompanyId = jobOpportunity.CompanyId,
                Location = jobOpportunity.Location,
                Salary = jobOpportunity.Salary,
                ClosingDate = jobOpportunity.ClosingDate,
                PostedDate = DateOnly.FromDateTime(DateTime.Now),
                IsActive = jobOpportunity.IsActive,
            };

            var companies = await _jobOpportunityService.GetAllCompaniesAsync();
            viewModel.Companies = companies
                .Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Text })
                .ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditJobOpportunityViewModel viewModel) // Use ViewModel
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var jobOpportunity = new JobOpportunity
                    {
                        Id = viewModel.Id,
                        Title = viewModel.Title,
                        Description = viewModel.Description,
                        CompanyId = viewModel.CompanyId,
                        Location = viewModel.Location,
                        Salary = viewModel.Salary,
                        ClosingDate = viewModel.ClosingDate,
                        PostedDate = DateOnly.FromDateTime(DateTime.Now),
                        IsActive = viewModel.IsActive,
                    };

                    var company = await _jobOpportunityService.GetCompanyByIdAsync(
                        jobOpportunity.CompanyId
                    );
                    if (company == null)
                    {
                        return NotFound();
                    }
                    jobOpportunity.Company = company.Name;

                    await _jobOpportunityService.UpdateJobOpportunityAsync(jobOpportunity);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobOpportunityExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var companies = await _jobOpportunityService.GetAllCompaniesAsync();
            viewModel.Companies = companies
                .Select(c => new SelectListItem { Value = c.Value.ToString(), Text = c.Text })
                .ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOpportunity = await _jobOpportunityService.GetJobOpportunityByIdAsync(id.Value);
            if (jobOpportunity == null)
            {
                return NotFound();
            }

            return View(jobOpportunity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _jobOpportunityService.DeleteJobOpportunityAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool JobOpportunityExists(int id)
        {
            return _jobOpportunityService.GetJobOpportunityByIdAsync(id) != null;
        }
    }
}
