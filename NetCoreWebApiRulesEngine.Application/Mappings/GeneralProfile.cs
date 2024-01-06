using AutoMapper;
using NetCoreWebApiRulesEngine.Application.Features.Employees.Queries.GetEmployees;
using NetCoreWebApiRulesEngine.Application.Features.Positions.Commands.CreatePosition;
using NetCoreWebApiRulesEngine.Application.Features.Positions.Queries.GetPositions;
using NetCoreWebApiRulesEngine.Domain.Entities;

namespace NetCoreWebApiRulesEngine.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Position, GetPositionsViewModel>().ReverseMap();
            CreateMap<Employee, GetEmployeesViewModel>().ReverseMap();
            CreateMap<CreatePositionCommand, Position>();
        }
    }
}