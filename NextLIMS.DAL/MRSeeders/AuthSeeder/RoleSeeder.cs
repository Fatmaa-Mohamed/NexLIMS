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
                new Role { CreatedAt=DateTime.Now, Description ="", Id=1, IsActive=true, Name="",  }
        
                );


        }
    }
}
