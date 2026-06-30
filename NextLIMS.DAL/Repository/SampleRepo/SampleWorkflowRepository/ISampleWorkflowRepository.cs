using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository.SampleRepo.SampleWorkflowRepository
{
    public interface ISampleWorkflowRepository
    {
        Task<int> SetWorkflowToInProgressAsync(int sampleTestId, int tenantId,int Level, string Action);
        Task<int> UpdateStutusInSampleTest(int sampleTestId, int tenantId, string? result, string Action);
    }
}
