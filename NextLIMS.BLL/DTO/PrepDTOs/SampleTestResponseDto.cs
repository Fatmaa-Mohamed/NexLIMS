using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO.PrepDTOs
{
    public class SampleTestResponseDto
    {
        public int SampleTestId { get; set; }
        public string TestName { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public List<ConfirmationTestResponseDto> ConfirmationTests { get; set; }
        public bool SampleStatusUpdated { get; set; }
        public string NewSampleStatus { get; set; }
    }

    public class ConfirmationTestResponseDto
    {
        public int Id { get; set; }
        public string ConfirmationTestName { get; set; }
        public string Result { get; set; }
        public DateTime? DatePerformed { get; set; }
    }
}
