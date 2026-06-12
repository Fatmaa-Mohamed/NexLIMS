using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.DTO.Sample
{
    public class SampleDataDto
    {
        public int sampleId {  get; set; }
        public string departmentName { get; set; }
        public string nid { get; set; }
        public string status { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
