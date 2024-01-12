using NetCoreWebApiRulesEngine.Domain.Enums;
using System;

namespace NetCoreWebApiRulesEngine.Application.Features.Employees.Queries.GetEmployees
{
    public class GetEmployeesViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public string EmployeeNumber { get; set; }
        public string Suffix { get; set; }
        public string Phone { get; set; }
    }
}