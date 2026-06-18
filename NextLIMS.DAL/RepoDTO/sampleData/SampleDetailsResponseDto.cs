using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLIMS.DAL.RepoDTO.sampleData
{
    public class SampleDetailsResponseDto
    {
        public int SampleId { get; set; }
        public string? SampleName { get; set; }
        public string? SampleType { get; set; }
        public List<SampleTestDetailsDto> Tests { get; set; } = new();
    }

    public class SampleTestDetailsDto
    {
        public int SampleTestId { get; set; }
        public int TestId { get; set; }
        public string? TestName { get; set; }
        public string? TestType { get; set; }
        public string? Status { get; set; }
        public EnumerationPrepResponseDto? EnumerationPrep { get; set; }
        public DetectionPrepResponseDto? DetectionPrep { get; set; }
    }

    public class EnumerationPrepResponseDto
    {
        public int Id { get; set; }
        public decimal? Weight { get; set; }
        public decimal? DiluentAmount { get; set; }
        public List<EnumerationDilutionResponseDto> Dilutions { get; set; } = new();
    }

    public class EnumerationDilutionResponseDto
    {
        public int Id { get; set; }
        public string? DilutionOrVolume { get; set; }
        public string? DilutionType { get; set; }
        public string? VolumePlated { get; set; }
        public string? ColonyCount { get; set; }
        public bool IsSelectedForCalculation { get; set; }
    }

    public class DetectionPrepResponseDto
    {
        public int Id { get; set; }
        public decimal? Weight { get; set; }
        public string? EnrichmentMedia { get; set; }
        public decimal? MediaAmount { get; set; }
    }
}