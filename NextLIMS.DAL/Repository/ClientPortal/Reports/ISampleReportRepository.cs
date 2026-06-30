using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository.ClientPortal.Reports
{
    public interface ISampleReportRepository 
    {
        Task<Sample?> GetClientSampleReportDataAsync(
            int tenantId, 
            int clientId, 
            int sampleId, 
            CancellationToken cancellationToken = default); 
        Task<IReadOnlyList<User>> GetActiveTenantAdminsAsync(
            int tenantId, 
            CancellationToken cancellationToken = default); 
    }
}
