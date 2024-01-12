using NetCoreWebApiRulesEngine.Application.Interfaces;
using NetCoreWebApiRulesEngine.Application.Interfaces.Repositories;
using NetCoreWebApiRulesEngine.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RulesEngine.Extensions;
using System.Dynamic;
using System.Linq;

namespace NetCoreWebApiRulesEngine.Infrastructure.Shared.Services
{
    public class BusinessRuleService : IBusinessRuleService
    {
        private readonly IPositionRepositoryAsync _positionRepository;
        private readonly IWorkflowRepositoryAsync _workflowRepository;

        public BusinessRuleService(IPositionRepositoryAsync positionRepository, IWorkflowRepositoryAsync workflowRepository)
        {
            _positionRepository = positionRepository;
            _workflowRepository = workflowRepository;
        }

        public bool EvaluateSalaryRule(Position position)
        {
            var breResult = DiscountRule();

            // Example of a rule: Salary should not be more than 1 million
            // return position.PositionSalary <= 1000000;
            return false;
        }

        private bool DiscountRule()
        {
            RulesEngine.RulesEngine bre;
            var basicInfo = "{\"name\": \"hello\",\"email\": \"abcy@xyz.com\",\"creditHistory\": \"good\",\"country\": \"canada\",\"loyaltyFactor\": 3,\"totalPurchasesToDate\": 10000}";
            var orderInfo = "{\"totalOrders\": 5,\"recurringItems\": 2}";
            var telemetryInfo = "{\"noOfVisitsPerMonth\": 10,\"percentageOfBuyingToVisit\": 15}";

            var converter = new ExpandoObjectConverter();

            dynamic input1 = JsonConvert.DeserializeObject<ExpandoObject>(basicInfo, converter);
            dynamic input2 = JsonConvert.DeserializeObject<ExpandoObject>(orderInfo, converter);
            dynamic input3 = JsonConvert.DeserializeObject<ExpandoObject>(telemetryInfo, converter);

            var inputs = new dynamic[]
                {
                    input1,
                    input2,
                    input3
                };

            var wfr = _workflowRepository.GetWorkflowReponseAsync().Result.ToArray();

            bre = new RulesEngine.RulesEngine(wfr, null);
            string discountOffered = "No discount offered.";

            var resultList = bre.ExecuteAllRulesAsync("Discount", inputs).Result;

            bool isValid = false;

            resultList.OnSuccess((eventName) =>
            {
                discountOffered = $"Discount offered is {eventName} % over MRP.";
                isValid = true;
            });

            resultList.OnFail(() =>
            {
                discountOffered = "The user is not eligible for any discount.";
                isValid = true;
            });
            return isValid;
        }
    }
}