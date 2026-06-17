using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.DTO.DepartmentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository.DepartmentDirector
{
    public class DepartmentDirectorRepository
    {
        private ApplicationDbContext _context;
        public DepartmentDirectorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<SampleTest>> GetSampleTestsAsync(int sampleId, int tenantId)
        {
            return await _context.SampleTests
                .Where(x => x.SampleId == sampleId &&
                            x.TenantTestId == tenantId)
                .ToListAsync();
        }

        public async Task<Sample?> GetSampleByIdAsync(int sampleId)
        {
            return await _context.Samples
                .FirstOrDefaultAsync(x => x.Id == sampleId);
        }

        public async Task AddWorkflowAsync(SampleWorkflow workflow)
        {
            await _context.SampleWorkflows.AddAsync(workflow);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Sample>> GetMySamplesAsync(int tenantId, int analystId)
        {
            var sampleIds = await _context.SampleWorkflows
          .Where(x => x.AssignedToId == analystId)
          .Select(x => x.SampleId)
          .Distinct()
          .ToListAsync();

            var result = await _context.Samples
                .Where(x =>
                    x.TenantId == tenantId &&
                    //   x.Status == status &&
                    sampleIds.Contains(x.Id))
                .ToListAsync();

            return result;
        }

        public async Task<DepartmentWorkloadDto> GetDepartmentWorkload(int tenantId)
        {
            var users = await _context.Users
      .Where(u => u.TenantId == tenantId)
      .Select(u => new UserWorkloadDto
      {
          UserId = u.Id,
          UserName = u.Name,
          AssignedSamples = _context.SampleWorkflows
              .Where(w => w.AssignedToId == u.Id)
              .GroupBy(w => w.SampleId)
              .Select(g => g
                  .OrderByDescending(w => w.Id)
                  .Select(w => new AssignedSampleDto
                  {
                      SampleId = w.SampleId,
                      SampleName = w.Sample.SampleName,
                      Status = w.Sample.Status,
                      WorkflowId = w.Id,
                      StartDate = w.StartDate,
                      EndDate = w.EndDate,
                      Action = w.Action
                  })
                  .First())
              .ToList(),

          TotalSamplesCount = _context.SampleWorkflows
              .Where(w => w.AssignedToId == u.Id)
              .Select(w => w.SampleId)
              .Distinct()
              .Count()
      })
      .ToListAsync();

            return new DepartmentWorkloadDto
            {
                TenantId = tenantId,
                Users = users
            };

        }
        public async Task<bool> updateStatus(int sampleid, string status, int tenantId)
        {
            var sample = await _context.Samples
     .FirstOrDefaultAsync(e => e.Id == sampleid && e.TenantId == tenantId);

            if (sample == null)
                return false;

            sample.Status = status;
            var firstWorkflow = await _context.SampleWorkflows
                .Where(w => w.SampleId == sampleid && w.TenantId == tenantId)
                .OrderBy(w => w.Id)
                .FirstOrDefaultAsync();
            var sampleWorkflow = new SampleWorkflow
            {
                TenantId=tenantId,
                SampleId = sampleid,
                Action = status,
                Level = 1,
                Flag = true,
                StartDate = DateTime.Now,
                AssignedToId = firstWorkflow?.AssignedToId // safe null check
            };
            await _context.SampleWorkflows.AddAsync(sampleWorkflow);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> approveSample(int sampleid, string status, int tenantId)
        {
            var sample = await _context.Samples
     .FirstOrDefaultAsync(e => e.Id == sampleid && e.TenantId == tenantId);

            if (sample == null)
                return false;

            sample.Status = status;
            var firstWorkflow = await _context.SampleWorkflows
                .Where(w => w.SampleId == sampleid && w.TenantId == tenantId)
                .OrderBy(w => w.Id)
                .FirstOrDefaultAsync();
            var sampleWorkflow = new SampleWorkflow
            {
                 TenantId=tenantId,
                SampleId = sampleid,
                Action = status,
                Level = 4,
                Flag = null,
                StartDate = DateTime.Now,
                AssignedToId = null // safe null check
            };
            await _context.SampleWorkflows.AddAsync(sampleWorkflow);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Sample>> getPendingApprovalSamples(int tenantid,string status)
        {
            var result = await _context.Samples.Where(e=>e.TenantId == tenantid&&e.Status==status).ToListAsync();
            return result;
        }
    }
}

    

