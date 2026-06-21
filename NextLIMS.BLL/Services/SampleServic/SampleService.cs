using Microsoft.AspNetCore.Http;
using NextLIMS.BLL.DTO.Client;
using NextLIMS.BLL.DTO.Sample;
using NextLIMS.BLL.DTO.SampleDTO;
using NextLIMS.BLL.Enums;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.RepoDTO.sampleData;
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

            int clientId;
            var existingClient = await _sampleRepository.findClient(tenantId, sampleDto.Client.NID);

            if (existingClient != null)
            {
                clientId = existingClient.Id;
            }
            else
            {
                var newClient = new Client
                {
                    Name = sampleDto.Client.Name,
                    NID = sampleDto.Client.NID,
                    PhoneNumber = sampleDto.Client.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy,
                    TenantId = tenantId,
                };

                var savedClient = await _sampleRepository.addClientAsync(newClient);
                clientId = savedClient.Id;
            }

            var sample = new Sample
            {
                SampleName = sampleDto.SampleName,
                SampleType = sampleDto.SampleType,
                Status = SampleStatuses.Registered,
                CreatedAt = DateTime.UtcNow,
                ClientId = clientId,
                CreatedBy = createdBy,
                TenantId = tenantId,
            };
            //continue from notion
            var savedSample = await _sampleRepository.AddSample(sample);
            //  var direcotor = await _sampleRepository.getDirectorBytenantandDepartment(tenantId);


            var sampleWorkflow = new SampleWorkflow
            {
                TenantId = tenantId,
                SampleId = savedSample.Id,
                Level = 0,
                AssignedToId = null,
                StartDate = DateTime.UtcNow,
                Action = SampleStatuses.Registered,
                Flag = null,
                Reason = null
            };
            await _sampleRepository.addsampleWorkflowAsync(sampleWorkflow);
            var response = new SampleResponseDto
            {
                ClientId = clientId,
                ClientName = sampleDto.Client.Name,
                ClientNID = sampleDto.Client.NID,
                CreatedAt = savedSample.CreatedAt,
                Id = savedSample.Id,
                SampleName = savedSample.SampleName,
                SampleType = savedSample.SampleType,
                Status = savedSample.Status
            };

            return response;
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
                    TestId = st.TenantTest?.TestId,
                    TestName = st.TenantTest?.Test.TestName,
                    TestType = st.TenantTest?.Test.TestType,
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
        public async Task<bool> attachTestsToSample(int sampleId, ICollection<int> testIds)
        {
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);

            var result = await _sampleRepository.AttachTestsToSample(sampleId, testIds, tenantId);
            return result;
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

            return await _sampleRepository.filterByStatus(status, tenantId);
        }

        public async Task<List<string>>? GetConfirmationTemplatesByTestId(int TestId)
        {
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
            return await _sampleRepository.GetConfirmationTemplatesByTestIdAsync(TestId, tenantId);
        }

        public async Task<SampleDetailsResponseDto>? GetSampleDetailsWithPreps(int SampleId)
        {
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
            var sample = await _sampleRepository.GetSampleWithAllTestDataAsync(SampleId, tenantId);
            if (sample == null) return null;

            var response = new SampleDetailsResponseDto
            {
                SampleId = sample.Id,
                SampleName = sample.SampleName,
                SampleType = sample.SampleType,

                Tests = sample.SampleTests?.Select(st => new SampleTestDetailsDto
                {
                    SampleTestId = st.Id,
                    Status = st.Status,
                    TestId = st.TenantTest.Test.Id,
                    TestName = st.TenantTest?.Test?.TestName,
                    TestType = st.TenantTest?.Test?.TestType,
                    EnumerationPrep = st.EnumerationData == null ? null : new EnumerationPrepResponseDto
                    {
                        Id = st.EnumerationData.Id,
                        Weight = st.EnumerationData.Weight,
                        DiluentAmount = st.EnumerationData.DiluentAmount,

                        Dilutions = st.EnumerationData.EnumerationDilutions?.Select(dil => new EnumerationDilutionResponseDto
                        {
                            Id = dil.Id,
                            DilutionType = dil.DilutionType,
                            IsSelectedForCalculation = dil.IsSelectedForCalculation,

                            DilutionOrVolume = dil.DilutionOrVolume,
                            VolumePlated = dil.VolumePlated,
                            ColonyCount = dil.ColonyCount
                        }).ToList() ?? new()
                    },
                    DetectionPrep = st.DetectionData == null ? null : new DetectionPrepResponseDto
                    {
                        Id = st.DetectionData.Id,
                        Weight = st.DetectionData.Weight,
                        EnrichmentMedia = st.DetectionData.EnrichmentMedia,
                        MediaAmount = st.DetectionData.MediaAmount
                    }
                }).ToList() ?? new()
            };

            return response;
        }

        public async Task AddSampleToDirector(int sampleid, int directorid)
        {
            var createdBy = int.Parse(_httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User
                .FindFirst("TenantId").Value);

            var oldSample = await _sampleRepository.GetSampleById(sampleid, tenantId);
            oldSample.Status = SampleStatuses.Pending;

            var sampleWorkflow = new SampleWorkflow
            {
                TenantId = tenantId,
                SampleId = oldSample.Id,
                Level = 0,
                AssignedToId = directorid,
                StartDate = DateTime.UtcNow,
                Action = SampleStatuses.Pending,
                Flag = true,
                Reason = null
            };
            await _sampleRepository.addsampleWorkflowAsync(sampleWorkflow);
            await _sampleRepository.savechangesasync();
        }
    }
}
