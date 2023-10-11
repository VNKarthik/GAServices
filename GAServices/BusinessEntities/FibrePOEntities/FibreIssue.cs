namespace GAServices.BusinessEntities.FibrePOEntities
{
    public class FibreMixing
    {
        public long ProgramId { get; set; }

        public string MixingDate { get; set; }

        public List<FibreIssued> Fibres { get; set; }

        public long IssuedByUseId { get; set; }
    }

    public class FibreIssued
    {
        public long ReceivedDtsId { get; set; }

        public long FibreTypeId { get; set; }

        public long ShadeId { get; set; }

        public string Lot { get; set; }

        public decimal IssueQuantity { get; set; }

		public long ProductionWasteDtsId { get; set; }
	}
}
