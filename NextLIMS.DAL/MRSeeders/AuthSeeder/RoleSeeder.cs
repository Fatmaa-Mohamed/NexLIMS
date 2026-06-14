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
    public class RoleSeeder : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
            new Role { CreatedAt = DateTime.Now, Description = "Has full access to lab configuration, settings, users, and system management", Id = 1, IsActive = true, Name = "Admin" },
new Role { CreatedAt = DateTime.Now, Description = "Can view and log samples into the system", Id = 2, IsActive = true, Name = "Receptionist" },
new Role { CreatedAt = DateTime.Now, Description = "Performs the first step of laboratory tests", Id = 3, IsActive = true, Name = "Analyst" },
new Role { CreatedAt = DateTime.Now, Description = "Enters enumeration colony counts, records detection results (P/N), performs confirmation tests, performs advanced molecular tests, and requests retests", Id = 4, IsActive = true, Name = "Senior Analyst" },
new Role { CreatedAt = DateTime.Now, Description = "Approves test results, authorizes retests, and reassigns tasks", Id = 5, IsActive = true, Name = "Department Director" }

);


        }
    }
}
