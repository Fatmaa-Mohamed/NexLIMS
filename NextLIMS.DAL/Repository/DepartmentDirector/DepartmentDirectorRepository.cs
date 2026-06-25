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
                            x.Sample.TenantId == tenantId)
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

        public async Task<List<(Sample Sample, bool IsAssignedToAnalyst)>> GetMySamplesAsync(int tenantId, int userId)
        {
            var sampleIds = await _context.SampleWorkflows
                .Where(x => x.AssignedToId == userId)
                .Select(x => x.SampleId)
                .Distinct()
                .ToListAsync();

            var samples = await _context.Samples
                .Include(s => s.SampleTests)
                .Where(x => x.TenantId == tenantId && sampleIds.Contains(x.Id))
                .ToListAsync();

            return samples
                .Select(s => (s, s.SampleTests.Any(st => st.AssignedToUserId.HasValue)))
                .ToList();
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
        public async Task<bool> updateStatus(int sampleid, string status, int tenantId,string reason)
        {
            var sample = await _context.Samples
     .FirstOrDefaultAsync(e => e.Id == sampleid && e.TenantId == tenantId);

            if (sample == null)
                return false;

            sample.Status = "Pending";

            var sampleTests = await _context.SampleTests
                .Where(st => st.SampleId == sampleid)
                .ToListAsync();
            foreach (var st in sampleTests)
                st.Status = "Pending";

            var sampleTestIds = sampleTests.Select(st => st.Id).ToList();

            // Clear confirmation tests
            var confirmationTests = await _context.SampleConfirmationTests
                .Where(ct => sampleTestIds.Contains(ct.SampleTestId))
                .ToListAsync();
            _context.SampleConfirmationTests.RemoveRange(confirmationTests);

            // Clear enumeration dilutions then enumeration data
            var enumerationDataList = await _context.EnumerationData
                .Where(ed => sampleTestIds.Contains(ed.SampleTestId))
                .ToListAsync();
            var enumerationDataIds = enumerationDataList.Select(ed => ed.Id).ToList();
            var dilutions = await _context.EnumerationDilutions
                .Where(d => enumerationDataIds.Contains(d.EnumerationDataId))
                .ToListAsync();
            _context.EnumerationDilutions.RemoveRange(dilutions);
            _context.EnumerationData.RemoveRange(enumerationDataList);

            // Clear detection data
            var detectionDataList = await _context.DetectionData
                .Where(dd => sampleTestIds.Contains(dd.SampleTestId))
                .ToListAsync();
            _context.DetectionData.RemoveRange(detectionDataList);

            var firstWorkflow = await _context.SampleWorkflows
                .Where(w => w.SampleId == sampleid && w.TenantId == tenantId)
                .OrderBy(w => w.Id)
                .FirstOrDefaultAsync();

            var latestWorkflow = await _context.SampleWorkflows
                .Where(w => w.SampleId == sampleid && w.TenantId == tenantId)
                .OrderByDescending(w => w.Id)
                .FirstOrDefaultAsync();
            if (latestWorkflow != null)
                latestWorkflow.EndDate = DateTime.UtcNow;

            await _context.SampleWorkflows.AddAsync(new SampleWorkflow
            {
                TenantId     = tenantId,
                SampleId     = sampleid,
                Action       = "InProgress",
                Level        = 1,
                Flag         = true,
                Reason       = reason ?? "",
                StartDate    = DateTime.UtcNow,
                AssignedToId = firstWorkflow?.AssignedToId,
            });
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> approveSample(int sampleid, string status, int tenantId,string reason)
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
                 Reason=reason,
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

        public async Task<List<object>> directors(int tenantId)
        {
            var knownRoles = new HashSet<string>
                { "Admin", "Analyst", "Senior Analyst", "Department Director", "Receptionist" };

            var users = await _context.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .Where(u => u.TenantId == tenantId && u.IsActive)
                .ToListAsync();

            return users
                .Where(u => u.Role != null && (
                    (knownRoles.Contains(u.Role.Name) && u.Role.Name == "Department Director") ||
                    (!knownRoles.Contains(u.Role.Name) &&
                     u.Role.RolePermissions.Any(rp => rp.Permission.Name == "AssignToAnalystASpeciicSample") &&
                     !u.Role.RolePermissions.Any(rp => rp.Permission.Name == "GET_ALL_Permissions"))
                ))
                .Select(u => (object)new
                {
                    UserId   = u.Id,
                    UserName = u.Name,
                    Email    = u.Email,
                    IsActive = u.IsActive
                })
                .ToList();
        }
    }
}

    

