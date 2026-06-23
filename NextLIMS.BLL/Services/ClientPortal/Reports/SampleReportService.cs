using System.Globalization;
using NextLIMS.BLL.DTO.ClientPortal.Reports;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository.ClientPortal.Reports;

namespace NextLIMS.BLL.Services.ClientPortal.Reports
{
    public sealed class SampleReportService
        : ISampleReportService
    {
        private readonly ISampleReportRepository
            _sampleReportRepository;

        private readonly ISampleReportPdfGenerator
            _pdfGenerator;

        public SampleReportService(
            ISampleReportRepository sampleReportRepository,
            ISampleReportPdfGenerator pdfGenerator)
        {
            _sampleReportRepository =
                sampleReportRepository;

            _pdfGenerator =
                pdfGenerator;
        }

        public async Task<GeneratedPdfReportDto>
            GenerateClientSampleReportAsync(
                string tenantSlug,
                int tenantId,
                int clientId,
                int sampleId,
                CancellationToken cancellationToken = default)
        {
            var sample =
                await _sampleReportRepository
                    .GetClientSampleReportDataAsync(
                        tenantId,
                        clientId,
                        sampleId,
                        cancellationToken);

            if (sample == null)
            {
                throw new KeyNotFoundException(
                    "The requested sample was not found.");
            }

            if (!string.Equals(
                    sample.Tenant?.Slug,
                    tenantSlug,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new KeyNotFoundException(
                    "The requested sample was not found.");
            }

            if (!string.Equals(
                    sample.Status,
                    "Approved",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "A report can only be downloaded after the sample is approved.");
            }

            var sampleTests =
                sample.SampleTests?
                    .OrderBy(sampleTest =>
                        sampleTest.TenantTest?
                            .Test?
                            .Department?
                            .Name)
                    .ThenBy(sampleTest =>
                        sampleTest.TenantTest?
                            .Test?
                            .TestName)
                    .ToList()
                ?? [];

            if (sampleTests.Count == 0)
            {
                throw new InvalidOperationException(
                    "The approved sample does not contain any tests.");
            }

            var missingResults =
                sampleTests.Any(sampleTest =>
                    string.IsNullOrWhiteSpace(
                        sampleTest.Result));

            if (missingResults)
            {
                throw new InvalidOperationException(
                    "The report cannot be generated because one or more test results are missing.");
            }

            var approvalWorkflow = FindSampleApprovalWorkflow(sample); 
            
            var workflowApprover = approvalWorkflow?.AssignedTo; 
            
            var workflowApprovedAt = approvalWorkflow?.EndDate;

            var tenantAdmins =
                await _sampleReportRepository
                    .GetActiveTenantAdminsAsync(
                        tenantId,
                        cancellationToken);

            var reportTests =
                sampleTests
                    .Select(sampleTest =>
                        MapTest(
                            sample.SampleType,
                            sampleTest,
                            workflowApprover,
                            workflowApprovedAt))
                    .ToList();

            var reportNumber =
                $"NXL-{sample.TenantId:D4}-{sample.Id:D8}";

            var reportData =
                new SampleReportDataDto
                {
                    ReportNumber = reportNumber,
                    GeneratedAtUtc = DateTime.UtcNow,

                    TenantName =
                        sample.Tenant?.Name
                        ?? "Laboratory",

                    TenantLocation =
                        sample.Tenant?.Location
                        ?? string.Empty,

                    ClientName =
                        sample.Client?.Name
                        ?? "Client",

                    ClientNationalId =
                        sample.Client?.NID
                        ?? string.Empty,

                    ClientPhoneNumber =
                        sample.Client?.PhoneNumber
                        ?? string.Empty,

                    SampleId = sample.Id,

                    SampleName =
                        sample.SampleName,

                    SampleType =
                        sample.SampleType,

                    SampleStatus =
                        sample.Status,

                    RegisteredAt =
                        sample.CreatedAt,

                    ApprovedAt = workflowApprovedAt ?? sampleTests
                        .Where(sampleTest => sampleTest.ApprovedAt.HasValue)
                        .Select(sampleTest => sampleTest.ApprovedAt)
                        .Max(),

                    Tests =
                        reportTests,

                    SampleApproval = 
                        workflowApprover == null 
                            ? null : new SampleReportApprovalDto 
                            {
                                UserId = workflowApprover.Id, 
                                ApproverName = workflowApprover.Name, 
                                RoleName = workflowApprover.Role?.Name ?? string.Empty, 
                                AssignedAt = approvalWorkflow?.StartDate, 
                                ApprovedAt = approvalWorkflow?.EndDate 
                            },

                    TenantAdmins =
                        tenantAdmins
                            .Select(admin =>
                                new SampleReportAdminDto
                                {
                                    UserId = admin.Id,
                                    Name =
                                        admin.Name,

                                    RoleName =
                                        admin.Role?.Name
                                        ?? "Admin"
                                })
                            .ToList()
                };

            var pdf =
                _pdfGenerator.Generate(
                    reportData);

            return new GeneratedPdfReportDto
            {
                Content = pdf,
                FileName =
                    $"Sample-{sample.Id}-Report.pdf",
                ContentType =
                    "application/pdf"
            };
        }

        private static SampleWorkflow? FindSampleApprovalWorkflow( Sample sample) 
        {
            var workflows = sample.SampleWorkflows? 
                .Where(workflow => workflow.TenantId == sample.TenantId &&
                    workflow.AssignedToId.HasValue &&
                    workflow.AssignedTo != null &&
                    IsPendingApprovalAction( workflow.Action)) 
                .ToList() ?? []; 
            if (workflows.Count == 0) 
            {
                return null; 
            } 
            var completedWorkflow = workflows 
                .Where(workflow => workflow.EndDate.HasValue) 
                .OrderByDescending(workflow => workflow.EndDate) 
                .ThenByDescending(workflow => workflow.Id) 
                .FirstOrDefault(); 
            if (completedWorkflow != null) 
            {
                return completedWorkflow; 
            }
            return workflows 
                .OrderByDescending(workflow => workflow.StartDate) 
                .ThenByDescending(workflow => workflow.Id) 
                .FirstOrDefault(); 
        } 
        private static bool IsPendingApprovalAction( string? action) 
        {
            var normalized = NormalizeKey(action); 
            return normalized.Contains( "pendingapproval"); 
        }

        private static SampleReportTestDto MapTest(
            string sampleType,
            SampleTest sampleTest,
            User? workflowApprover,
            DateTime? workflowApprovedAt)
        {
            var tenantTest =
                sampleTest.TenantTest;

            var test =
                tenantTest?.Test;

            var testType =
                test?.TestType
                ?? string.Empty;

            var sampleWeight =
                sampleTest.DetectionData?.Weight
                ?? sampleTest.EnumerationData?.Weight;

            return new SampleReportTestDto
            {
                TestName =
                    test?.TestName
                    ?? $"Test {sampleTest.TenantTestId}",

                DepartmentName =
                    test?.Department?.Name
                    ?? "Not specified",

                StandardMethod =
                    !string.IsNullOrWhiteSpace(
                        tenantTest?.StandardMethod)
                        ? tenantTest.StandardMethod
                        : test?.StandardMethod
                          ?? "Not specified",

                TestType = testType,

                FormattedResult =
                    FormatResult(
                        sampleTest.Result,
                        sampleType,
                        testType,
                        sampleWeight),

                ApprovedByName =
                    workflowApprover?.Name
                    ?? "Not recorded",

                ApprovedByRole =
                    workflowApprover?.Role?.Name
                    ?? string.Empty,

                ApprovedAt =
                    workflowApprovedAt
            };
        }

        private static string FormatResult(
            string? rawResult,
            string? sampleType,
            string? testType,
            decimal? sampleWeight)
        {
            var result =
                string.IsNullOrWhiteSpace(rawResult)
                    ? "Not reported"
                    : rawResult.Trim();

            if (IsDetectionTestType(testType))
            {
                result =
                    NormalizeDetectionResult(
                        result);

                return FormatDetectionResult(
                    result,
                    sampleType,
                    sampleWeight);
            }

            var unit =
                GetEnumerationUnit(
                    sampleType);

            if (string.IsNullOrWhiteSpace(unit))
            {
                return result;
            }

            // Avoid adding CFU twice if the result already
            // contains a unit.
            if (result.Contains(
                    "CFU",
                    StringComparison.OrdinalIgnoreCase))
            {
                return result;
            }

            return $"{result} {unit}";
        }

        private static string
            NormalizeDetectionResult(
                string result)
        {
            if (string.Equals(
                    result,
                    "Negative",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Not detected";
            }

            if (string.Equals(
                    result,
                    "Positive",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Detected";
            }

            return result;
        }

        private static string FormatDetectionResult(
            string result,
            string? sampleType,
            decimal? sampleWeight)
        {
            var normalizedSampleType =
                NormalizeKey(sampleType);

            if (
                normalizedSampleType.Contains("food") ||
                normalizedSampleType.Contains("soil"))
            {
                if (!sampleWeight.HasValue)
                {
                    return
                        $"{result} (sample weight not recorded)";
                }

                return
                    $"{result} in {FormatNumber(sampleWeight.Value)} g";
            }

            if (
                normalizedSampleType.Contains("water"))
            {
                if (!sampleWeight.HasValue)
                {
                    return
                        $"{result} (sample volume not recorded)";
                }

                return
                    $"{result} in {FormatNumber(sampleWeight.Value)} mL";
            }

            if (
                normalizedSampleType.Contains("swab"))
            {
                return $"{result} in swab";
            }

            return result;
        }

        private static string GetEnumerationUnit(
            string? sampleType)
        {
            var normalizedSampleType =
                NormalizeKey(sampleType);

            if (
                normalizedSampleType.Contains("food") ||
                normalizedSampleType.Contains("soil"))
            {
                return "CFU/g";
            }

            if (
                normalizedSampleType.Contains("water"))
            {
                return "CFU/100 mL";
            }

            if (
                normalizedSampleType.Contains("swab"))
            {
                return "CFU/swab";
            }

            if (
                normalizedSampleType.Contains("air"))
            {
                return "CFU/plate";
            }

            return string.Empty;
        }

        private static bool IsDetectionTestType(
            string? testType)
        {
            var normalized =
                NormalizeKey(testType);

            return
                normalized.Contains(
                    "traditionaldetection") ||
                normalized.Contains(
                    "pcrdetection") ||
                normalized.Contains(
                    "moleculartest") ||
                normalized.Contains(
                    "molecular");
        }

        private static string NormalizeKey(
            string? value)
        {
            return new string(
                (value ?? string.Empty)
                    .Where(char.IsLetterOrDigit)
                    .Select(char.ToLowerInvariant)
                    .ToArray());
        }

        private static string FormatNumber(
            decimal value)
        {
            return value.ToString(
                "0.##",
                CultureInfo.InvariantCulture);
        }
    }
}