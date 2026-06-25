using Azure;
using Microsoft.EntityFrameworkCore;
using NextLIMS.BLL.DTO.Sample;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.RepoDTO.sampleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace NextLIMS.DAL.Repository.SampleRepo
{
    public class SampleRepository
    {
        private readonly ApplicationDbContext _context;
        public SampleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<SampleDataDto>> GetAllSamplesAsync(int tenantId, int page, int pageSize)
        {
            var samples = await _context.Samples
                .AsNoTracking()
                .Where(e => e.TenantId == tenantId)
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new SampleDataDto
                {
                    sampleId = e.Id,
                    departmentName = e.Tenant.TenantDepartments
                        .Select(td => td.Department.Name)
                        .FirstOrDefault(),
                    nid = e.Client.NID,
                    RegisteredAt = e.CreatedAt,
                    status = e.Status,
                })
                .ToListAsync();
            return samples;
        }
        public async Task<Sample> GetSampleById(int id, int tenantId)
        {
            var sample = await _context.Samples.FirstOrDefaultAsync(s => s.Id == id && s.TenantId == tenantId);
            return sample;
        }
        public async Task<Sample?> AddSample(Sample sample)
        {
            await _context.Samples.AddAsync(sample);
            await _context.SaveChangesAsync();
            return sample;
        }
        public async Task<Client> addClientAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return client;
        }
        public async Task<Sample?> getSampleWithItsTests(int id, int tenantid)
        {
            return await _context.Samples
                .Include(s => s.SampleTests)
                .ThenInclude(st => st.TenantTest)
                .ThenInclude(tt => tt.Test)
        .Include(s => s.SampleTests)
            .ThenInclude(st => st.AssignedToUser)
        .FirstOrDefaultAsync(s => s.Id == id &&
                                  s.TenantId == tenantid);
        }
        //test
        public async Task<bool> AttachTestsToSample(int sampleId, ICollection<int> testIds, int tenantId)
        {
            var sample = await _context.Samples
                .FirstOrDefaultAsync(s => s.Id == sampleId && s.TenantId == tenantId);

            var allExist = await _context.TenantTests
                .Where(t => t.TenantId == tenantId)
                .CountAsync(t => testIds.Contains(t.Id)) == testIds.Count;
          //  fetch the actual TenantTest records for this lab that match the incoming TestIds

           var tenantTests = await _context.TenantTests
               .Where(t => t.TenantId == tenantId && testIds.Contains(t.Id))
               .ToListAsync();

            if (tenantTests.Count != testIds.Count)
                    return false;

            if (sample == null)
                throw new Exception("Sample not found");

                var sampleTests = testIds.Select(testId => new SampleTest
                {
                    SampleId = sampleId,
                    TenantTestId = testId,
                    Status = "pending",
                    CreatedAt = DateTime.UtcNow

                });

                await _context.SampleTests.AddRangeAsync(sampleTests);

                await _context.SaveChangesAsync();
            return true;
            }

        public async Task DetachTestsFromSample(int sampleId, ICollection<int> testIds, int tenantId)
        {
            var sampleTests = await _context.SampleTests
                .Where(st =>
                    st.SampleId == sampleId &&
                    st.Sample.TenantId == tenantId &&
                    st.Status == "pending" &&
                    testIds.Contains(st.TenantTestId.Value))
                .ToListAsync();

            if (!sampleTests.Any())
                return;

            _context.SampleTests.RemoveRange(sampleTests);

            await _context.SaveChangesAsync();
        }
        //unfinished
        public async Task<List<SampleDataDto>> filterByStatus(string status ,int tenantId)
        {
            var samples = await _context.Samples
            .AsNoTracking()
            .Where(e => e.TenantId == tenantId&&e.Status==status)
            .OrderBy(e => e.Id)
            .Select(e => new SampleDataDto
            {
                sampleId = e.Id,
                departmentName = e.Tenant.TenantDepartments
                .Select(td => td.Department.Name)
                .FirstOrDefault(),
                nid = e.Client.NID,
                RegisteredAt = e.CreatedAt,
                status = e.Status,
            })
            .ToListAsync();
            return samples;
        }
        public async Task<List<SampleDataDto>> FilterSamples(int tenantId, int? sampleId = null, int? clientId = null)
        {
            var query = _context.Samples
                .AsNoTracking()
                .Where(e => e.TenantId == tenantId);

            if (sampleId.HasValue)
                query = query.Where(e => e.Id == sampleId.Value);

            if (clientId.HasValue)
                query = query.Where(e => e.ClientId == clientId.Value);

            var result = await query
                .OrderBy(e => e.Id)
                .Select(e => new SampleDataDto
                {
                    sampleId = e.Id,
                    departmentName = e.Tenant.TenantDepartments
                        .Select(td => td.Department.Name)
                        .FirstOrDefault(),
                    nid = e.Client.NID,
                    RegisteredAt = e.CreatedAt,
                    status = e.Status,
                })
                .ToListAsync();

            return result;
        }
        public async Task<Client> findClient(int tenantId,string nid)
        {
            return await _context.Clients.FirstOrDefaultAsync(e=>e.TenantId==tenantId&&e.NID== nid);
        }

        public async Task<List<string>>? GetConfirmationTemplatesByTestIdAsync(int testid, int tenantId)
        {
            return await _context.ConfirmationTestTemplates
                                .Where(s => s.TestId == testid && (s.TenantId == null || s.TenantId == tenantId))
                                .Select(S=>S.ConfirmationTestName).ToListAsync();
        }

        public async Task<Sample?> GetSampleWithAllTestDataAsync(int sampleId, int tenantId)
        {
            return await _context.Samples
                .Where(s => s.Id == sampleId && s.TenantId == tenantId)
                .Include(s => s.SampleTests)
                    .ThenInclude(st => st.TenantTest)
                        .ThenInclude(tt => tt.Test)
                .Include(s => s.SampleTests)
                    .ThenInclude(st => st.EnumerationData)
                        .ThenInclude(ed => ed.EnumerationDilutions)
                .Include(s => s.SampleTests)
                    .ThenInclude(st => st.DetectionData)
                .Include(s => s.SampleTests)
                    .ThenInclude(st => st.SampleConfirmationTests)
                .FirstOrDefaultAsync();
        }

        public async Task addsampleWorkflowAsync(SampleWorkflow sampleWorkflow)
        {
            await _context.SampleWorkflows.AddAsync(sampleWorkflow);
            await _context.SaveChangesAsync();
        }

        public async Task<User> getDirectorBytenantandDepartment(int tenantId)
        {
            var result = await _context.Users.Include(e => e.Role).Where(e => e.Role.Name == "Department Director" && e.TenantId == tenantId).FirstOrDefaultAsync();

            return result;
        }
        public async Task savechangesasync()
        {
            await _context.SaveChangesAsync();
        }


        // Method for showing sample statistics for clients:
        public async Task<(List<Sample> Items, int TotalCount)>
            GetClientSamplesAsync(
                int tenantId,
                int clientId,
                int page,
                int pageSize,
                CancellationToken cancellationToken = default)
        {
            var query = _context.Samples
                .AsNoTracking()
                .Where(sample =>
                    sample.TenantId == tenantId &&
                    sample.ClientId == clientId);

            var totalCount =
                await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(sample => sample.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}