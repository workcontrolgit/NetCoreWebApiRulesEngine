using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetCoreWebApiRulesEngine.Domain.Entities;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

internal static class ApplicationDbContextHelpers
{
    public static void RulesEngineModelCreating(ModelBuilder modelBuilder)
    {
        // Rules Engine
        modelBuilder.Entity<ScopedParam>()
          .HasKey(k => k.Name);

        modelBuilder.Entity<Workflow>(entity =>
        {
            entity.HasKey(k => k.WorkflowName);
            entity.Ignore(b => b.WorkflowsToInject);
        });

        modelBuilder.Entity<Rule>().HasOne<Rule>().WithMany(r => r.Rules).HasForeignKey("RuleNameFK");

        var serializationOptions = new JsonSerializerOptions(JsonSerializerDefaults.General);

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.HasKey(k => k.RuleName);

            var valueComparer = new ValueComparer<Dictionary<string, object>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c);

            entity.Property(b => b.Properties)
            .HasConversion(
                v => JsonSerializer.Serialize(v, serializationOptions),
                v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, serializationOptions))
                .Metadata
                .SetValueComparer(valueComparer);

            entity.Property(p => p.Actions)
            .HasConversion(
                v => JsonSerializer.Serialize(v, serializationOptions),
               v => JsonSerializer.Deserialize<RuleActions>(v, serializationOptions));

            entity.Ignore(b => b.WorkflowsToInject);
        });
    }

    public static void HumanResourceModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.PositionId, "IX_Employees_PositionId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.EmployeeNumber).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(100);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Prefix).HasMaxLength(100);

            entity.HasOne(d => d.Position).WithMany(p => p.Employees).HasForeignKey(d => d.PositionId);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasIndex(e => e.DepartmentId, "IX_Positions_DepartmentId");

            entity.HasIndex(e => e.SalaryRangeId, "IX_Positions_SalaryRangeId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.PositionDescription)
                .IsRequired()
                .HasMaxLength(1000);
            entity.Property(e => e.PositionNumber)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.PositionTitle)
                .IsRequired()
                .HasMaxLength(250);

            entity.HasOne(d => d.Department).WithMany(p => p.Positions).HasForeignKey(d => d.DepartmentId);

            entity.HasOne(d => d.SalaryRange).WithMany(p => p.Positions).HasForeignKey(d => d.SalaryRangeId);
        });

        modelBuilder.Entity<SalaryRange>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(100);
            entity.Property(e => e.MaxSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinSalary).HasColumnType("decimal(18, 2)");
        });
    }
}