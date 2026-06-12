using NextLIMS.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO.Sample
{
   public class SamplesDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string SampleName { get; set; }
        public string SampleType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }

        public Tenant Tenant { get; set; }
       // public Client Client { get; set; }
        public ICollection<SampleTest> SampleTests { get; set; }
        public ICollection<SampleWorkflow> SampleWorkflows { get; set; }
        public ICollection<AuditLog> AuditLogs { get; set; }
    }
}
