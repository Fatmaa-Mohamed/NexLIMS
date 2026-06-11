using Microsoft.EntityFrameworkCore;
using NextLIMS.BLL.DTO.TestDto;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository.Test;

namespace NextLIMS.BLL.Services.Tests
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        private readonly ApplicationDbContext _context;

        private static readonly HashSet<string> ValidTestTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "Enumeration", "Traditional Detection", "PCR Detection", "Molecular"
        };

        public TestService(ITestRepository testRepository, ApplicationDbContext context)
        {
            _testRepository = testRepository;
            _context = context;
        }

        public async Task<IEnumerable<GetTestDto>> GetAllGlobalAsync(int? departmentId, string? testType, CancellationToken ct = default)
        {
            if (!string.IsNullOrEmpty(testType) && !ValidTestTypes.Contains(testType))
                throw new ArgumentException($"Invalid testType '{testType}'. Accepted values: Enumeration, Traditional Detection, PCR Detection, Molecular.");

            var tests = await _testRepository.GetAllGlobalAsync(departmentId, testType, ct);

            return tests.Select(t => new GetTestDto
            {
                Id = t.Id,
                TestName = t.TestName,
                TestType = t.TestType,
                StandardMethod = t.StandardMethod,
                TurnaroundTime = t.TurnaroundTime,
                SampleTypes = t.TestSampleTypes
                    .Select(ts => new SampleTypeDto
                    {
                        Id = ts.SampleType.Id,
                        Name = ts.SampleType.Name
                    }).ToList(),
                ConfirmationTests = t.ConfirmationTestTemplates
                    .Select(c => new ConfirmationTestDto
                    {
                        Id = c.Id,
                        Name = c.ConfirmationTestName
                    }).ToList()
            });
        }

        public async Task<List<SelectTenantTestResponseDto>> SelectTenantTestsAsync(int tenantId, int createdBy, List<SelectTenantTestRequestDto> request, CancellationToken ct = default)
        {
            // Check for duplicates within request array
            var duplicateIdsInRequest = request
                .GroupBy(r => r.TestId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateIdsInRequest.Any())
                throw new ArgumentException($"Duplicate testIds in request: {string.Join(", ", duplicateIdsInRequest)}");

            // Check for duplicates already in TenantTests
            var testIds = request.Select(r => r.TestId).ToList();
            var duplicatesInDb = await _testRepository.GetDuplicateTenantTestIdsAsync(tenantId, testIds, ct);

            if (duplicatesInDb.Any())
                throw new InvalidOperationException($"Tests already added to this lab: {string.Join(", ", duplicatesInDb)}");

            // Validate all testIds exist in global catalog and all sampleTypeIds exist
            var tests = new List<Test>();
            foreach (var item in request)
            {
                var test = await _testRepository.GetGlobalByIdAsync(item.TestId, ct);
                if (test == null)
                    throw new KeyNotFoundException($"Test with id {item.TestId} was not found in the global catalog.");

                foreach (var sampleTypeId in item.SampleTypeIds)
                {
                    var sampleTypeExists = await _testRepository.SampleTypeExistsAsync(sampleTypeId, ct);
                    if (!sampleTypeExists)
                        throw new KeyNotFoundException($"SampleType with id {sampleTypeId} was not found.");
                }

                tests.Add(test);
            }

            // Build TenantTests with TenantTestSampleTypes in a single transaction
            using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                var tenantTests = request.Select((item, index) => new TenantTest
                {
                    TenantId = tenantId,
                    TestId = item.TestId,
                    Price = item.Price,
                    StandardMethod = item.StandardMethod,
                    TurnaroundTime = item.TurnaroundTime,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy,
                    TenantTestSampleTypes = item.SampleTypeIds.Select(sId => new TenantTestSampleType
                    {
                        SampleTypeId = sId
                    }).ToList()
                }).ToList();

                var created = await _testRepository.AddTenantTestsAsync(tenantTests, ct);
                await transaction.CommitAsync(ct);

                return created.Select((tt, index) =>
                {
                    var test = tests[index];
                    var requestItem = request[index];

                    return new SelectTenantTestResponseDto
                    {
                        TenantTestId = tt.Id,
                        TestId = tt.TestId,
                        TestName = test.TestName,
                        TestType = test.TestType,
                        Price = tt.Price ?? 0,
                        StandardMethod = tt.StandardMethod,
                        TurnaroundTime = tt.TurnaroundTime,
                        SampleTypes = requestItem.SampleTypeIds.Select(sId => // ← use requestItem
                        {
                            var st = test.TestSampleTypes.FirstOrDefault(ts => ts.SampleTypeId == sId)?.SampleType;
                            return new SampleTypeDto { Id = sId, Name = st?.Name ?? "" };
                        }).ToList()
                    };
                }).ToList();
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<List<CreateCustomTestResponseDto>> CreateCustomTestsAsync(int tenantId, int createdBy, List<CreateCustomTestRequestDto> request, CancellationToken ct = default)
        {
            // Validate testTypes
            var invalidTypes = request
                .Where(r => !ValidTestTypes.Contains(r.TestType))
                .Select(r => r.TestType)
                .Distinct()
                .ToList();

            if (invalidTypes.Any())
                throw new ArgumentException($"Invalid testType values: {string.Join(", ", invalidTypes)}. Accepted: Enumeration, Detection, PCR, Molecular.");

            // Validate departmentIds belong to tenant
            foreach (var item in request)
            {
                var deptExists = await _testRepository.TenantDepartmentExistsAsync(tenantId, item.DepartmentId, ct);
                if (!deptExists)
                    throw new KeyNotFoundException($"Department with id {item.DepartmentId} is not selected for this lab.");

                foreach (var sampleTypeId in item.SampleTypeIds)
                {
                    var sampleTypeExists = await _testRepository.SampleTypeExistsAsync(sampleTypeId, ct);
                    if (!sampleTypeExists)
                        throw new KeyNotFoundException($"SampleType with id {sampleTypeId} was not found.");
                }
            }

            // Build and save in a single transaction
            using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                var response = new List<CreateCustomTestResponseDto>();

                foreach (var item in request)
                {
                    // Insert into Tests with TenantId
                    var test = new Test
                    {
                        TenantId = tenantId,
                        DepartmentId = item.DepartmentId,
                        TestName = item.TestName,
                        TestType = item.TestType,
                        StandardMethod = item.StandardMethod,
                        TurnaroundTime = item.TurnaroundTime
                    };

                    var createdTest = await _testRepository.AddTestAsync(test, ct);

                    // Insert into TenantTests
                    var tenantTest = new TenantTest
                    {
                        TenantId = tenantId,
                        TestId = createdTest.Id,
                        Price = item.Price,
                        StandardMethod = item.StandardMethod,
                        TurnaroundTime = item.TurnaroundTime,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = createdBy,
                        TenantTestSampleTypes = item.SampleTypeIds.Select(sId => new TenantTestSampleType
                        {
                            SampleTypeId = sId
                        }).ToList()
                    };

                    var createdTenantTests = await _testRepository.AddTenantTestsAsync(new List<TenantTest> { tenantTest }, ct);
                    var createdTenantTest = createdTenantTests.First();

                    // Fetch sample type names for response
                    var sampleTypes = new List<SampleTypeDto>();
                    foreach (var sId in item.SampleTypeIds)
                    {
                        var st = await _testRepository.GetSampleTypeByIdAsync(sId, ct);
                        sampleTypes.Add(new SampleTypeDto { Id = sId, Name = st?.Name ?? "" });
                    }

                    response.Add(new CreateCustomTestResponseDto
                    {
                        TestId = createdTest.Id,
                        TenantTestId = createdTenantTest.Id,
                        TestName = createdTest.TestName,
                        TestType = createdTest.TestType,
                        Price = createdTenantTest.Price ?? 0,
                        StandardMethod = createdTenantTest.StandardMethod,
                        TurnaroundTime = createdTenantTest.TurnaroundTime,
                        SampleTypes = sampleTypes
                    });
                }

                await transaction.CommitAsync(ct);
                return response;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<EditTenantTestResponseDto> EditTenantTestAsync(int tenantTestId, int tenantId, EditTenantTestRequestDto request, CancellationToken ct = default)
        {
            // Fetch TenantTest — verifies it belongs to this tenant
            var tenantTest = await _testRepository.GetTenantTestByIdAsync(tenantTestId, tenantId, ct);
            if (tenantTest == null)
                throw new KeyNotFoundException($"TenantTest with id {tenantTestId} was not found for this lab.");

            // Validate all sampleTypeIds exist
            var sampleTypes = await _testRepository.GetSampleTypesByIdsAsync(request.SampleTypeIds, ct);
            if (sampleTypes.Count != request.SampleTypeIds.Count)
            {
                var missingIds = request.SampleTypeIds.Except(sampleTypes.Select(st => st.Id)).ToList();
                throw new KeyNotFoundException($"SampleType ids not found: {string.Join(", ", missingIds)}");
            }

            // If testType is being changed, check for active samples
            bool isCustomTest = tenantTest.Test.TenantId == tenantId;
            if (isCustomTest &&
                !string.IsNullOrEmpty(request.TestType) &&
                !string.Equals(request.TestType, tenantTest.Test.TestType, StringComparison.OrdinalIgnoreCase))
            {
                var hasActiveSamples = await _testRepository.HasActiveSamplesAsync(tenantTestId, ct);
                if (hasActiveSamples)
                    throw new InvalidOperationException("Cannot change testType — test has active samples currently in progress.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                // Update Tests table only for custom tests
                if (isCustomTest)
                {
                    if (!string.IsNullOrEmpty(request.TestName))
                        tenantTest.Test.TestName = request.TestName;

                    if (!string.IsNullOrEmpty(request.TestType))
                        tenantTest.Test.TestType = request.TestType;
                }

                // Always update TenantTests
                tenantTest.Price = request.Price;
                tenantTest.StandardMethod = request.StandardMethod;
                tenantTest.TurnaroundTime = request.TurnaroundTime;

                // Replace sample types — delete existing then insert new
                _context.TenantTestSampleTypes.RemoveRange(tenantTest.TenantTestSampleTypes);

                tenantTest.TenantTestSampleTypes = request.SampleTypeIds
                    .Select(sId => new TenantTestSampleType
                    {
                        TenantTestId = tenantTestId,
                        SampleTypeId = sId
                    }).ToList();

                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

                return new EditTenantTestResponseDto
                {
                    TenantTestId = tenantTest.Id,
                    TestId = tenantTest.TestId,
                    TestName = tenantTest.Test.TestName,
                    TestType = tenantTest.Test.TestType,
                    Price = tenantTest.Price ?? 0,
                    StandardMethod = tenantTest.StandardMethod,
                    TurnaroundTime = tenantTest.TurnaroundTime,
                    SampleTypes = sampleTypes.Select(st => new SampleTypeDto
                    {
                        Id = st.Id,
                        Name = st.Name
                    }).ToList()
                };
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<ToggleTenantTestStatusResponseDto> ToggleTenantTestStatusAsync(int tenantTestId, int tenantId, ToggleTenantTestStatusRequestDto request, CancellationToken ct = default)
        {
            var tenantTest = await _testRepository.GetTenantTestByIdAsync(tenantTestId, tenantId, ct);
            if (tenantTest == null)
                throw new KeyNotFoundException($"TenantTest with id {tenantTestId} was not found for this lab.");

            // If already matches, do nothing — not an error
            if (tenantTest.IsActive == request.IsActive!.Value)
                return new ToggleTenantTestStatusResponseDto
                {
                    TenantTestId = tenantTest.Id,
                    IsActive = tenantTest.IsActive
                };

            tenantTest.IsActive = request.IsActive!.Value;
            await _context.SaveChangesAsync(ct);

            return new ToggleTenantTestStatusResponseDto
            {
                TenantTestId = tenantTest.Id,
                IsActive = tenantTest.IsActive
            };
        }
    }
}