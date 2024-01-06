using NetCoreWebApiRulesEngine.Application.Interfaces;
using System;

namespace NetCoreWebApiRulesEngine.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}