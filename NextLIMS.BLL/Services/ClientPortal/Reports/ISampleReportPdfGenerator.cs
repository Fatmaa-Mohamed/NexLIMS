using NextLIMS.BLL.DTO.ClientPortal.Reports;

namespace NextLIMS.BLL.Services.ClientPortal.Reports
{
    public interface ISampleReportPdfGenerator 
    {
        byte[] Generate(SampleReportDataDto report); 
    }
}
