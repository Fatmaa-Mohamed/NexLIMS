using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.RepoDTO.sampleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository.SampleRepo.LabRepo
{
    public interface ILabAnalysisRepository
    {
        Task SaveLabDataAsync(int SampleTestId, int tenantId, int userId ,
                              List<DilutionDto> dilutionDtos, 
                              List<ConfirmationTestDto> testDtos);
        Task<SampleTest?> GetSampleTestDetailsAsync(int sampleTestId);
        Task<EnumerationDilution?> GetSampleTestDetailsToCalcCFU(int sampleTestId);
        public Task SaveDetectionAsync(int SampleTestId, int tenantId, int userId
            , List<ConfirmationTestDto> testDtos);
    }
}
