using Bogus;
using NetCoreWebApiRulesEngine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreWebApiRulesEngine.Infrastructure.Shared.Services
{
    public class DatabaseSeeder
    {
        public IReadOnlyCollection<Department> Departments { get; }
        public IReadOnlyCollection<Employee> Employees { get; }
        public IReadOnlyCollection<Position> Positions { get; }

        public IReadOnlyCollection<SalaryRange> SalaryRanges { get; }

        public DatabaseSeeder(int rowCount = 100, int seedValue = 1969)
        {
            Departments = GenerateDepartments(rowCount, seedValue);
            SalaryRanges = GenerateSalaryRanges(rowCount, seedValue);
            Positions = GeneratePositions(rowCount, seedValue, Departments, SalaryRanges);
            Employees = GenerateEmployees(rowCount, seedValue, Positions);
        }

        private static IReadOnlyCollection<SalaryRange> GenerateSalaryRanges(int rowCount, int seedValue)
        {
            var faker = new Faker<SalaryRange>()
                  .UseSeed(seedValue) // Use any number
                  .RuleFor(r => r.Id, f => Guid.NewGuid())
                  .RuleFor(r => r.Description, f => f.Name.JobDescriptor())
                  .RuleFor(r => r.MinSalary, f => f.Finance.Amount()) // TODO Set min range
                  .RuleFor(r => r.MaxSalary, f => f.Finance.Amount()) // TODO Set max range
                  .RuleFor(r => r.Created, f => f.Date.Recent())
                  .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                  ;

            return faker.Generate(rowCount);
        }

        private static IReadOnlyCollection<Department> GenerateDepartments(int rowCount, int seedValue)
        {
            var random = new Random();
            var faker = new Faker<Department>()
                .UseSeed(seedValue) // Use any number
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.Name, f => f.Commerce.ProductName()) // TODO Find department name
                .RuleFor(r => r.Created, f => f.Date.Past(random.Next(1, 360)))
                .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(rowCount);
        }

        private static IReadOnlyCollection<Employee> GenerateEmployees(int rowCount, int seedValue, IEnumerable<Position> positions)
        {
            var faker = new Faker<Employee>()
                .UseSeed(seedValue) // Use any number
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.EmployeeNumber, f => f.Commerce.Ean13())
                .RuleFor(r => r.Salary, f => f.Finance.Amount())
                .RuleFor(r => r.Suffix, f => f.Name.Suffix())
                .RuleFor(r => r.FirstName, f => f.Name.FirstName())
                .RuleFor(r => r.MiddleName, f => f.Name.FirstName())
                .RuleFor(r => r.LastName, f => f.Name.LastName())
                .RuleFor(r => r.Birthday, f => f.Date.Past(18))
                .RuleFor(r => r.Email, (f, p) => f.Internet.Email(p.FirstName, p.LastName))
                .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber("(###)-###-####"))
                .RuleFor(r => r.PositionId, f => f.PickRandom(positions).Id)
                .RuleFor(r => r.Created, f => f.Date.Recent())
                .RuleFor(r => r.CreatedBy, f => f.Internet.UserName())
                ;

            return faker.Generate(rowCount);
        }

        private static IReadOnlyCollection<Position> GeneratePositions(
            int rowCount, int seedValue,
            IEnumerable<Department> departments,
            IEnumerable<SalaryRange> salaryRanges)
        {
            // Now we set up the faker for our join table.
            // We do this by grabbing a random product and category that were generated.
            var faker = new Faker<Position>()
                .UseSeed(seedValue) // Use any number
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(o => o.PositionTitle, f => f.Name.JobTitle())
                .RuleFor(o => o.PositionNumber, f => f.Commerce.Ean13())
                .RuleFor(o => o.PositionDescription, f => f.Name.JobDescriptor())
                .RuleFor(r => r.DepartmentId, f => f.PickRandom(departments).Id)
                .RuleFor(r => r.SalaryRangeId, f => f.PickRandom(salaryRanges).Id);

            return faker.Generate(rowCount)
                .GroupBy(r => new { r.DepartmentId, r.SalaryRangeId })
                .Select(r => r.First())
                .ToList();
        }
    }
}