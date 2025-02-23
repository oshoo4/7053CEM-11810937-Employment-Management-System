using EmploymentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploymentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<JobOpportunity> JobOpportunities { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Company>()
                .HasData(
                    new Company
                    {
                        Id = 1,
                        Name = "Tech Solutions Inc.",
                        Address = "London UK",
                    },
                    new Company
                    {
                        Id = 2,
                        Name = "Data Insights Ltd.",
                        Address = "New York USA",
                    },
                    new Company
                    {
                        Id = 3,
                        Name = "Vision Bionics Ltd.",
                        Address = "Manchester UK",
                    }
                );

            modelBuilder
                .Entity<JobOpportunity>()
                .HasData(
                    new JobOpportunity
                    {
                        Id = 1,
                        Title = "Software Engineer",
                        Description = "Develop and maintain software applications.",
                        Company = "Tech Solutions Inc.",
                        Location = "London",
                        Salary = 50000,
                        PostedDate = new DateOnly(2023, 10, 26),
                        ClosingDate = new DateOnly(2023, 11, 25),
                        IsActive = true,
                        CompanyId = 1,
                    },
                    new JobOpportunity
                    {
                        Id = 2,
                        Title = "Data Analyst",
                        Description = "Analyze data and provide insights.",
                        Company = "Data Insights Ltd.",
                        Location = "New York",
                        Salary = 60000,
                        PostedDate = new DateOnly(2023, 10, 20),
                        ClosingDate = new DateOnly(2023, 12, 1),
                        IsActive = false,
                        CompanyId = 2,
                    },
                    new JobOpportunity
                    {
                        Id = 3,
                        Title = "Senior Software Engineer",
                        Description =
                            "Develop and maintain software applications. Leading junior developers.",
                        Company = "Tech Solutions Inc.",
                        Location = "London",
                        Salary = 80000,
                        PostedDate = new DateOnly(2023, 10, 26),
                        ClosingDate = new DateOnly(2023, 11, 25),
                        IsActive = true,
                        CompanyId = 1,
                    },
                    new JobOpportunity
                    {
                        Id = 4,
                        Title = "Junior Data Analyst",
                        Description = "Analyze data and provide insights.",
                        Company = "Data Insights Ltd.",
                        Location = "New York",
                        Salary = 40000,
                        PostedDate = new DateOnly(2023, 10, 20),
                        ClosingDate = new DateOnly(2023, 12, 1),
                        IsActive = true,
                        CompanyId = 2,
                    },
                    new JobOpportunity
                    {
                        Id = 5,
                        Title = "Software Engineer",
                        Description = "Develop and maintain software applications.",
                        Company = "Vision Bionics Ltd.",
                        Location = "Manchester",
                        Salary = 55000,
                        PostedDate = new DateOnly(2023, 10, 26),
                        ClosingDate = new DateOnly(2023, 11, 30),
                        IsActive = true,
                        CompanyId = 3,
                    }
                );
        }
    }
}
