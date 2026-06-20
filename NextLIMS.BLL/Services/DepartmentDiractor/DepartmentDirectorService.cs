using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using NextLIMS.BLL.DTO.DepartmentDto;
using NextLIMS.BLL.DTO.Sample;
using NextLIMS.BLL.Enums;
using NextLIMS.BLL.Extensions;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.DTO.DepartmentDto;
using NextLIMS.DAL.Repository.DepartmentDirector;
using NextLIMS.DAL.Repository.SampleRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.DepartmentDiractor
{
    public class DepartmentDirectorService
    {
        private DepartmentDirectorRepository repository;
        private IHttpContextAccessor httpContextAccessor;
        public DepartmentDirectorService(DepartmentDirectorRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public int TenantId => int.Parse(httpContextAccessor.HttpContext.User
               .FindFirst("TenantId").Value);

        public int UserId => int.Parse(httpContextAccessor.HttpContext.User
              .FindFirst(ClaimTypes.NameIdentifier).Value);
        public async Task AssignSampleToAnalystAsync(
       int sampleId,
       int analystId,
       string? reason
       )
        {
            var sampleTests =
                await repository.GetSampleTestsAsync(sampleId, TenantId);

            foreach (var sampleTest in sampleTests)
            {
                sampleTest.AssignedToUserId = analystId;
            }

            var workflow = new SampleWorkflow
            {
                TenantId = TenantId,
                Action = "Pending",
                StartDate = DateTime.Now,
                Flag = true,
                Reason = reason??" " ,
                Level = 1,
                AssignedToId = analystId,
                SampleId = sampleId,
                EndDate = DateTime.Now
            };

            await repository.AddWorkflowAsync(workflow);

            var sample = await repository.GetSampleByIdAsync(sampleId);

            if (sample != null)
            {
                sample.Status = "Pending";
            }

            await repository.SaveChangesAsync();
        }

        public async Task<List<MySamplesDto>> getMySamples()
        {
            var result = await repository.GetMySamplesAsync(TenantId, UserId);
            return result.Select(r =>
            {
                var dto = r.Sample.ToDto();
                dto.IsAssignedToAnalyst = r.IsAssignedToAnalyst;
                return dto;
            }).ToList();
        }
        public async Task<DepartmentWorkloadDto> dashboard()
        {
            return await repository.GetDepartmentWorkload(TenantId);
        }

        public async Task<bool> applyUpdateAsync(int sampleId,string reason)
        {

            var result = await repository.updateStatus(sampleId, SampleStatuses.Pending, TenantId,reason);
            if (!result)
                return false;
            return true;


        }
        public async Task<bool> approveAsync(int sampleId,string reason)
        {

            var result = await repository.approveSample(sampleId, SampleStatuses.Approved, TenantId,reason);
            if (!result)
                return false;
            return true;
        }
        public async Task<List<MySamplesDto>>pendingApproval()
        {
            var result = await repository.getPendingApprovalSamples(TenantId,SampleStatuses.Approved);
            return result.ToDto();
        }

        public async Task<List<object>> getDirectors()
        {
            var result=await repository.directors(TenantId);

            return result;
        }
    }
}
