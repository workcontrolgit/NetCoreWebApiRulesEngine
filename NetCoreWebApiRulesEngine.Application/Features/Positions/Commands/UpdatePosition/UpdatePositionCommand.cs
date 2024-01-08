using MediatR;
using NetCoreWebApiRulesEngine.Application.Exceptions;
using NetCoreWebApiRulesEngine.Application.Interfaces;
using NetCoreWebApiRulesEngine.Application.Interfaces.Repositories;
using NetCoreWebApiRulesEngine.Application.Wrappers;
using RulesEngine.Extensions;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreWebApiRulesEngine.Application.Features.Positions.Commands.UpdatePosition
{
    public class UpdatePositionCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
        public string PositionTitle { get; set; }
        public string PositionNumber { get; set; }
        public string PositionDescription { get; set; }
        public decimal PositionSalary { get; set; }

        public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, Response<Guid>>
        {
            private readonly IPositionRepositoryAsync _positionRepository;
            private readonly IBusinessRuleService _businessRuleService;

            public UpdatePositionCommandHandler(IPositionRepositoryAsync positionRepository, IBusinessRuleService businessRuleService)
            {
                _positionRepository = positionRepository;
                _businessRuleService = businessRuleService;
            }

            public async Task<Response<Guid>> Handle(UpdatePositionCommand command, CancellationToken cancellationToken)
            {
                var position = await _positionRepository.GetByIdAsync(command.Id);

                if (position == null)
                {
                    throw new ApiException($"Position Not Found.");
                }
                else
                {
                    position.PositionTitle = command.PositionTitle;
                    position.PositionSalary = command.PositionSalary;
                    position.PositionDescription = command.PositionDescription;

                    // example local rules
                    RulesEngineExample(position);

                    // call RulesEngine service

                    if (_businessRuleService.EvaluateSalaryRule(position))
                    {
                        await _positionRepository.UpdateAsync(position);
                    }
                    return new Response<Guid>(position.Id);
                }

                static void RulesEngineExample(Domain.Entities.Position position)
                {
                    // rules enginer prototype
                    var discountWorkflows = new List<Workflow>();
                    Workflow discountWorkFlow = new Workflow();

                    discountWorkFlow.WorkflowName = "Sunday Discounts";
                    discountWorkFlow.Rules = UpdatePosition.DiscountRule.GetSundayDiscountRules();
                    discountWorkflows.Add(discountWorkFlow);

                    var bre = new RulesEngine.RulesEngine(discountWorkflows.ToArray());
                    var rulesResult = bre.ExecuteAllRulesAsync(discountWorkFlow.WorkflowName, position).Result;
                    rulesResult.OnSuccess((eventName) =>
                    {
                        if (eventName == "Discount given on a Sunday")
                        {
                            var discount = (position.PositionSalary / 100) * 10;
                            position.PositionSalary = position.PositionSalary - discount;
                        }
                    });
                }
            }
        }
    }
}