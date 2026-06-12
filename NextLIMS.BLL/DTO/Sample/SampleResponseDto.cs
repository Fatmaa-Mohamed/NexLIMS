using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO.Sample
{
    public class SampleResponseDto
    {
        public int Id { get; set; }
        public string SampleName { get; set; } = string.Empty;
        public string SampleType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientNID { get; set; } = string.Empty;
    }
}
