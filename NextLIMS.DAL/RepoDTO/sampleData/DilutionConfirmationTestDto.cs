using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.RepoDTO.sampleData
{
    public class DilutionDto
    {
        public int DilutionId { get; set; }
        public string? ColonyCount { get; set; }
        public bool IsSelectedForCalculation { get; set; }
    }

    public class ConfirmationTestDto
    {
        public string? ConfirmationTestName { get; set; }
        public string? Result { get; set; }
    }

    public class SaveLabDataRequest
    {
        public List<DilutionDto> Dilutions { get; set; }
        public List<ConfirmationTestDto> ConfirmationTests { get; set; }
    }
}
