using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreWebApiRulesEngine.Application;
using NetCoreWebApiRulesEngine.Infrastructure.Persistence;
using NetCoreWebApiRulesEngine.Infrastructure.Persistence.Contexts;
using NetCoreWebApiRulesEngine.Infrastructure.Persistence.SeedData;
using NetCoreWebApiRulesEngine.Infrastructure.Shared;
using NetCoreWebApiRulesEngine.Infrastructure.Shared.Services;
using NetCoreWebApiRulesEngine.WebApi.Extensions;
using Serilog;
using System;

try
{
    var builder = WebApplication.CreateBuilder(args);
    // load up serilog configuraton
    Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);

    Log.Information("Application startup services registration");

    builder.Services.AddApplicationLayer();
    builder.Services.AddPersistenceInfrastructure(builder.Configuration);
    builder.Services.AddSharedInfrastructure(builder.Configuration);
    builder.Services.AddSwaggerExtension();
    builder.Services.AddControllersExtension();
    // CORS
    builder.Services.AddCorsExtension();
    builder.Services.AddHealthChecks();
    //API Security
    builder.Services.AddJWTAuthentication(builder.Configuration);
    builder.Services.AddAuthorizationPolicies(builder.Configuration);
    // API version
    builder.Services.AddApiVersioningExtension();
    // API explorer
    builder.Services.AddMvcCore()
        .AddApiExplorer();
    // API explorer version
    builder.Services.AddVersionedApiExplorerExtension();
    var app = builder.Build();

    Log.Information("Application startup middleware registration");

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        // for quick database (usually for prototype)
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // use context
            // Generate seed data with Bogus
            var databaseSeeder = new DatabaseSeeder(1000);
            if (dbContext.Database.EnsureCreated())
            {
                DbInitializer.RulesInitialize(dbContext);
                dbContext.BulkInsert(databaseSeeder.Departments);
                dbContext.BulkInsert(databaseSeeder.SalaryRanges);
                dbContext.BulkInsert(databaseSeeder.Positions);
                dbContext.BulkInsert(databaseSeeder.Employees);
            }
        }
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Add this line; you'll need `using Serilog;` up the top, too
    app.UseSerilogRequestLogging();
    app.UseHttpsRedirection();
    app.UseRouting();
    //Enable CORS
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSwaggerExtension();
    app.UseErrorHandlingMiddleware();
    app.UseHealthChecks("/health");
    app.MapControllers();

    Log.Information("Application Starting");

    app.Run();
}
catch (Exception ex)
{
    Log.Warning(ex, "An error occurred starting the application");
}
finally
{
    Log.CloseAndFlush();
}