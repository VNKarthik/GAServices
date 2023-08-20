using Org.BouncyCastle.Crypto.Digests;

namespace GAServices.BusinessEntities.FibrePOEntities
{
    public class FibreStock
    {
        public string ReceivedDCNo { get; set; }
        public long ReceivedDtsId { get; set; }
        public string ReceivedDate { get; set; }
        public long FibreCategoryId { get; set; }
        public string PartyName { get; set; }
        public string FibreCategoryName { get; set; }
        //public string CategoryCode { get; set; }
        //public int CategoryOrder { get; set; }
        public long FibreTypeId { get; set; }
        public string FibreType { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public string Lot { get; set; }
        public long AgeInDays { get; set; }
        public double ReceivedQty { get; set; }
        public double IssuedQty { get; set; }
        public double Stock { get; set; }
        public double Rate { get; set; }
    }

    //public class FiberConsumption
    //{
    //    public long ReceivedDtsId { get; set; }
    //    public string ReceivedDCNo { get; set; }
    //    public DateOnly ReceivedDate { get; set; }
    //    public string FiberName { get; set; }
    //    public double ReceivedQty { get; set; }
    //    public DateOnly StockDate { get; set; }
    //    public double StockQty { get; set; }
    //    public List<FiberIssueDetails> IssuedDetails { get; set; }
    //}

    public class FiberIssueDetails
    {
        public string IssuedProgramNo { get; set; }
        public string IssuedDate { get; set; }
        public string YarnShade { get; set; }
        public string BlendName { get; set; }
        public double IssuedQty { get; set; }

    }
}
