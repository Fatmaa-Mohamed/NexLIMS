using Azure;
using Microsoft.EntityFrameworkCore;
using NextLIMS.BLL.DTO.Sample;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

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
   .FirstOrDefaultAsync(s => s.Id == id &&
                             s.TenantId == tenantid &&
                             s.Status == "pending");

        }
        public async Task AttachTestsToSample(int sampleId, ICollection<int> testIds, int tenantId)
        {
            var sample = await _context.Samples
                .FirstOrDefaultAsync(s => s.Id == sampleId && s.TenantId == tenantId);

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
    }
}
//var samples = await _context.Samples.AsNoTracking()
//    .Include(o=>o.Client)
//    .Include(o=>o.Tenant)
//    .ThenInclude(e=>e.TenantDepartments)
//    .ThenInclude(e=>e.Department)
//    .Where(e => e.TenantId == tenantId)
//    .Select(e => new SampleDataDto
//    {
//        sampleId = e.Id,
//        departmentName = e.Tenant.TenantDepartments.Select(e=>e.Department.Name).ToString(),
//        nid=e.Client.NID,
//        RegisteredAt=e.CreatedAt,
//        status=e.Status,
//    })
//    .Skip((page-1)*pagesize)
//    .Take(pagesize)
//    .AsSplitQuery().
//    ToListAsync();