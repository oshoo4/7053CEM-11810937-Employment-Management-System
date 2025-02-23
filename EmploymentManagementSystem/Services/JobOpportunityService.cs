using System.ComponentModel.DataAnnotations;
using EmploymentManagementSystem.Models;
using EmploymentManagementSystem.Repositories;

namespace EmploymentManagementSystem.Services
{
    // @SERVICE(Layered Architectural Design)
    // EXPERT(GRASP Patterns)
    // PURE FABRICATION(GRASP Patterns)
    public class JobOpportunityService : IJobOpportunityService
    {
        private readonly IJobOpportunityRepository _jobOpportunityRepository;

        // LOW COUPLING(GRASP Patterns)
        public JobOpportunityService(IJobOpportunityRepository jobOpportunityRepository)
        {
            _jobOpportunityRepository = jobOpportunityRepository;
        }

        // HIGH COHESION(GRASP Patterns)
        public async Task<List<JobOpportunity>> GetAllJobOpportunitiesAsync()
        {
            return await _jobOpportunityRepository.GetAllJobOpportunitiesAsync();
        }

        public async Task<JobOpportunity> GetJobOpportunityByIdAsync(int id)
        {
            return await _jobOpportunityRepository.GetJobOpportunityByIdAsync(id);
        }

        public async Task<JobOpportunity> CreateJobOpportunityAsync(JobOpportunity jobOpportunity)
        {
            ValidateJobOpportunity(jobOpportunity);
            return await _jobOpportunityRepository.AddJobOpportunityAsync(jobOpportunity);
        }

        public async Task UpdateJobOpportunityAsync(JobOpportunity jobOpportunity)
        {
            ValidateJobOpportunity(jobOpportunity);
            await _jobOpportunityRepository.UpdateJobOpportunityAsync(jobOpportunity);
        }

        public async Task DeleteJobOpportunityAsync(int id)
        {
            await _jobOpportunityRepository.DeleteJobOpportunityAsync(id);
        }

        public async Task<List<JobOpportunity>> GetFilteredJobOpportunitiesAsync(
            string searchTerm,
            string company,
            string location,
            string sortBy,
            string sortOrder
        )
        {
            return await _jobOpportunityRepository.GetFilteredJobOpportunitiesAsync(
                searchTerm,
                company,
                location,
                sortBy,
                sortOrder
            );
        }

        public async Task<List<string>> GetAllCompanyNamesAsync()
        {
            return await _jobOpportunityRepository.GetAllCompanyNamesAsync();
        }

        public async Task<List<(int Value, string Text)>> GetAllCompaniesAsync()
        {
            return await _jobOpportunityRepository.GetAllCompaniesAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _jobOpportunityRepository.GetCompanyByIdAsync(id);
        }

        private void ValidateJobOpportunity(JobOpportunity jobOpportunity)
        {
            var context = new ValidationContext(jobOpportunity, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(jobOpportunity, context, results, true);
            if (!isValid)
            {
                throw new ArgumentException(results.FirstOrDefault().ErrorMessage);
            }
        }
    }
}
