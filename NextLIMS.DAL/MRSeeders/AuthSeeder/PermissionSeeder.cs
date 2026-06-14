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
    //it is just template till reciption flow end
    public class PermissionSeeder : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasData(
                new Permission { Id=1, Name="", Description="" }
                );



        }
    }
}
