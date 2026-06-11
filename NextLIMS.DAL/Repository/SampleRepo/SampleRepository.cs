using Microsoft.EntityFrameworkCore;
using NextLIMS.DAL.Data;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.Repository.SampleRepo
{
    public class SampleRepository
    {
        private readonly ApplicationDbContext _context;
        public SampleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Sample>> GetAllSamples(int tenantId)
        {
            var samples = await _context.Samples.Where(s => s.TenantId == tenantId).ToListAsync();
            return samples;
        }
        public async Task<Sample> GetSampleById(int id, int tenantId)
        {
            var sample = await _context.Samples.FirstOrDefaultAsync(s => s.Id == id && s.TenantId == tenantId);
            return sample;
        }
        public async Task<Sample?> AddSample(Sample sample)
        {
            await _context.Samples.AddAsync(sample);
            await _context.SaveChangesAsync();
            return sample;
        }
        
    }
}
