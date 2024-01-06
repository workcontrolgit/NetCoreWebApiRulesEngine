﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetCoreWebApiRulesEngine.Application.Behaviours;
using NetCoreWebApiRulesEngine.Application.Helpers;
using NetCoreWebApiRulesEngine.Application.Interfaces;
using NetCoreWebApiRulesEngine.Domain.Entities;
using System.Reflection;

namespace NetCoreWebApiRulesEngine.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IDataShapeHelper<Position>, DataShapeHelper<Position>>();
            services.AddScoped<IDataShapeHelper<Employee>, DataShapeHelper<Employee>>();
            services.AddScoped<IModelHelper, ModelHelper>();
        }
    }
}