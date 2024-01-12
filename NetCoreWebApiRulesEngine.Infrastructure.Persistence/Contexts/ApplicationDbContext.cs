using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using NetCoreWebApiRulesEngine.Application.Interfaces;
using NetCoreWebApiRulesEngine.Domain.Common;
using NetCoreWebApiRulesEngine.Domain.Entities;
using RulesEngine.Models;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreWebApiRulesEngine.Infrastructure.Persistence.Contexts
{
    public partial class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly ILoggerFactory _loggerFactory;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IDateTimeService dateTime,
            ILoggerFactory loggerFactory
            ) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _loggerFactory = loggerFactory;
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<SalaryRange> SalaryRanges { get; set; }

        // Rules Engine
        public DbSet<Workflow> Workflows { get; set; }

        public DbSet<Rule> Rules { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the tables
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
            // Seed position data
            //var _mockData = this.Database.GetService<IMockService>();
            // mock 1000 rows
            //var seedPositions = _mockData.SeedPositions(1000);
            // seed mock data
            //modelBuilder.Entity<Position>().HasData(seedPositions);

            // rules engine
            ApplicationDbContextHelpers.
                        RulesEngineModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }
}