﻿using NetCoreWebApiRulesEngine.Domain.Common;
using System.Collections.Generic;

namespace NetCoreWebApiRulesEngine.Domain.Entities
{
    public class SalaryRange : AuditableBaseEntity
    {
        // Minimum salary value
        public decimal MinSalary { get; set; }

        // Maximum salary value
        public decimal MaxSalary { get; set; }

        // Optional: Description or additional details
        public string Description { get; set; }

        // Navigation property back to Position if needed
        public virtual ICollection<Position> Positions { get; set; }

        // Additional properties or methods can be added here
    }
}