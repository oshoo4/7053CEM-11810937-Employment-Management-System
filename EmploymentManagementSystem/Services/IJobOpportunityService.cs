using EmploymentManagementSystem.Models;

namespace EmploymentManagementSystem.Services
{
    public interface IJobOpportunityService
    {
        Task<List<JobOpportunity>> GetAllJobOpportunitiesAsync();
        Task<JobOpportunity> GetJobOpportunityByIdAsync(int id);
        Task<JobOpportunity> CreateJobOpportunityAsync(JobOpportunity jobOpportunity);
        Task UpdateJobOpportunityAsync(JobOpportunity jobOpportunity);
        Task DeleteJobOpportunityAsync(int id);
        Task<List<JobOpportunity>> GetFilteredJobOpportunitiesAsync(
            string searchTerm,
            string company,
            string location,
            string sortBy,
            string sortOrder
        );
        Task<List<string>> GetAllCompanyNamesAsync();
        Task<List<(int Value, string Text)>> GetAllCompaniesAsync();
        Task<Company> GetCompanyByIdAsync(int id);
    }
}
