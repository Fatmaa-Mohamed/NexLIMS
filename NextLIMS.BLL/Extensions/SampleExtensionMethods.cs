using NextLIMS.BLL.DTO.Sample;
using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.Extensions
{
    public static class SampleExtensions
    {
        public static MySamplesDto ToDto(this Sample sample)
        {
            return new MySamplesDto
            {
                Id = sample.Id,
                ClientId = sample.ClientId,
                SampleName = sample.SampleName,
                SampleType = sample.SampleType,
                Status = sample.Status,
                CreatedAt = sample.CreatedAt,
                CreatedBy = sample.CreatedBy
            };
        }

        public static List<MySamplesDto> ToDto(this IEnumerable<Sample> samples)
        {
            return samples.Select(x => x.ToDto()).ToList();
        }
    }
}
