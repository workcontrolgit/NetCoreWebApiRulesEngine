using Microsoft.EntityFrameworkCore;
using NetCoreWebApiRulesEngine.Infrastructure.Persistence.Contexts;
using Newtonsoft.Json;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreWebApiRulesEngine.Infrastructure.Persistence.SeedData
{
    public static class DbInitializer
    {
        public static void RulesInitialize(ApplicationDbContext context)
        {
            // Optionally check if the database is created
            // context.Database.EnsureCreated();

            // Your custom initialization logic here
            // For example, seeding the database
            SeedData(context);
        }

        private static void SeedData(ApplicationDbContext context)
        {
            if (!context.Workflows.Any())
            {
                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "Discount.json", SearchOption.AllDirectories);
                if (files == null || files.Length == 0)
                    throw new Exception("Rules not found.");

                var fileData = File.ReadAllText(files[0]);
                var workflow = JsonConvert.DeserializeObject<List<Workflow>>(fileData);
                context.Workflows.AddRange(workflow);
                context.SaveChanges();
            }
        }
    }
}
