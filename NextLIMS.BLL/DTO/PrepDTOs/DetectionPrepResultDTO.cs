using NextLIMS.DAL.RepoDTO.sampleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO.PrepDTOs
{
    public class DetectionPrepResultDTO
    {
        public string? result {  get; set; }
        public List<ConfirmationTestDto> ConfirmationTests { get; set; }
    }
}
