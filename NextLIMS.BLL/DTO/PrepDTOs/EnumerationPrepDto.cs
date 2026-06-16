using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.BLL.DTO.prep
{
    public class EnumerationPrepDto
    {
        public decimal? Weight { get; set; }
        public decimal? DiluentAmount { get; set; }
        public ICollection<Dilution> dilutions { get; set; } = new List<Dilution>();
    }
    public class Dilution
    {
        public string? DilutionOrVolume { get; set; }
        public string? VolumePlated { get; set; }
    }
}

