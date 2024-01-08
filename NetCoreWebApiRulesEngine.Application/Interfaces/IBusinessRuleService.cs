using NetCoreWebApiRulesEngine.Domain.Entities;

namespace NetCoreWebApiRulesEngine.Application.Interfaces
{
    public interface IBusinessRuleService
    {
        bool EvaluateSalaryRule(Position position);
    }
}