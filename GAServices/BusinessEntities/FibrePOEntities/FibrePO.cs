namespace GAServices.BusinessEntities.FibrePOEntities
{
    public class FibrePO
    {
        public long FibrePoid { get; set; }

        public DateTime Podate { get; set; }

        public string? Pono { get; set; }

        public long PartyId { get; set; }

        public string PartyName { get; set; }

        public string BranchName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public int GSTCode { get; set; }
        public string GSTNo { get; set; }
        public string EMailId { get; set; }

        //public int? CreatedBy { get; set; }

        //public DateTime? CreatedDate { get; set; }

        //public int? ModifiedBy { get; set; }

        //public DateTime? ModifiedDate { get; set; }

        //public sbyte? IsCancelled { get; set; }

        //public int? CancelledBy { get; set; }

        //public DateTime? CancelledDate { get; set; }

        //public virtual Party Party { get; set; }
        //public virtual Party Party { get; set; }

        //public virtual ICollection<FibrePODts> FibrePODts { get; set; } = new List<FibrePODts>();
        public List<FibrePODts> FibrePODts { get; set; }
    }

    public class FibrePODts
    {
        public long POId { get; set; }

        public long PoDtsId { get; set; }

        public long FibreTypeId { get; set; }

        public string FibreType { get; set; }

        public long ShadeId { get; set; }

        public string ShadeName { get; set; }

        public double Weight { get; set; }

        public double Rate { get; set; }

        public double Gstpercent { get; set; }

        public double Length { get; set; }

        public string Length_Unit { get; set; }

        public double Counts { get; set; }

        public string Counts_Unit { get; set; }

        //public virtual FibreType FibreType { get; set; } = null!;
        //public FibreType FibreType { get; }

        //public virtual FibrePO Po { get; set; } = null!;
        //public FibrePO Po { get; }
    }

    public class PartywisePOCounts
    {
        public long PartyId { get; set; }

        public string PartyName { get; set; }

        public long POCounts { get; set; }
    }

    public class PendingPODtsByParty
    {
        public long POId { get; set; }

        public string PONo { get; set; }

        public long PODtsId { get; set; }

        public long FibreTypeId { get; set; }

        public string FibreType { get; set; }

        public long ShadeId { get; set; }

        public string ShadeName { get; set; }

        public double OrderQty { get; set; }

        public double ReceivedQty { get; set; }

        public double BalanceQty { get; set; }

        public double Rate { get; set; }

        public double Gstpercent { get; set; }
    }

    public class POSummaryFor12Months
    {
        public string POMonthYear { get;set; }

        public long POCounts { get;set; }

        public double POQuantity { get; set; }

        public double ReceivedQty { get; set; }
    }

}