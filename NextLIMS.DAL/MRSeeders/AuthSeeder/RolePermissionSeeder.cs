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
    public class RolePermissionSeeder : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasData(
                new RolePermission {Id=0, PermissionId = 1, RoleId = 1 }

                );
  
        }
    }
}
