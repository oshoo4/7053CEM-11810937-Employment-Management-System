using EmploymentManagementSystem.Models;
using EmploymentManagementSystem.Repositories;
using EmploymentManagementSystem.Services;
using Moq;
using Xunit;

namespace EmploymentManagementSystem.Tests
{
    public class JobOpportunityServiceTests
    {
        [Fact]
        public async Task GetAllJobOpportunitiesAsync_ReturnsAllJobOpportunities()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var expectedJobs = new List<JobOpportunity>
            {
                new JobOpportunity
                {
                    Id = 1,
                    Title = "Job 1",
                    CompanyId = 1,
                    CompanyObject = new Company { Id = 1, Name = "Company 1" },
                },
                new JobOpportunity
                {
                    Id = 2,
                    Title = "Job 2",
                    CompanyId = 2,
                    CompanyObject = new Company { Id = 2, Name = "Company 2" },
                },
            };
            mockRepo.Setup(repo => repo.GetAllJobOpportunitiesAsync()).ReturnsAsync(expectedJobs);
            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.GetAllJobOpportunitiesAsync();

            Assert.NotNull(result);
            Assert.Equal(expectedJobs.Count, result.Count);
            Assert.Equal(expectedJobs[0].Title, result[0].Title);
            Assert.Equal(expectedJobs[1].Title, result[1].Title);

