using EmploymentManagementSystem.Data;
using EmploymentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploymentManagementSystem.Repositories
{
    // @REPOSITORY(Layered Architectural Design)
    public class JobOpportunityRepository : IJobOpportunityRepository
    {
        // INFORMATION HIDING (Object-Oriented Programming)
        private readonly ApplicationDbContext _context;

        public JobOpportunityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobOpportunity>> GetAllJobOpportunitiesAsync()
        {
            return await _context.JobOpportunities.Include(j => j.CompanyObject).ToListAsync();
        }

        public async Task<JobOpportunity> GetJobOpportunityByIdAsync(int id)
        {
            return await _context
                .JobOpportunities.Include(j => j.CompanyObject)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // CREATOR(GRASP Patterns)
        public async Task<JobOpportunity> AddJobOpportunityAsync(JobOpportunity jobOpportunity)
        {
            _context.JobOpportunities.Add(jobOpportunity);
            await _context.SaveChangesAsync();
            return jobOpportunity;
        }

        public async Task UpdateJobOpportunityAsync(JobOpportunity jobOpportunity)
        {
            _context.Entry(jobOpportunity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteJobOpportunityAsync(int id)
        {
            var jobOpportunity = await _context.JobOpportunities.FindAsync(id);
            if (jobOpportunity != null)
            {
                _context.JobOpportunities.Remove(jobOpportunity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<JobOpportunity>> GetFilteredJobOpportunitiesAsync(
            string searchTerm,
            string company,
            string location,
            string sortBy,
            string sortOrder
        )
        {
            IQueryable<JobOpportunity> query = _context.JobOpportunities.Include(j =>
                j.CompanyObject
            );

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(j => j.Title.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(company))
            {
                query = query.Where(j => j.CompanyObject.Name == company);
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(j => j.Location == location);
            }

            switch (sortBy)
            {
                case "title":
                    query =
                        sortOrder == "desc"
                            ? query.OrderByDescending(j => j.Title)
                            : query.OrderBy(j => j.Title);
                    break;
                case "company":
                    query =
                        sortOrder == "desc"
                            ? query.OrderByDescending(j => j.CompanyObject.Name)
                            : query.OrderBy(j => j.CompanyObject.Name);
                    break;
                case "location":
                    query =
                        sortOrder == "desc"
                            ? query.OrderByDescending(j => j.Location)
                            : query.OrderBy(j => j.Location);
                    break;
                case "salary":
                    query =
                        sortOrder == "desc"
                            ? query.OrderByDescending(j => j.Salary)
                            : query.OrderBy(j => j.Salary);
                    break;
                case "closingDate":
                    query =
                        sortOrder == "desc"
                            ? query.OrderByDescending(j => j.ClosingDate)
                            : query.OrderBy(j => j.ClosingDate);
                    break;
                case "isActive":
                    query =
                        sortOrder == "desc"
                            ? query.OrderByDescending(j => j.IsActive)
                            : query.OrderBy(j => j.IsActive);
                    break;
                default:
                    query = query.OrderBy(j => j.PostedDate);
                    break;
            }

            return await query.ToListAsync();
        }

        public async Task<List<string>> GetAllCompanyNamesAsync()
        {
            return await _context.Companies.Select(c => c.Name).ToListAsync();
        }

        public async Task<List<(int Value, string Text)>> GetAllCompaniesAsync()
        {
            return await _context
                .Companies.Select(c => new ValueTuple<int, string>(c.Id, c.Name))
                .ToListAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }
    }
}
