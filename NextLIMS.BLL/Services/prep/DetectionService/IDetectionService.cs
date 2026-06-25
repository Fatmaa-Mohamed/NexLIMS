using NextLIMS.BLL.DTO.PrepDTOs;

namespace NextLIMS.BLL.Services.prep.DetectionService
{
    public interface IDetectionService
    {
        Task<int> SaveDetectionPrepAsync(int sampleTestId, DetectionPrepDto dto);
    }
}
