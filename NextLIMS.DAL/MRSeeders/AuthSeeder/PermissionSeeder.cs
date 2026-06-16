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
   
     new Permission { Id = 1, Name = "GET_ALL_Permissions", Description = "Allows retrieving the list of all available permissions in the system" },
     new Permission { Id = 2, Name = "CreateRole", Description = "Allows creating a new role" },
     new Permission { Id = 3, Name = "GetRoles", Description = "Allows retrieving the list of all roles" },
     new Permission { Id = 4, Name = "Add_Permissions_to_Role", Description = "Allows assigning one or more permissions to a role" },
     new Permission { Id = 5, Name = "Remove_Permission_from_User", Description = "Allows removing a specific permission from a user" },
     new Permission { Id = 6, Name = "Invite_employee", Description = "Allows sending an invitation to a new employee to join the lab" },
     new Permission { Id = 7, Name = "GetEmployeesByLab", Description = "Allows retrieving the list of employees belonging to a specific lab" },
     new Permission { Id = 8, Name = "CreateSample", Description = "Allows creating a new sample record" },
     new Permission { Id = 9, Name = "GetAllSamples", Description = "Allows retrieving the list of all samples" },
     new Permission { Id = 10, Name = "GetSampleWithItsTests", Description = "Allows retrieving a sample along with its associated tests" },
     new Permission { Id = 11, Name = "Add_Sample_To_Test", Description = "Allows linking a sample to a specific test" },
     new Permission { Id = 12, Name = "Remove_Sample_From_Test", Description = "Allows removing a sample from a specific test" },
     new Permission { Id = 13, Name = "Filter_Samples_by_sampleId_OR_ClientId", Description = "Allows filtering samples by sample ID or client ID" },
     new Permission { Id = 14, Name = "Filter_Samples_by_Status", Description = "Allows filtering samples by their current status" },
          new Permission { Id = 15, Name = "SetupWizard", Description = "Apply Wizard changes " }

     );




        }
    }

}