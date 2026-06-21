using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;

namespace NextLIMS.DAL.Repository.SampleRepo.SampleWorkflowRepository
{
    public class SampleWorkflowRepository : ISampleWorkflowRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SampleWorkflowRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SetWorkflowToInProgressAsync(int sampleTestId, int tenantId, int Level, string Action)
        {
            var sampleTest = await _dbContext.SampleTests.FirstOrDefaultAsync(st => st.Id == sampleTestId);
            var workflow = await _dbContext.SampleWorkflows
                .OrderByDescending(w => w.Id)
                .FirstOrDefaultAsync(w => w.SampleId == sampleTest.SampleId && w.TenantId == tenantId);

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

            var sample = await _dbContext.Samples.FirstOrDefaultAsync(s => s.Id == sampleTest.SampleId);
            if (sample != null)
                sample.Status = "InProgress";

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateStutusInSampleTest(int sampleTestId, int tenantId, string? result, string Action)
        {
            var sampleTest = await _dbContext.SampleTests.FirstOrDefaultAsync(st => st.Id == sampleTestId);
            if (sampleTest != null)
            {
                sampleTest.Status = Action;
                sampleTest.Result = result;
                _dbContext.Update(sampleTest);
                return await _dbContext.SaveChangesAsync();
            }
            return 0;
        }
    }
}
