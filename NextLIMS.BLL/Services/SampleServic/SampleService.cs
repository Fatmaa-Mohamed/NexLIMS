using NextLIMS.BLL.DTO.Sample;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.Repository.SampleRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Services.SampleServic
{
    public class SampleService
    {
        private readonly SampleRepository _sampleRepository;

        public SampleService(SampleRepository sampleRepository)
        {
            _sampleRepository = sampleRepository;
        }
        public async Task<SampleDTO> CreateSampleAsync(SampleDTO sampleDto)
        {
            var sample = new Sample
            {
                SampleName = sampleDto.SampleName,
                CreatedAt = DateTime.UtcNow,
                SampleType = sampleDto.SampleType,
                Status = "pending"
            };
            var createdSample = await _sampleRepository.AddSample(sample);
            return new SampleDTO
            {
                Id = createdSample.Id,
                SampleName = createdSample.SampleName,

                CreatedAt = createdSample.CreatedAt
            };
        }
    }
}
