using Microsoft.AspNetCore.Http;
using NextLIMS.BLL.DTO.prep;
using NextLIMS.BLL.DTO.PrepDTOs;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.RepoDTO.sampleData;
using NextLIMS.DAL.Repository.SampleRepo.LabRepo;
using NextLIMS.DAL.Repository.SampleRepo.SampleWorkflowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.prep.LabAnalysisService
{
    public class LabAnalysisService : ILabAnalysisService
    {
        private readonly ILabAnalysisRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISampleWorkflowRepository _sampleWorkflowRepository;

        public LabAnalysisService(ILabAnalysisRepository repository , 
                                 IHttpContextAccessor httpContextAccessor ,
                                 ISampleWorkflowRepository sampleWorkflowRepository)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _sampleWorkflowRepository = sampleWorkflowRepository;
        }
        public async Task<SampleTestResponseDto?> ProcessLabDataAsync(int SampleTestId,SaveLabDataRequest request)
        {
            var createdBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
            var SelectedDilution = 0; 
            if (request.Dilutions != null)
            {
                foreach (var item in request.Dilutions)
                {
                    if (item.DilutionId <= 0)
                        throw new ArgumentException("Invalid Dilution ID.");
                    item.ColonyCount.ToUpper();
                    if (item.ColonyCount != "TNTC" && item.ColonyCount != "<10" && !int.TryParse(item.ColonyCount, out _))
                        throw new ArgumentException($"Invalid colony count structure '{item.ColonyCount}' for ID {item.DilutionId}.");
                    if (item.ColonyCount == "TNTC" || item.ColonyCount == "<10")
                        item.IsSelectedForCalculation = false;
                    if (item.IsSelectedForCalculation == true)
                        SelectedDilution = item.DilutionId;
                }
            }

            if (request.ConfirmationTests != null)
            {
                
                foreach (var test in request.ConfirmationTests)
                {
                    if (string.IsNullOrWhiteSpace(test.ConfirmationTestName))
                        throw new ArgumentException("Confirmation test name cannot be empty.");

                    if (string.IsNullOrWhiteSpace(test.Result))
                        throw new ArgumentException($"Result cannot be empty for test: {test.ConfirmationTestName}");
                }
            }
            await _repository.SaveLabDataAsync(SampleTestId,tenantId, createdBy, request.Dilutions, request.ConfirmationTests);
            var workflowwadded = await _sampleWorkflowRepository.SetWorkflowToInProgressAsync(SampleTestId, tenantId, 3, "Pending Approval");
            var result = await CFUCalculation(SelectedDilution);
            var SampleTestStuts = await _sampleWorkflowRepository.UpdateStutusInSampleTest(SampleTestId, tenantId, result , "Pending Approval");
            return await GetSampleTestDetailsAsync(SampleTestId);
        }
        private async Task<SampleTestResponseDto?> GetSampleTestDetailsAsync(int sampleTestId)
        {
            SampleTest? sampleTest = await _repository.GetSampleTestDetailsAsync(sampleTestId);

            if (sampleTest == null)
            {
                return null;
            }

            var responseDto = new SampleTestResponseDto
            {
                SampleTestId = sampleTest.Id,
                TestName = sampleTest.TenantTest?.Test?.TestName ?? "Unknown Test",
                Result = sampleTest.Result,
                Status = sampleTest.Status,


                ConfirmationTests = sampleTest.SampleConfirmationTests?
                    .Select(ct => new ConfirmationTestResponseDto
                    {
                        Id = ct.Id,
                        ConfirmationTestName = ct.ConfirmationTestName,
                        Result = ct.Result,
                        DatePerformed = ct.DatePerformed
                    }).ToList() ?? new List<ConfirmationTestResponseDto>(),

                SampleStatusUpdated = true,
                NewSampleStatus = sampleTest.Status
            };

            return responseDto;
        }
        private async Task<string> CFUCalculation(int EnumerationDilutionId)
        {
            var EnumerationDilution = await _repository.GetSampleTestDetailsToCalcCFU(EnumerationDilutionId);
            var totalCfuSum = 0.0;
            var sum = 0;
            if (EnumerationDilution != null)
            {
                if (EnumerationDilution.EnumerationData.SampleTest.Sample?.SampleType.ToLower() == "food" ||
                    EnumerationDilution.EnumerationData.SampleTest.Sample?.SampleType.ToLower() == "liquid food" ||
                    EnumerationDilution.EnumerationData.SampleTest.Sample?.SampleType.ToLower() == "swab" ||  
                    (EnumerationDilution.EnumerationData.SampleTest.Sample?.SampleType.ToLower() == "water" 
                    && (new[] { 3, 4, 10, 11 }.Contains(EnumerationDilution.EnumerationData.SampleTest.TenantTest?.Test?.Id ?? 0)
                    )))
                {


                    if (double.TryParse(EnumerationDilution.ColonyCount, out double colonies)
                        && double.TryParse(EnumerationDilution.DilutionOrVolume, out double DilutionOrVolume)
                        && double.TryParse(EnumerationDilution.VolumePlated, out double VolumePlated))
                    {
                        totalCfuSum = colonies / (Math.Pow(10, -DilutionOrVolume) * VolumePlated);
                    }                    

                }
                else if (EnumerationDilution.EnumerationData.SampleTest.Sample?.SampleType.ToLower() == "Water")
                {
                    if (double.TryParse(EnumerationDilution.ColonyCount, out double colonies)
                        && double.TryParse(EnumerationDilution.DilutionOrVolume, out double DilutionOrVolume)
                        && double.TryParse(EnumerationDilution.VolumePlated, out double VolumePlated))
                    {
                        totalCfuSum = (colonies * 100.0) / VolumePlated;
                    }

                }
                else if (EnumerationDilution.EnumerationData.SampleTest.Sample?.SampleType.ToLower() == "air")
                {
                    if (double.TryParse(EnumerationDilution.ColonyCount, out double colonies))
                    {
                        totalCfuSum = colonies ;
                    }
                }
            }
            if (totalCfuSum == 0) return "0";

            return totalCfuSum.ToString();
        }
        public async Task<SampleTestResponseDto?> ProcessDetectionAsync(int SampleTestId, DetectionPrepResultDTO request)
        {
            var createdBy = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var tenantId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("TenantId").Value);
            if (request.ConfirmationTests != null)
            {

                foreach (var test in request.ConfirmationTests)
                {
                    if (string.IsNullOrWhiteSpace(test.ConfirmationTestName))
                        throw new ArgumentException("Confirmation test name cannot be empty.");

                    if (string.IsNullOrWhiteSpace(test.Result))
                        throw new ArgumentException($"Result cannot be empty for test: {test.ConfirmationTestName}");
                }
            }
            await _repository.SaveDetectionAsync(SampleTestId, tenantId, createdBy, request.ConfirmationTests);
            var workflowwadded = await _sampleWorkflowRepository.SetWorkflowToInProgressAsync(SampleTestId, tenantId, 3, "Pending Approval");
            var SampleTestStuts = await _sampleWorkflowRepository.UpdateStutusInSampleTest(SampleTestId, tenantId, request.result , "Pending Approval");
            return await GetSampleTestDetailsAsync(SampleTestId);


        }


    }
}
