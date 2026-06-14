using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.DataSeed;
using NexLIMS.API.DataSeed;
using NextLIMS.DAL.Data.Models;
using System.Text.Json;

namespace NextLIMS.DAL
{
    public class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            await SeedDepartments(context);
            await SeedSampleTypes(context);
            await SeedTests(context);
            await SeedConfirmationTestTemplates(context);
        }

        private static string GetPath(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, "DataSeed", fileName);
        }

        // ---------------- DEPARTMENTS ----------------
        private static async Task SeedDepartments(ApplicationDbContext context)
        {
            var json = await File.ReadAllTextAsync(GetPath("departments.json"));
            var items = JsonSerializer.Deserialize<List<DepartmentSeed>>(json);
            if (items == null) return;

            foreach (var item in items)
            {
                var exists = await context.Departments
                    .AnyAsync(x => x.Name == item.Name);

                if (!exists)
                {
                    context.Departments.Add(new Department
                    {
                        Name = item.Name
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        // ---------------- SAMPLE TYPES ----------------
        private static async Task SeedSampleTypes(ApplicationDbContext context)
        {
            var json = await File.ReadAllTextAsync(GetPath("sampleTypes.json"));
            var items = JsonSerializer.Deserialize<List<SampleTypeSeed>>(json);
            if (items == null) return;

            foreach (var item in items)
            {
                var exists = await context.SampleTypes
                    .AnyAsync(x => x.Name == item.Name);

                if (!exists)
                {
                    context.SampleTypes.Add(new SampleType
                    {
                        Name = item.Name
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        // ---------------- TESTS ----------------
        private static async Task SeedTests(ApplicationDbContext context)
        {
            var json = await File.ReadAllTextAsync(GetPath("tests.json"));
            var items = JsonSerializer.Deserialize<List<TestSeed>>(json);
            if (items == null) return;

            foreach (var seed in items)
            {
                var exists = await context.Tests.AnyAsync(x =>
                    x.TestName == seed.TestName &&
                    x.DepartmentId == seed.DepartmentId);

                if (exists)
                    continue;

                var test = new Test
                {
                    DepartmentId = seed.DepartmentId,
                    TenantId = seed.TenantId,
                    TestName = seed.TestName,
                    TestType = seed.TestType,
                    StandardMethod = seed.StandardMethod,
                    TurnaroundTime = seed.TurnaroundTime,

                    TestSampleTypes = seed.SampleTypeIds?.Select(id =>
                        new TestSampleType
                        {
                            SampleTypeId = id
                        }).ToList()
                };

                context.Tests.Add(test);
            }

            await context.SaveChangesAsync();
        }

        // ---------------- CONFIRMATION TEMPLATES ----------------
        private static async Task SeedConfirmationTestTemplates(ApplicationDbContext context)
        {
            var json = await File.ReadAllTextAsync(GetPath("confirmationTestTemplates.json"));
            var items = JsonSerializer.Deserialize<List<ConfirmationTestTemplateSeed>>(json);
            if (items == null) return;

            foreach (var item in items)
            {
                var exists = await context.ConfirmationTestTemplates.AnyAsync(x =>
                    x.TestId == item.TestId &&
                    x.ConfirmationTestName == item.ConfirmationTestName);

                if (!exists)
                {
                    context.ConfirmationTestTemplates.Add(new ConfirmationTestTemplate
                    {
                        TenantId = item.TenantId,
                        TestId = (int)item.TestId,
                        ConfirmationTestName = item.ConfirmationTestName
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}