using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.MRSeeders.AuthSeeder
{
    public class SubscriptionPlanSeeder : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            builder.HasData(
                new SubscriptionPlan { Id = 1, PlanName = "Micro Lab", Price = 1200, MonthlySampleLimit = 50 },
                new SubscriptionPlan { Id = 2, PlanName = "Small Lab", Price = 2500, MonthlySampleLimit = 150 },
                new SubscriptionPlan { Id = 3, PlanName = "Growing Lab", Price = 4200, MonthlySampleLimit = 500 },
                new SubscriptionPlan { Id = 4, PlanName = "Enterprise", Price = 12000, MonthlySampleLimit = 999999 }
            );
        }
    }
}
