using NextLIMS.BLL.DTO.ClientPortal.Reports;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NextLIMS.BLL.Services.ClientPortal.Reports
{
    public sealed class SampleReportDocument : IDocument 
    { 
        private const string BrandPurple = "#622ACA"; 
        private const string BrandLight = "#F3EEFF"; 
        private const string BorderColor = "#E2E8F0"; 
        private const string DarkText = "#172033"; 
        private const string MutedText = "#64748B"; 
        private readonly SampleReportDataDto _report; 
        public SampleReportDocument(SampleReportDataDto report) 
        {
            _report = report; 
        } 
        public DocumentMetadata GetMetadata() 
        {
            return new DocumentMetadata 
            {
                Title = $"Certificate of Analysis - Sample {_report.SampleId}",
                Author = _report.TenantName,
                Subject = $"Laboratory report for {_report.SampleName}",
                Creator = "NexLIMS" 
            };
        }
        public DocumentSettings GetSettings() 
        {
            return DocumentSettings.Default; 
        } 
        public void Compose(IDocumentContainer container) 
        {
            container.Page(page => 
            {
                page.Size(PageSizes.A4); 
                page.Margin(28); 
                page.PageColor(Colors.White); 
                page.DefaultTextStyle(style => style.FontSize(9).FontColor(DarkText)); 
                page.Header().Element(ComposeHeader); 
                page.Content()
                    .PaddingVertical(16)
                    .Column(column => {
                        column.Spacing(14); 
                        column.Item().Element(ComposeReportHeading); 
                        column.Item().Element(ComposeClientAndSampleInformation); 
                        column.Item().Element(ComposeTestResults); 
                        column.Item().Element(ComposeApprovalSection); 
                        column.Item().Element(ComposeDisclaimer); 
                    }); 
                page.Footer().Element(ComposeFooter); 
            }); 
        } 
        private void ComposeHeader(IContainer container) 
        {
            container
                .BorderBottom(1)
                .BorderColor(BorderColor)
                .PaddingBottom(12)
                .Row(row => 
                {
                    row.RelativeItem()
                        .Column(column => 
                        {
                            column.Item()
                                .Text(_report.TenantName)
                                .FontSize(18).SemiBold()
                                .FontColor(BrandPurple); 
                            column.Item()
                                .PaddingTop(3)
                                .Text(
                                    string.IsNullOrWhiteSpace(
                                        _report.TenantLocation) 
                                        ? "Laboratory" 
                                        : _report.TenantLocation)
                                .FontSize(9)
                                .FontColor(MutedText); 
                        }); 
                    row.AutoItem()
                        .AlignRight()
                        .Column(column => 
                        {
                            column.Item()
                                .Text("NexLIMS")
                                .FontSize(12)
                                .Bold()
                                .FontColor(DarkText); 
                            column.Item()
                                .AlignRight()
                                .PaddingTop(3)
                                .Text("Laboratory Information Management")
                                .FontSize(7).
                                FontColor(MutedText); 
                        }); 
                }); 
        }
        private void ComposeReportHeading(IContainer container) 
        {
            container
                .Background(BrandLight)
                .Border(1).BorderColor("#D8C8FF")
                .Padding(14).
                Row(row => {
                    row.RelativeItem()
                        .Column(column => 
                        {
                            column.Item()
                                .Text("CERTIFICATE OF ANALYSIS")
                                .FontSize(16)
                                .Bold()
                                .FontColor(BrandPurple); 
                            column.Item().
                                PaddingTop(4)
                                .Text($"Report number: {_report.ReportNumber}")
                                .FontSize(8)
                                .FontColor(MutedText); 
                        }); 
                    row.AutoItem()
                        .AlignRight()
                        .Column(column =>
                        {
                            column.Item()
                                .AlignRight()
                                .Text("Generated")
                                .FontSize(7)
                                .FontColor(MutedText); 
                            column.Item()
                            .AlignRight()
                            .PaddingTop(2)
                            .Text(
                                _report.GeneratedAtUtc
                                    .ToLocalTime()
                                    .ToString(
                                        "dd MMM yyyy HH:mm"))
                            .FontSize(9)
                            .SemiBold(); 
                        }); 
                }); 
        } 
        private void ComposeClientAndSampleInformation(IContainer container) 
        {
            container
                .Column(column => 
                {
                    column.Item()
                        .Text("Client and sample information")
                        .FontSize(11)
                        .Bold()
                        .FontColor(DarkText); 
                    column.Item()
                        .PaddingTop(7)
                        .Border(1)
                        .BorderColor(BorderColor)
                        .Padding(12)
                        .Column(details => 
                        {
                            details.Spacing(9); 
                            details.Item()
                                .Row(row => 
                                {
                                    row.RelativeItem()
                                        .Element(cell => 
                                            ComposeValueCell(cell, "Client", _report.ClientName)); 
                                    row.RelativeItem()
                                        .Element(cell => 
                                            ComposeValueCell(cell, "National ID", _report.ClientNationalId)); 
                                }); 
                            details.Item()
                                .Row(row => 
                                {
                                    row.RelativeItem()
                                        .Element(cell =>
                                            ComposeValueCell(cell, "Phone number", _report.ClientPhoneNumber)); 
                                    row.RelativeItem()
                                        .Element(cell =>
                                            ComposeValueCell(cell, "Sample ID", _report.SampleId.ToString())); 
                                }); 
                            details.Item()
                                .Row(row => 
                                {
                                    row.RelativeItem()
                                        .Element(cell =>
                                            ComposeValueCell(cell, "Sample name", _report.SampleName)); 
                                    row.RelativeItem()
                                        .Element(cell =>
                                            ComposeValueCell(cell, "Sample type", _report.SampleType)); 
                                });
                            details.Item()
                                .Row(row => 
                                {
                                    row.RelativeItem()
                                        .Element(cell => 
                                            ComposeValueCell(cell, "Registered date", _report.RegisteredAt.ToLocalTime().ToString("dd MMM yyyy HH:mm"))); 
                                    row.RelativeItem()
                                        .Element(cell => 
                                            ComposeValueCell(cell, "Status", _report.SampleStatus)); 
                                }); 
                        }); 
                }); 
        } 
        private static void ComposeValueCell(
            IContainer container, 
            string label, 
            string? value) 
        {
            container
                .PaddingRight(8)
                .Column(column => 
                { 
                    column.Item()
                        .Text(label)
                        .FontSize(7)
                        .FontColor(MutedText); 
                    column.Item()
                        .PaddingTop(2)
                        .Text(
                            string.IsNullOrWhiteSpace(value) 
                                ? "Not available" 
                                : value)
                        .FontSize(9)
                        .SemiBold()
                        .FontColor(DarkText); 
                });
        } 
        private void ComposeTestResults(
            IContainer container)
        {
            container.Column(column =>
            {
                column.Item()
                    .Text("Test results")
                    .FontSize(11)
                    .Bold()
                    .FontColor(DarkText);

                column.Item()
                    .PaddingTop(7)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1.50f);
                            columns.RelativeColumn(1.00f);
                            columns.RelativeColumn(1.35f);

                            // Result and unit are now one column.
                            columns.RelativeColumn(1.35f);

                            columns.RelativeColumn(1.15f);
                        });

                        table.Header(header =>
                        {
                            header.Cell()
                                .Element(HeaderCell)
                                .Text("Test");

                            header.Cell()
                                .Element(HeaderCell)
                                .Text("Department");

                            header.Cell()
                                .Element(HeaderCell)
                                .Text("ISO / Method");

                            header.Cell()
                                .Element(HeaderCell)
                                .Text("Result");

                            header.Cell()
                                .Element(HeaderCell)
                                .Text("Test approved by");
                        });

                        foreach (var test in _report.Tests)
                        {
                            table.Cell()
                                .Element(BodyCell)
                                .Text(test.TestName);

                            table.Cell()
                                .Element(BodyCell)
                                .Text(test.DepartmentName);

                            table.Cell()
                                .Element(BodyCell)
                                .Text(test.StandardMethod);

                            table.Cell()
                                .Element(BodyCell)
                                .Text(test.FormattedResult)
                                .SemiBold();

                            table.Cell()
                                .Element(BodyCell)
                                .Column(approval =>
                                {
                                    approval.Item()
                                        .Text(
                                            string.IsNullOrWhiteSpace(
                                                test.ApprovedByName)
                                                ? "Not recorded"
                                                : test.ApprovedByName);

                                    if (!string.IsNullOrWhiteSpace(
                                            test.ApprovedByRole))
                                    {
                                        approval.Item()
                                            .PaddingTop(2)
                                            .Text(test.ApprovedByRole)
                                            .FontSize(7)
                                            .FontColor(MutedText);
                                    }

                                    if (test.ApprovedAt.HasValue)
                                    {
                                        approval.Item()
                                            .PaddingTop(2)
                                            .Text(
                                                test.ApprovedAt.Value
                                                    .ToLocalTime()
                                                    .ToString(
                                                        "dd MMM yyyy"))
                                            .FontSize(7)
                                            .FontColor(MutedText);
                                    }
                                });
                        }
                    });
            });
        }

        private void ComposeApprovalSection(
            IContainer container)
        {
            container.Column(column =>
            {
                column.Item()
                    .Text("Electronic approvals")
                    .FontSize(11)
                    .Bold();

                column.Item()
                    .PaddingTop(7)
                    .Text("Sample approval")
                    .FontSize(8)
                    .SemiBold()
                    .FontColor(MutedText);

                if (_report.SampleApproval != null)
                {
                    var approval =
                        _report.SampleApproval;

                    column.Item()
                        .PaddingTop(7)
                        .Border(1)
                        .BorderColor(BorderColor)
                        .Padding(10)
                        .Row(row =>
                        {
                            row.RelativeItem()
                                .Column(details =>
                                {
                                    details.Item()
                                        .Text(
                                            approval.ApproverName)
                                        .FontSize(10)
                                        .SemiBold();

                                    if (!string.IsNullOrWhiteSpace(
                                            approval.RoleName))
                                    {
                                        details.Item()
                                            .PaddingTop(2)
                                            .Text(
                                                approval.RoleName)
                                            .FontSize(7)
                                            .FontColor(MutedText);
                                    }

                                    details.Item()
                                        .PaddingTop(2)
                                        .Text(
                                            $"User ID: {approval.UserId}")
                                        .FontSize(7)
                                        .FontColor(MutedText);
                                });

                            if (approval.ApprovedAt.HasValue)
                            {
                                row.AutoItem()
                                    .AlignRight()
                                    .Column(dateColumn =>
                                    {
                                        dateColumn.Item()
                                            .AlignRight()
                                            .Text("Approved")
                                            .FontSize(7)
                                            .FontColor(MutedText);

                                        dateColumn.Item()
                                            .AlignRight()
                                            .PaddingTop(2)
                                            .Text(
                                                approval.ApprovedAt.Value
                                                    .ToLocalTime()
                                                    .ToString(
                                                        "dd MMM yyyy HH:mm"))
                                            .FontSize(8)
                                            .SemiBold();
                                    });
                            }
                        });
                }
                else
                {
                    column.Item()
                        .PaddingTop(7)
                        .Border(1)
                        .BorderColor(BorderColor)
                        .Padding(10)
                        .Text(
                            "No Pending Approval workflow assignment was found.")
                        .FontSize(8)
                        .FontColor(MutedText);
                }

                column.Item()
                    .PaddingTop(12)
                    .Text("Tenant administrators")
                    .FontSize(8)
                    .SemiBold()
                    .FontColor(MutedText);

                column.Item()
                    .PaddingTop(7)
                    .Border(1)
                    .BorderColor(BorderColor)
                    .Padding(10)
                    .Column(adminColumn =>
                    {
                        if (_report.TenantAdmins.Count == 0)
                        {
                            adminColumn.Item()
                                .Text(
                                    "No active tenant administrator was found.")
                                .FontSize(8)
                                .FontColor(MutedText);

                            return;
                        }

                        foreach (
                            var admin
                            in _report.TenantAdmins)
                        {
                            adminColumn.Item()
                                .PaddingBottom(4)
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .Text(admin.Name)
                                        .FontSize(9)
                                        .SemiBold();

                                    row.AutoItem()
                                        .Text(admin.RoleName)
                                        .FontSize(7)
                                        .FontColor(MutedText);
                                });
                        }
                    });
            });
        }
        private static void ComposeDisclaimer(IContainer container) 
        {
            container
                .Background("#F8FAFC")
                .Border(1)
                .BorderColor(BorderColor)
                .Padding(10)
                .Text(
                    "This report applies only to the sample identified above. " + 
                    "The report was generated electronically through NexLIMS. " + 
                    "Electronic approval names represent the users who approved " + 
                    "or released the recorded laboratory results.")
                .FontSize(7)
                .LineHeight(1.4f)
                .FontColor(MutedText); 
        } 
        private void ComposeFooter(IContainer container) 
        { 
            container
                .BorderTop(1)
                .BorderColor(BorderColor)
                .PaddingTop(8)
                .Row(row => {
                    row.RelativeItem()
                        .Text(_report.ReportNumber)
                        .FontSize(7)
                        .FontColor(MutedText); 
                    row.AutoItem()
                        .Text(text => {
                            text.DefaultTextStyle(style =>
                                style
                                    .FontSize(7)
                                    .FontColor(MutedText)); 
                            text.Span("Page "); 
                            text.CurrentPageNumber(); 
                            text.Span(" of "); 
                            text.TotalPages(); 
                        }); 
                }); 
        } 
        private static IContainer HeaderCell(IContainer container) 
        {
            return container
                .Background(BrandLight)
                .BorderBottom(1)
                .BorderColor("#D8C8FF")
                .PaddingVertical(7)
                .PaddingHorizontal(5)
                .DefaultTextStyle(style => 
                    style
                        .FontSize(7)
                        .SemiBold()
                        .FontColor(BrandPurple)); 
        } 
        private static IContainer BodyCell(IContainer container) 
        {
            return container
                .BorderBottom(1)
                .BorderColor(BorderColor)
                .PaddingVertical(7)
                .PaddingHorizontal(5)
                .DefaultTextStyle(style => 
                    style
                        .FontSize(7.5f)
                        .FontColor(DarkText)); 
        } 
    }
}