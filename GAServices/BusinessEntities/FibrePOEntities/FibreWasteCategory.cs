namespace GAServices.BusinessEntities.FibrePOEntities
{
    public class FibreWasteCategory
    {
        public long WasteCategoryId { get; set; }
        public string WasteCategoryName { get; set; }
    }

    public class CreateFiberWaste
    {
		public long WasteCategoryId { get; set; }
		public string WasteCategoryName { get; set; }
        public double Quantity { get; set; }
	}
}
