using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO.Sample
{
    public class SampleDTO
    {
        public int? Id { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string SampleName { get; set; }
        public string SampleType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

     
    }
}
