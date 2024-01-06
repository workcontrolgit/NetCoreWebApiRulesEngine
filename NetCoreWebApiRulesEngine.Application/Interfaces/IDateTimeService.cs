using System;

namespace NetCoreWebApiRulesEngine.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}