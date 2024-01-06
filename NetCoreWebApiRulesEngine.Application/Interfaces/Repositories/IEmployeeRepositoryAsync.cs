using NetCoreWebApiRulesEngine.Application.Features.Employees.Queries.GetEmployees;
using NetCoreWebApiRulesEngine.Application.Parameters;
using NetCoreWebApiRulesEngine.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreWebApiRulesEngine.Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface for retrieving paged employee response asynchronously.
    /// </summary>
    /// <param name="requestParameters">The request parameters.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    public interface IEmployeeRepositoryAsync : IGenericRepositoryAsync<Employee>
    {
        Task<(IEnumerable<Entity> data, RecordsCount recordsCount)> GetPagedEmployeeResponseAsync(GetEmployeesQuery requestParameters);
    }
}