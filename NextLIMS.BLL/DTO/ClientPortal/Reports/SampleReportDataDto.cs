using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO.ClientPortal.Reports
{
    public sealed class SampleReportDataDto
    { 
        public string ReportNumber { get; set; } = string.Empty; 
        public DateTime GeneratedAtUtc { get; set; } 
        public string TenantName { get; set; } = string.Empty; 
        public string TenantLocation { get; set; } = string.Empty; 
        public string ClientName { get; set; } = string.Empty; 
        public string ClientNationalId { get; set; } = string.Empty; 
        public string ClientPhoneNumber { get; set; } = string.Empty; 
        public int SampleId { get; set; } 
        public string SampleName { get; set; } = string.Empty; 
        public string SampleType { get; set; } = string.Empty; 
        public string SampleStatus { get; set; } = string.Empty; 
        public DateTime RegisteredAt { get; set; } 
        public DateTime? ApprovedAt { get; set; } 
        public List<SampleReportTestDto> Tests { get; set; } = [];
        public SampleReportApprovalDto? SampleApproval { get; set; }
        public List<SampleReportAdminDto> TenantAdmins { get; set; } = [];
    }
    public sealed class SampleReportTestDto 
    { 
        public string TestName { get; set; } = string.Empty; 
        public string DepartmentName { get; set; } = string.Empty; 
        public string StandardMethod { get; set; } = string.Empty; 
        public string TestType { get; set; } = string.Empty;
        public string FormattedResult { get; set; } = string.Empty;
        public string ApprovedByName { get; set; } = string.Empty; 
        public string ApprovedByRole { get; set; } = string.Empty; 
        public DateTime? ApprovedAt { get; set; } 
    }
    public sealed class SampleReportApprovalDto 
    {
        public int UserId { get; set; } 
        public string ApproverName { get; set; } = string.Empty; 
        public string RoleName { get; set; } = string.Empty; 
        public DateTime? AssignedAt { get; set; } 
        public DateTime? ApprovedAt { get; set; } 
    }
    public sealed class SampleReportAdminDto 
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string RoleName { get; set; } = string.Empty; 
    }
    public sealed class GeneratedPdfReportDto 
    { 
        public byte[] Content { get; set; } = []; 
        public string FileName { get; set; } = string.Empty; 
        public string ContentType { get; set; } = "application/pdf"; 
    }
}
