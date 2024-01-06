using RulesEngine.Models;
using System.Collections.Generic;

namespace NetCoreWebApiRulesEngine.Application.Features.Positions.Commands.UpdatePosition
{
    public class DiscountRule
    {
        public static List<Rule> GetSundayDiscountRules()
        {
            var rules = new List<Rule>();

            Rule sundayDiscountRule = new Rule
            {
                RuleName = "Discount Rule",
                SuccessEvent = "Discount given on a Sunday",
                ErrorMessage = "Discounts are only available on Sundays",
                Expression = "Created.DayOfWeek == DayOfWeek.Saturday",
                RuleExpressionType = RuleExpressionType.LambdaExpression
            };

            rules.Add(sundayDiscountRule);
            return rules;
        }
    }
}
