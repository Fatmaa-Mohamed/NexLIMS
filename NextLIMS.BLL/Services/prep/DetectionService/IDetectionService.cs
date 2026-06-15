using NextLIMS.BLL.DTO.PrepDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.prep.DetectionService
{
    public interface IDetectionService
    {
        Task<int> SaveDetectionPrepAsync(int sampleTestId, DetectionPrepDto dto);
    }
}
