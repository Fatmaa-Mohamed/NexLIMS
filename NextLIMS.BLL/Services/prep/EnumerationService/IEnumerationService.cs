using NextLIMS.BLL.DTO.prep;

namespace NextLIMS.BLL.Services.prep.EnumerationService
{
    public interface IEnumerationService
    {
        Task<int> SaveEnumerationPrepAsync(int sampleTestId, EnumerationPrepDto dto);
    }
}
