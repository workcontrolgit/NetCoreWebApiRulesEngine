using RulesEngine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreWebApiRulesEngine.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Workflow entity with asynchronous methods.
    /// </summary>
    /// <param name="positionNumber">Workflow number to check for uniqueness.</param>
    /// <returns>
    /// Task indicating whether the position number is unique.
    /// </returns>
    /// <param name="rowCount">Number of rows to seed.</param>
    /// <returns>
    /// Task indicating the completion of seeding.
    /// </returns>
    /// <param name="requestParameters">Parameters for the query.</param>
    /// <param name="data">Data to be returned.</param>
    /// <param name="recordsCount">Number of records.</param>
    /// <returns>
    /// Task containing the paged response.
    /// </returns>    
    public interface IWorkflowRepositoryAsync : IGenericRepositoryAsync<Workflow>
    {
        Task<IEnumerable<Workflow>> GetWorkflowReponseAsync();

    }
}