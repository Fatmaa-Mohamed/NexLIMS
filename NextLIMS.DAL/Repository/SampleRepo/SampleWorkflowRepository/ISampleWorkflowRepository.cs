namespace NextLIMS.DAL.Repository.SampleRepo.SampleWorkflowRepository
{
    public interface ISampleWorkflowRepository
    {
        Task<int> SetWorkflowToInProgressAsync(int sampleTestId, int tenantId, int Level, string Action);
    }
}