            Assert.NotNull(result[0].CompanyObject);
            Assert.Equal(expectedJobs[0].CompanyObject.Name, result[0].CompanyObject.Name);
            Assert.NotNull(result[1].CompanyObject);
            Assert.Equal(expectedJobs[1].CompanyObject.Name, result[1].CompanyObject.Name);
            mockRepo.Verify(repo => repo.GetAllJobOpportunitiesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetJobOpportunityByIdAsync_ExistingId_ReturnsJobOpportunity()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var expectedJob = new JobOpportunity
            {
                Id = 1,
                Title = "Job 1",
                CompanyId = 1,
                CompanyObject = new Company { Id = 1, Name = "Company 1" },
            };
            mockRepo.Setup(repo => repo.GetJobOpportunityByIdAsync(1)).ReturnsAsync(expectedJob);
            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.GetJobOpportunityByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(expectedJob.Id, result.Id);
            Assert.Equal(expectedJob.Title, result.Title);

            Assert.NotNull(result.CompanyObject);
            Assert.Equal(expectedJob.CompanyObject.Name, result.CompanyObject.Name);
            mockRepo.Verify(repo => repo.GetJobOpportunityByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetJobOpportunityByIdAsync_NonExistingId_ReturnsNull()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            mockRepo
                .Setup(repo => repo.GetJobOpportunityByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((JobOpportunity)null);
            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.GetJobOpportunityByIdAsync(99);

            Assert.Null(result);
            mockRepo.Verify(repo => repo.GetJobOpportunityByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task CreateJobOpportunityAsync_ValidJobOpportunity_ReturnsCreatedJobOpportunity()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var newJobOpportunity = new JobOpportunity
            {
                Title = "New Job",
                Company = "New Company",
                Description = "A valid description",
                Location = "Some Location",
                Salary = 50000,
                ClosingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                PostedDate = DateOnly.FromDateTime(DateTime.Now),
                IsActive = true,
                CompanyId = 1,
                CompanyObject = new Company { Id = 1, Name = "Company 1" },
            };

            mockRepo
                .Setup(repo => repo.AddJobOpportunityAsync(It.IsAny<JobOpportunity>()))
                .ReturnsAsync(
                    (JobOpportunity job) =>
                    {
                        job.Id = 3;
                        return job;
                    }
                );

            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.CreateJobOpportunityAsync(newJobOpportunity);

            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
            Assert.Equal("New Job", result.Title);
            mockRepo.Verify(
                repo => repo.AddJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateJobOpportunityAsync_ValidJobOpportunity_CallsRepositoryUpdate()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var jobToUpdate = new JobOpportunity
            {
                Id = 1,
                Title = "Updated Job",
                Company = "Test Company", // Set the Company string directly
                Description = "A valid description",
                Location = "Some Location",
                Salary = 60000,
                ClosingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30)),
                PostedDate = DateOnly.FromDateTime(DateTime.Now),
                IsActive = true,
                CompanyId = 1,
            };

            mockRepo
                .Setup(repo => repo.UpdateJobOpportunityAsync(It.IsAny<JobOpportunity>()))
                .Returns(Task.CompletedTask);

            var service = new JobOpportunityService(mockRepo.Object);

            await service.UpdateJobOpportunityAsync(jobToUpdate);

            mockRepo.Verify(
                repo => repo.UpdateJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteJobOpportunityAsync_ExistingId_CallsRepositoryDelete()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            mockRepo.Setup(repo => repo.DeleteJobOpportunityAsync(1)).Returns(Task.CompletedTask);
            var service = new JobOpportunityService(mockRepo.Object);

            await service.DeleteJobOpportunityAsync(1);

            mockRepo.Verify(repo => repo.DeleteJobOpportunityAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetFilteredJobOpportunitiesAsync_KeywordSearch_ReturnsFilteredResults()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var expectedJobs = new List<JobOpportunity>
            {
                new JobOpportunity
                {
                    Id = 1,
                    Title = "Software Engineer",
                    CompanyObject = new Company { Id = 1, Name = "Company 1" },
                },
                new JobOpportunity
                {
                    Id = 3,
                    Title = "Senior Software Engineer",
                    CompanyObject = new Company { Id = 1, Name = "Company 1" },
                },
            };
            mockRepo
                .Setup(repo =>
                    repo.GetFilteredJobOpportunitiesAsync("Software", null, null, null, null)
                )
                .ReturnsAsync(expectedJobs);

            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.GetFilteredJobOpportunitiesAsync(
                "Software",
                null,
                null,
                null,
                null
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, j => j.Title.Contains("Software Engineer"));
            Assert.Contains(result, j => j.Title.Contains("Senior Software Engineer"));
            mockRepo.Verify(
                repo => repo.GetFilteredJobOpportunitiesAsync("Software", null, null, null, null),
                Times.Once
            );
        }

        [Fact]
        public async Task GetFilteredJobOpportunitiesAsync_CompanyFilter_ReturnsFilteredResults()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var expectedJobs = new List<JobOpportunity>
            {
                new JobOpportunity
                {
                    Id = 1,
                    Title = "Software Engineer",
                    Company = "Tech Solutions Inc.",
                    CompanyObject = new Company { Id = 1, Name = "Tech Solutions Inc." },
                },
                new JobOpportunity
                {
                    Id = 3,
                    Title = "Senior Software Engineer",
                    Company = "Tech Solutions Inc.",
                    CompanyObject = new Company { Id = 1, Name = "Tech Solutions Inc." },
                },
            };
            mockRepo
                .Setup(repo =>
                    repo.GetFilteredJobOpportunitiesAsync(
                        null,
                        "Tech Solutions Inc.",
                        null,
                        null,
                        null
                    )
                )
                .ReturnsAsync(expectedJobs);

            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.GetFilteredJobOpportunitiesAsync(
                null,
                "Tech Solutions Inc.",
                null,
                null,
                null
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, j => Assert.Equal("Tech Solutions Inc.", j.Company));

            mockRepo.Verify(
                repo =>
                    repo.GetFilteredJobOpportunitiesAsync(
                        null,
                        "Tech Solutions Inc.",
                        null,
                        null,
                        null
                    ),
                Times.Once
            );
        }

        [Fact]
        public async Task GetFilteredJobOpportunitiesAsync_LocationFilter_ReturnsFilteredResults()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var expectedJobs = new List<JobOpportunity>
            {
                new JobOpportunity
                {
                    Id = 1,
                    Title = "Software Engineer",
                    Location = "London",
                    CompanyObject = new Company { Id = 1, Name = "Company 1" },
                },
            };
            mockRepo
                .Setup(repo =>
                    repo.GetFilteredJobOpportunitiesAsync(null, null, "London", null, null)
                )
                .ReturnsAsync(expectedJobs);

            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.GetFilteredJobOpportunitiesAsync(
                null,
                null,
                "London",
                null,
                null
            );

            Assert.Single(result);
            Assert.Equal("London", result[0].Location);
            mockRepo.Verify(
                repo => repo.GetFilteredJobOpportunitiesAsync(null, null, "London", null, null),
                Times.Once
            );
        }

        [Fact]
        public async Task GetFilteredJobOpportunitiesAsync_SortByTitleAsc_ReturnsSortedResults()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var expectedJobs = new List<JobOpportunity>
            {
                new JobOpportunity
                {
                    Id = 2,
                    Title = "Data Analyst",
                    CompanyObject = new Company { Id = 1, Name = "Company 1" },
                },
                new JobOpportunity
                {
                    Id = 1,
                    Title = "Software Engineer",
                    CompanyObject = new Company { Id = 1, Name = "Company 1" },
                },
            };
            mockRepo
                .Setup(repo =>
                    repo.GetFilteredJobOpportunitiesAsync(null, null, null, "title", "asc")
                )
                .ReturnsAsync(expectedJobs);

            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.GetFilteredJobOpportunitiesAsync(
                null,
                null,
                null,
                "title",
                "asc"
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Data Analyst", result[0].Title);
            Assert.Equal("Software Engineer", result[1].Title);
            mockRepo.Verify(
                repo => repo.GetFilteredJobOpportunitiesAsync(null, null, null, "title", "asc"),
                Times.Once
            );
        }

        [Fact]
        public async Task GetFilteredJobOpportunitiesAsync_SortByClosingDateDesc_ReturnsSortedResults()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var expectedJobs = new List<JobOpportunity>
            {
                new JobOpportunity
                {
                    Id = 2,
                    Title = "Data Analyst",
                    ClosingDate = new DateOnly(2025, 1, 1),
                    CompanyObject = new Company { Id = 1, Name = "Company 1" },
                },
                new JobOpportunity
                {
                    Id = 1,
                    Title = "Software Engineer",
                    ClosingDate = new DateOnly(2024, 12, 1),
                    CompanyObject = new Company { Id = 1, Name = "Company 1" },
                },
            };
            mockRepo
                .Setup(repo =>
                    repo.GetFilteredJobOpportunitiesAsync(null, null, null, "closingDate", "desc")
                )
                .ReturnsAsync(expectedJobs);

            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.GetFilteredJobOpportunitiesAsync(
                null,
                null,
                null,
                "closingDate",
                "desc"
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Data Analyst", result[0].Title);
            Assert.Equal("Software Engineer", result[1].Title);
            mockRepo.Verify(
                repo =>
                    repo.GetFilteredJobOpportunitiesAsync(null, null, null, "closingDate", "desc"),
                Times.Once
            );
        }

        [Fact]
        public async Task GetFilteredJobOpportunitiesAsync_CombinedFilterAndSort_ReturnsFilteredAndSortedResults()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var expectedJobs = new List<JobOpportunity>
            {
                new JobOpportunity
                {
                    Id = 3,
                    Title = "Senior Software Engineer",
                    Company = "Tech Solutions Inc.",
                    Location = "London",
                    Salary = 80000,
                    CompanyObject = new Company { Id = 1, Name = "Tech Solutions Inc." },
                },
                new JobOpportunity
                {
                    Id = 1,
                    Title = "Software Engineer",
                    Company = "Tech Solutions Inc.",
                    Location = "London",
                    Salary = 50000,
                    CompanyObject = new Company { Id = 1, Name = "Tech Solutions Inc." },
                },
            };

            mockRepo
                .Setup(repo =>
                    repo.GetFilteredJobOpportunitiesAsync(
                        "Software",
                        "Tech Solutions Inc.",
                        "London",
                        "salary",
                        "desc"
                    )
                )
                .ReturnsAsync(expectedJobs);

            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.GetFilteredJobOpportunitiesAsync(
                "Software",
                "Tech Solutions Inc.",
                "London",
                "salary",
                "desc"
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Senior Software Engineer", result[0].Title);
            Assert.Equal(80000, result[0].Salary);
            Assert.Equal("Software Engineer", result[1].Title);
            mockRepo.Verify(
                repo =>
                    repo.GetFilteredJobOpportunitiesAsync(
                        "Software",
                        "Tech Solutions Inc.",
                        "London",
                        "salary",
                        "desc"
                    ),
                Times.Once
            );
        }

        [Fact]
        public async Task GetFilteredJobOpportunitiesAsync_SortByIsActiveAsc_ReturnsSortedResults()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var expectedJobs = new List<JobOpportunity>
            {
                new JobOpportunity
                {
                    Id = 2,
                    Title = "Data Analyst",
                    IsActive = false,
                    CompanyObject = new Company { Id = 1, Name = "Company 1" },
                },
                new JobOpportunity
                {
                    Id = 1,
                    Title = "Software Engineer",
                    IsActive = true,
                    CompanyObject = new Company { Id = 1, Name = "Company 1" },
                },
            };
            mockRepo
                .Setup(repo =>
                    repo.GetFilteredJobOpportunitiesAsync(null, null, null, "isActive", "asc")
                )
                .ReturnsAsync(expectedJobs);

            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.GetFilteredJobOpportunitiesAsync(
                null,
                null,
                null,
                "isActive",
                "asc"
            );

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.False(result[0].IsActive);
            Assert.True(result[1].IsActive);

            mockRepo.Verify(
                repo => repo.GetFilteredJobOpportunitiesAsync(null, null, null, "isActive", "asc"),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateJobOpportunityAsync_InvalidTitle_ThrowsException()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var service = new JobOpportunityService(mockRepo.Object);
            var invalidJob = new JobOpportunity
            {
                Title = "",
                CompanyId = 1,
                Description = "Test Description",
                Salary = 50000,
                Location = "Test Location",
                ClosingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                PostedDate = DateOnly.FromDateTime(DateTime.Now),
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateJobOpportunityAsync(invalidJob)
            );
            mockRepo.Verify(
                repo => repo.AddJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Never
            );
        }

        [Fact]
        public async Task CreateJobOpportunityAsync_TitleTooLong_ThrowsException()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var service = new JobOpportunityService(mockRepo.Object);
            var longTitle = new string('A', 101);
            var invalidJob = new JobOpportunity
            {
                Title = longTitle,
                CompanyId = 1,
                Description = "Test Description",
                Salary = 50000,
                Location = "Test Location",
                ClosingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                PostedDate = DateOnly.FromDateTime(DateTime.Now),
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateJobOpportunityAsync(invalidJob)
            );
            mockRepo.Verify(
                repo => repo.AddJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Never
            );
        }

        [Fact]
        public async Task CreateJobOpportunityAsync_NegativeSalary_ThrowsException()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var service = new JobOpportunityService(mockRepo.Object);
            var invalidJob = new JobOpportunity
            {
                Title = "Test Job",
                CompanyId = 1,
                Description = "Test Description",
                Salary = -100,
                Location = "Test Location",
                ClosingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                PostedDate = DateOnly.FromDateTime(DateTime.Now),
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateJobOpportunityAsync(invalidJob)
            );
            mockRepo.Verify(
                repo => repo.AddJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Never
            );
        }

