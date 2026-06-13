namespace NextLIMS.DAL.Data.Models
{
    public class EnumerationDilution
    {
        public int Id { get; set; }
        public int EnumerationDataId { get; set; }
        public int TenantId { get; set; }
        public string DilutionOrVolume { get; set; }
        public string DilutionType { get; set; }
        public string ColonyCount { get; set; }
        public bool IsSelectedForCalculation { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public  string VolumePlated {  get; set; }
        public EnumerationData EnumerationData { get; set; }
        public Tenant Tenant { get; set; }
    }
}
