using LinqKit;
using Microsoft.EntityFrameworkCore;
using NetCoreWebApiRulesEngine.Application.Features.Positions.Queries.GetPositions;
using NetCoreWebApiRulesEngine.Application.Interfaces;
using NetCoreWebApiRulesEngine.Application.Interfaces.Repositories;
using NetCoreWebApiRulesEngine.Application.Parameters;
using NetCoreWebApiRulesEngine.Domain.Entities;
using NetCoreWebApiRulesEngine.Infrastructure.Persistence.Contexts;
using NetCoreWebApiRulesEngine.Infrastructure.Persistence.Repository;
using RulesEngine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebApiRulesEngine.Infrastructure.Persistence.Repositories
{

    public class WorkflowRepositoryAsync : GenericRepositoryAsync<Workflow>, IWorkflowRepositoryAsync
    {
        private readonly DbSet<Workflow> _repository;

        public WorkflowRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _repository = dbContext.Set<Workflow>();
        }

        //public async Task<(IEnumerable<Workflow>)> GetWorkflowReponseAsync<Workflow>()
        //{
        //    return await this.GetAllAsync();
        //}
        public async Task<IEnumerable<Workflow>> GetWorkflowReponseAsync()
        {
            return await _repository
                .Include(i => i.Rules).ThenInclude(i => i.Rules)
                .ToListAsync();
        }


    }
}