        [Fact]
        public async Task UpdateJobOpportunityAsync_InvalidTitle_ThrowsException()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var service = new JobOpportunityService(mockRepo.Object);
            var invalidJob = new JobOpportunity
            {
                Id = 1,
                Title = "",
                CompanyId = 1,
                Description = "Test Description",
                Salary = 50000,
                Location = "Test Location",
                ClosingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                PostedDate = DateOnly.FromDateTime(DateTime.Now),
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.UpdateJobOpportunityAsync(invalidJob)
            );
            mockRepo.Verify(
                repo => repo.UpdateJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Never
            );
        }

        [Fact]
        public async Task CreateJobOpportunityAsync_ClosingDateBeforePostedDate_ThrowsException()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var service = new JobOpportunityService(mockRepo.Object);
            var invalidJob = new JobOpportunity
            {
                Title = "Test Job",
                CompanyId = 1,
                Description = "Test Description",
                Location = "Test Location",
                Salary = 50000,
                PostedDate = new DateOnly(2025, 1, 15),
                ClosingDate = new DateOnly(2025, 1, 10),
                IsActive = true,
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateJobOpportunityAsync(invalidJob)
            );
            mockRepo.Verify(
                repo => repo.AddJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Never()
            );
        }

        [Fact]
        public async Task CreateJobOpportunityAsync_ClosingDateEqualsPostedDate_ThrowsException()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var service = new JobOpportunityService(mockRepo.Object);
            var invalidJob = new JobOpportunity
            {
                Title = "Test Job",
                CompanyId = 1,
                Description = "Test Description",
                Location = "Test Location",
                Salary = 50000,
                PostedDate = new DateOnly(2025, 1, 15),
                ClosingDate = new DateOnly(2025, 1, 15),
                IsActive = true,
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateJobOpportunityAsync(invalidJob)
            );
            mockRepo.Verify(
                repo => repo.AddJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Never()
            );
        }

        [Fact]
        public async Task CreateJobOpportunityAsync_ClosingDateAfterPostedDate_ReturnsCreatedJobOpportunity()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();

            var validJob = new JobOpportunity
            {
                Id = 1,
                Title = "Test Job",
                CompanyId = 1,
                Company = "Test Company",
                Description = "Test Description",
                Location = "Test Location",
                Salary = 50000,
                PostedDate = new DateOnly(2024, 1, 15),
                ClosingDate = new DateOnly(2024, 1, 16),
                IsActive = true,
            };

            mockRepo
                .Setup(repo => repo.AddJobOpportunityAsync(It.IsAny<JobOpportunity>()))
                .ReturnsAsync(
                    (JobOpportunity job) =>
                    {
                        job.Id = 1;
                        return job;
                    }
                );

            var service = new JobOpportunityService(mockRepo.Object);

            var result = await service.CreateJobOpportunityAsync(validJob);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            mockRepo.Verify(
                repo => repo.AddJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Once
            );
            mockRepo.Verify(repo => repo.GetCompanyByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateJobOpportunityAsync_ClosingDateBeforePostedDate_ThrowsException()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var service = new JobOpportunityService(mockRepo.Object);
            var invalidJob = new JobOpportunity
            {
                Id = 1,
                Title = "Test Job",
                CompanyId = 1,
                Description = "Test Description",
                Location = "Test Location",
                Salary = 50000,
                PostedDate = new DateOnly(2025, 1, 15),
                ClosingDate = new DateOnly(2025, 1, 10),
                IsActive = true,
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.UpdateJobOpportunityAsync(invalidJob)
            );
            mockRepo.Verify(
                repo => repo.UpdateJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Never
            );
        }

        [Fact]
        public async Task UpdateJobOpportunityAsync_ClosingDateEqualsPostedDate_ThrowsException()
        {
            var mockRepo = new Mock<IJobOpportunityRepository>();
            var service = new JobOpportunityService(mockRepo.Object);
            var invalidJob = new JobOpportunity
            {
                Id = 1,
                Title = "Test Job",
                CompanyId = 1,
                Description = "Test Description",
                Location = "Test Location",
                Salary = 50000,
                PostedDate = new DateOnly(2025, 1, 15),
                ClosingDate = new DateOnly(2025, 1, 15),
                IsActive = true,
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => service.UpdateJobOpportunityAsync(invalidJob)
            );
            mockRepo.Verify(
                repo => repo.UpdateJobOpportunityAsync(It.IsAny<JobOpportunity>()),
                Times.Never
            );
        }
    }
}
