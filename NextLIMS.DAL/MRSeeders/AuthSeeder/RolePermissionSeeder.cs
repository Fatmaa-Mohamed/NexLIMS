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

    // ===== Admin (Id = 1) — Full access to everything =====
    new RolePermission { Id = 1, RoleId = 1, PermissionId = 1 },   // GET_ALL_Permissions
    new RolePermission { Id = 2, RoleId = 1, PermissionId = 2 },   // CreateRole
    new RolePermission { Id = 3, RoleId = 1, PermissionId = 3 },   // GetRoles
    new RolePermission { Id = 4, RoleId = 1, PermissionId = 4 },   // Add_Permissions_to_Role
    new RolePermission { Id = 5, RoleId = 1, PermissionId = 5 },   // Remove_Permission_from_User
    new RolePermission { Id = 6, RoleId = 1, PermissionId = 6 },   // Invite_employee
    new RolePermission { Id = 7, RoleId = 1, PermissionId = 7 },   // GetEmployeesByLab
    new RolePermission { Id = 8, RoleId = 1, PermissionId = 8 },   // CreateSample
    new RolePermission { Id = 9, RoleId = 1, PermissionId = 9 },   // GetAllSamples
    new RolePermission { Id = 10, RoleId = 1, PermissionId = 10 }, // GetSampleWithItsTests
    new RolePermission { Id = 11, RoleId = 1, PermissionId = 11 }, // Add_Sample_To_Test
    new RolePermission { Id = 12, RoleId = 1, PermissionId = 12 }, // Remove_Sample_From_Test
    new RolePermission { Id = 13, RoleId = 1, PermissionId = 13 }, // Filter_Samples_by_sampleId_OR_ClientId
    new RolePermission { Id = 14, RoleId = 1, PermissionId = 14 }, // Filter_Samples_by_Status
    new RolePermission { Id = 39, RoleId = 1, PermissionId = 16 }, // Get_Confirmation_Templates //NEW
    new RolePermission { Id = 40, RoleId = 1, PermissionId = 17 }, // Save_Enumeration_Prep
    new RolePermission { Id = 41, RoleId = 1, PermissionId = 18 }, // Save_Detection_Prep
    new RolePermission { Id = 43, RoleId = 1, PermissionId = 19 }, // Save_Molecular_Prep
    new RolePermission { Id = 154, RoleId = 1, PermissionId = 21 }, // Get_Sample_Details_With_Preps 
    new RolePermission { Id = 155, RoleId = 1, PermissionId = 27 }, // Save_Enumeration_Prep_Data 
    new RolePermission { Id = 156, RoleId = 1, PermissionId = 28 }, // Save_Detection_Prep_Data 
    new RolePermission { Id = 35, RoleId = 1, PermissionId = 15 },//Setup Wizard
    new RolePermission { Id = 44, RoleId = 1, PermissionId = 20 }, // SendClientPortalInvitation

    new RolePermission { Id = 47, RoleId = 1, PermissionId = 22 }, // AssignToAnalystASpeciicSample
    new RolePermission { Id = 48, RoleId = 1, PermissionId = 23 }, // GetMySamples
    new RolePermission { Id = 49, RoleId = 1, PermissionId = 24 }, // ShowDashboard
    new RolePermission { Id = 50, RoleId = 1, PermissionId = 25 }, // ApplySampleRetest
    new RolePermission { Id = 51, RoleId = 1, PermissionId = 26 }, // Appro

    // ===== Receptionist (Id = 2) — View/Log samples =====
    new RolePermission { Id = 15, RoleId = 2, PermissionId = 8 },  // CreateSample
    new RolePermission { Id = 16, RoleId = 2, PermissionId = 9 },  // GetAllSamples
    new RolePermission { Id = 17, RoleId = 2, PermissionId = 10 }, // GetSampleWithItsTests
    new RolePermission { Id = 18, RoleId = 2, PermissionId = 13 }, // Filter_Samples_by_sampleId_OR_ClientId
    new RolePermission { Id = 19, RoleId = 2, PermissionId = 14 }, // Filter_Samples_by_Status
    new RolePermission { Id = 45, RoleId = 2, PermissionId = 20 }, // SendClientPortalInvitation

    // ===== Analyst (Id = 3) — First step of tests =====
    new RolePermission { Id = 20, RoleId = 3, PermissionId = 9 },  // GetAllSamples
    new RolePermission { Id = 21, RoleId = 3, PermissionId = 10 }, // GetSampleWithItsTests
    new RolePermission { Id = 22, RoleId = 3, PermissionId = 11 }, // Add_Sample_To_Test
    new RolePermission { Id = 23, RoleId = 3, PermissionId = 14 }, // Filter_Samples_by_Status
    new RolePermission { Id = 36, RoleId = 3, PermissionId = 16 }, // Get_Confirmation_Templates //NEW
    new RolePermission { Id = 37, RoleId = 3, PermissionId = 17 }, // Save_Enumeration_Prep
    new RolePermission { Id = 38, RoleId = 3, PermissionId = 18 }, // Save_Detection_Prep
    new RolePermission { Id = 42, RoleId = 3, PermissionId = 19 }, // Save_Molecular_Prep
    new RolePermission { Id = 52, RoleId = 3, PermissionId = 23 }, // GetMySamples

    // ===== Senior Analyst (Id = 4) — Enumeration, detection, confirmation, molecular tests, retest =====

    new RolePermission { Id = 24, RoleId = 4, PermissionId = 9 },  // GetAllSamples
    new RolePermission { Id = 25, RoleId = 4, PermissionId = 10 }, // GetSampleWithItsTests
    new RolePermission { Id = 26, RoleId = 4, PermissionId = 11 }, // Add_Sample_To_Test
    new RolePermission { Id = 27, RoleId = 4, PermissionId = 12 }, // Remove_Sample_From_Test 
    new RolePermission { Id = 28, RoleId = 4, PermissionId = 14 }, // Filter_Samples_by_Status
    new RolePermission { Id = 46, RoleId = 4, PermissionId = 21 }, // Get_Sample_Details_With_Preps 
    new RolePermission { Id = 152, RoleId = 4, PermissionId = 27 }, // Save_Enumeration_Prep_Data 
    new RolePermission { Id = 153, RoleId = 4, PermissionId = 28 }, // Save_Detection_Prep_Data 


     new RolePermission { Id = 62, RoleId = 4, PermissionId = 22 }, // AssignToAnalystASpeciicSample
    new RolePermission { Id = 53, RoleId = 4, PermissionId = 23 }, // GetMySamples
    new RolePermission { Id = 54, RoleId = 4, PermissionId = 24 }, // ShowDashboard
    new RolePermission { Id = 55, RoleId = 4, PermissionId = 25 }, // ApplySampleRetest
    new RolePermission { Id = 56, RoleId = 4, PermissionId = 26 },

    // ===== Department Director (Id = 5) — Approve results, retest, reassign tasks =====
    new RolePermission { Id = 29, RoleId = 5, PermissionId = 3 },  // GetRoles
    new RolePermission { Id = 30, RoleId = 5, PermissionId = 7 },  // GetEmployeesByLab
    new RolePermission { Id = 31, RoleId = 5, PermissionId = 9 },  // GetAllSamples
    new RolePermission { Id = 32, RoleId = 5, PermissionId = 10 }, // GetSampleWithItsTests
    new RolePermission { Id = 33, RoleId = 5, PermissionId = 12 }, // Remove_Sample_From_Test
    new RolePermission { Id = 34, RoleId = 5, PermissionId = 14 }, // Filter_Samples_by_Status

      new RolePermission { Id = 57, RoleId = 5, PermissionId = 22 }, // AssignToAnalystASpeciicSample
    new RolePermission { Id = 58, RoleId = 5, PermissionId = 23 }, // GetMySamples
    new RolePermission { Id = 59, RoleId = 5, PermissionId = 24 }, // ShowDashboard
    new RolePermission { Id = 60, RoleId = 5, PermissionId = 25 }, // ApplySampleRetest
    new RolePermission { Id = 61, RoleId = 5, PermissionId = 26 }  // Approve Sample By SampleId


);

        }
    }
}
