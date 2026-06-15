using NextLIMS.BLL.DTO.prep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.prep.EnumerationService
{
    public interface IEnumerationService
    {
        Task<int> SaveEnumerationPrepAsync(int sampleTestId, EnumerationPrepDto dto);
    }
}
