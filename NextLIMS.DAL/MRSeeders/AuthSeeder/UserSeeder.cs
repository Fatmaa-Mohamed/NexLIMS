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
    public class UserSeeder : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = 1,
                    TenantId = 3,
                    Name = "Ahmed Hassan",
                    Email = "ahmed.hassan@labtest.com",
                    PasswordHash = "$2b$12$Qe1daAScKU/Vt97doG0Y4.UU7kgptQzPFovBjfUucyRm/scg3sYDW",
                    RoleId = 1, // Admin
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = null
                },
                new User
                {
                    Id = 2,
                    TenantId = 3,
                    Name = "Mona Khaled",
                    Email = "mona.khaled@labtest.com",
                    PasswordHash = "$2b$12$Qe1daAScKU/Vt97doG0Y4.UU7kgptQzPFovBjfUucyRm/scg3sYDW",
                    RoleId = 2, // Receptionist
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = null
                },
                new User
                {
                    Id = 3,
                    TenantId = 3,
                    Name = "Youssef Adel",
                    Email = "youssef.adel@labtest.com",
                    PasswordHash = "$2b$12$Qe1daAScKU/Vt97doG0Y4.UU7kgptQzPFovBjfUucyRm/scg3sYDW",
                    RoleId = 3, // Analyst
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = null
                },
                new User
                {
                    Id = 4,
                    TenantId = 3,
                    Name = "Sara Mostafa",
                    Email = "sara.mostafa@labtest.com",
                    PasswordHash = "$2b$12$Qe1daAScKU/Vt97doG0Y4.UU7kgptQzPFovBjfUucyRm/scg3sYDW",
                    RoleId = 4, // Senior Analyst
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = null
                },
                new User
                {
                    Id = 5,
                    TenantId = 3,
                    Name = "Khaled Ibrahim",
                    Email = "khaled.ibrahim@labtest.com",
                    PasswordHash = "$2b$12$Qe1daAScKU/Vt97doG0Y4.UU7kgptQzPFovBjfUucyRm/scg3sYDW",
                    RoleId = 5, // Department Director
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreatedBy = null
                }
            );
        }
    }
}
