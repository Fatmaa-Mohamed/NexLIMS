using NextLIMS.BLL.DTO.PrepDTOs;
using NextLIMS.DAL.RepoDTO.sampleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.prep.LabAnalysisService
{
    public interface ILabAnalysisService
    {
        Task<SampleTestResponseDto?> ProcessLabDataAsync(int SampleTestId, SaveLabDataRequest request);
        public Task<SampleTestResponseDto?> ProcessDetectionAsync(int SampleTestId, DetectionPrepResultDTO request);
    }
}
