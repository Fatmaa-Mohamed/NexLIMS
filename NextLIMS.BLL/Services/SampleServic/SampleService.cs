using Microsoft.AspNetCore.Http;
using NextLIMS.BLL.DTO.Client;
using NextLIMS.BLL.DTO.Sample;
using NextLIMS.BLL.DTO.SampleDTO;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository.SampleRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.SampleServic
{
    public class SampleService
    {
        private readonly SampleRepository _sampleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SampleService(SampleRepository sampleRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sampleRepository = sampleRepository;
            _httpContextAccessor = httpContextAccessor;
        }//fix it
        public async Task<SampleResponseDto> CreateSampleAsync(SampleDTO sampleDto)
        {
            var createdBy = int.Parse(_httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User
                .FindFirst("TenantId").Value);

            var newClient = new Client
            {
                Name = sampleDto.Client.Name,
                NID = sampleDto.Client.NID,
                PhoneNumber = sampleDto.Client.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy,
                TenantId = tenantId,
            };

            var savedClient = await _sampleRepository.addClientAsync(newClient);  // upsert
            int clientId = savedClient.Id;
            var sample = new Sample
            {
                SampleName = sampleDto.SampleName,
                SampleType = sampleDto.SampleType,
                Status = "pending",
                CreatedAt = DateTime.UtcNow,
                ClientId = clientId,   // ✅ use the returned client's Id
                CreatedBy = createdBy,
                TenantId = tenantId,
            };
            var first = await _sampleRepository.AddSample(sample);

            var result = new SampleResponseDto
            {
                ClientId = clientId,
                ClientName = sampleDto.Client.Name,
                ClientNID = sampleDto.Client.NID,
                CreatedAt = DateTime.UtcNow,
                Id = first.Id,
                SampleName = first.SampleName,
                SampleType = sampleDto.SampleType,
                Status = first.Status
            };
            return result;
        }
        public async Task<List<SampleDataDto>> getAllSamplesAsync(int page, int pageSize)
        {
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
            var samples = await _sampleRepository.GetAllSamplesAsync(tenantId, page, pageSize);
            return samples;
        }

        public async Task<SampleWithTestsDTO> getSampleWithTestsById(int id)
        {
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
            var sample = await _sampleRepository.getSampleWithItsTests(id, tenantId);

            return new SampleWithTestsDTO
            {
                Id = sample.Id,
                TenantId = sample.TenantId,
                ClientId = sample.ClientId,
                SampleName = sample.SampleName,
                SampleType = sample.SampleType,
                Status = sample.Status,
                CreatedAt = sample.CreatedAt,
                CreatedBy = sample.CreatedBy,

                Tests = sample.SampleTests.Select(st => new SampleTestDTO
                {
                    Id = st.Id,
                    TenantTestId = st.TenantTestId,
                    TestName = st.TenantTest?.Test.TestName, // adjust to your model
                    Status = st.Status,
                    Result = st.Result,
                    AssignedToUserId = st.AssignedToUserId,
                    AssignedToUserName = st.AssignedToUser?.Name,
                    ApprovedBy = st.ApprovedBy,
                    ApprovedAt = st.ApprovedAt,
                    CreatedAt = st.CreatedAt
                }).ToList()
            };

        }
        public async Task attachTestsToSample(int sampleId, ICollection<int> testIds)
        {
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);

            await _sampleRepository.AttachTestsToSample(sampleId,testIds,tenantId);

        }
        
        public async Task detachTestsfromSample(int sampleId, ICollection<int> testIds)
        {
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);

            await _sampleRepository.DetachTestsFromSample(sampleId, testIds, tenantId);
        }
        public async Task<List<SampleDataDto>> filterSampleByoptions(int? sampleId = null, int? clientId = null)
        {
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);

            return await _sampleRepository.FilterSamples(tenantId, sampleId, clientId);
        }
        public async Task<List<SampleDataDto>> filterSampleByStatus(string status)
        {
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);

            return await _sampleRepository.filterByStatus( status,tenantId);
        }

    }
}