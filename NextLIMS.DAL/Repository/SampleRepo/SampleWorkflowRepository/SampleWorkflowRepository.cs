using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace NextLIMS.DAL.Repository.SampleRepo.SampleWorkflowRepository
{
    public class SampleWorkflowRepository : ISampleWorkflowRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SampleWorkflowRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SetWorkflowToInProgressAsync(int sampleTestId, int tenantId , int Level, string Action)
        {
            var sampleTest = await _dbContext.SampleTests.FirstOrDefaultAsync(st => st.Id == sampleTestId);
            var workflow = await _dbContext.SampleWorkflows.OrderByDescending(w => w.Id).FirstOrDefaultAsync(w => w.SampleId == sampleTest.SampleId && w.TenantId == tenantId);
            if (workflow != null)
            {
                workflow.EndDate = DateTime.UtcNow;
                _dbContext.SampleWorkflows.Update(workflow);
            }
            var newWorkflow = new SampleWorkflow
            {
                TenantId = tenantId,
                SampleId = (int)sampleTest.SampleId,
                Level = Level,
                AssignedToId = workflow?.AssignedToId,
                StartDate = workflow?.EndDate ?? DateTime.UtcNow,
                Action = Action, 
            };
            await _dbContext.SampleWorkflows.AddAsync(newWorkflow);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
