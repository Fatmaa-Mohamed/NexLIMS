using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.RepoDTO.sampleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository.SampleRepo.LabRepo
{
    public class LabAnalysisRepository : ILabAnalysisRepository
    {
        private readonly ApplicationDbContext _context;

        public LabAnalysisRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<EnumerationDilution?> GetSampleTestDetailsToCalcCFU(int EnumerationDilutionId)
        {
            return await _context.EnumerationDilutions.Include(ST => ST.EnumerationData)
                                                        .ThenInclude(St => St.SampleTest).ThenInclude(St => St.Sample)
                                             .FirstOrDefaultAsync(s => s.Id == EnumerationDilutionId);

        }
        public async Task<SampleTest?> GetSampleTestDetailsAsync(int sampleTestId)
        {
            return await _context.SampleTests
                .Include(s => s.TenantTest)
                    .ThenInclude(ts => ts.Test)
                .Include(s => s.SampleConfirmationTests)
                .FirstOrDefaultAsync(s => s.Id == sampleTestId);
        }

        public async Task SaveLabDataAsync(int SampleTestId , int tenantId , int userId
            , List<DilutionDto> dilutionDtos, List<ConfirmationTestDto> testDtos)
        {
            if (dilutionDtos != null)
            {
                foreach (var dto in dilutionDtos)
                {
                    var existingDilution = await _context.EnumerationDilutions
                        .FirstOrDefaultAsync(d => d.Id == dto.DilutionId);

                    if (existingDilution != null)
                    {
                        existingDilution.ColonyCount = dto.ColonyCount;
                        existingDilution.IsSelectedForCalculation = dto.IsSelectedForCalculation;
                    }
                }
            }

            if (testDtos != null)
            {
                foreach (var dto in testDtos)
                {
                    var newConfirmationTest = new SampleConfirmationTest
                    {
                        PerformedBySeniorAnalystId = userId,
                        SampleTestId = SampleTestId,
                        ConfirmationTestName = dto.ConfirmationTestName,
                        Result = dto.Result,
                        DatePerformed = DateTime.UtcNow 
                    };

                    await _context.SampleConfirmationTests.AddAsync(newConfirmationTest);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task SaveDetectionAsync(int SampleTestId, int tenantId, int userId ,List<ConfirmationTestDto> testDtos)
        {
            if (testDtos != null)
            {
                foreach (var dto in testDtos)
                {
                    var newConfirmationTest = new SampleConfirmationTest
                    {
                        PerformedBySeniorAnalystId = userId,
                        SampleTestId = SampleTestId,
                        ConfirmationTestName = dto.ConfirmationTestName,
                        Result = dto.Result,
                        DatePerformed = DateTime.UtcNow
                    };

                    await _context.SampleConfirmationTests.AddAsync(newConfirmationTest);
                }
            }
            await _context.SaveChangesAsync();
        }

    }
}
