namespace GAServices.BusinessEntities.FibrePOEntities
{
    public class ReceiveFibrePO
    {
        public string RecdDCNo { get; set; }

        public string RecdDate { get; set; }

        public string DCDate { get; set; }

        public long PartyId { get; set; }

        public long ReceivedByUserId { get; set; }

        public List<ReceiveFibrePODts> FibrePODts { get; set; }
    }

    public class ReceiveFibrePODts
    {
        public long PoDtsId { get; set; }

        public long FiberTypeId { get; set; }

        public long FiberShadeId { get; set; }

        public string Lot { get; set; }

        public long HSNCode { get; set; }

        public double ReceivedWeight { get; set; }

        public double ReceivedBales { get; set; }

        public double Rate { get; set; }

        public double GSTPercent { get; set; }
    }
}
