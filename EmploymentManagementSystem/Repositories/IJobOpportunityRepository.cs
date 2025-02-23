using EmploymentManagementSystem.Models;

namespace EmploymentManagementSystem.Repositories
{
    // POLYMORPHISM(GRASP Patterns)
    // ABSTRACTION(Object-Oriented Programming)
    public interface IJobOpportunityRepository
    {
        Task<List<JobOpportunity>> GetAllJobOpportunitiesAsync();
        Task<JobOpportunity> GetJobOpportunityByIdAsync(int id);
        Task<JobOpportunity> AddJobOpportunityAsync(JobOpportunity jobOpportunity);
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
