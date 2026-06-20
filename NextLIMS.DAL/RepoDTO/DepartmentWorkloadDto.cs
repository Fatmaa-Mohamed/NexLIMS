using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.DTO.DepartmentDto
{
    public class DepartmentWorkloadDto
    {
        public int TenantId { get; set; }

        public List<UserWorkloadDto> Users { get; set; } = new();
        //public int userId { get; set; }
        //public string userName { get; set; }
        //    public int LastSampleId { get; set; }
        //    public string LastSampleName {  get; set; }
        //    public string Status { get; set; }
        //    public int totalSampleCount { get; set; }

    }
    public class UserWorkloadDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TotalSamplesCount { get; set; }

        public List<AssignedSampleDto> AssignedSamples { get; set; } = new();
    }
    public class AssignedSampleDto
    {
        public int SampleId { get; set; }
        public string SampleName { get; set; }
        public string Status { get; set; }

        public int WorkflowId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Action { get; set; }
    }
}
