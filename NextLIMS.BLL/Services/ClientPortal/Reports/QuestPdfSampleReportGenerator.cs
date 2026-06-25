using NextLIMS.BLL.DTO.ClientPortal.Reports;
using QuestPDF.Fluent;


namespace NextLIMS.BLL.Services.ClientPortal.Reports
{
    public sealed class QuestPdfSampleReportGenerator : ISampleReportPdfGenerator 
    {
        public byte[] Generate(SampleReportDataDto report) 
        {
            var document = new SampleReportDocument(report); 
            return document.GeneratePdf(); 
        } 
    }
}
