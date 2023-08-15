namespace GAServices.BusinessEntities.FibrePOEntities
{
    public class FibreStock
    {
        public string ReceivedDCNo { get; set; }
        public long ReceivedDtsId { get; set; }
        public long FibreCategoryId { get; set; }
        public string FibreCategoryName { get; set; }
        public string CategoryCode { get; set; }        
        public int CategoryOrder { get; set; }        
        public long FibreTypeId { get; set; }
        public string FibreType { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public string Lot { get; set; }        
        public double Stock { get; set; }
    }
}
