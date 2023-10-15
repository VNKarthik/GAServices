namespace GAServices.BusinessEntities.FibrePOEntities
{
	public class FiberSalesDC
	{
		public long DCId { get; set; }
		public string DCNo { get; set; }
		public long PartyId { get; set; }
		public long PartyName { get; set; }
		public string DCDate { get; set; }
		public List<FiberSalesDCDetails> SalesDCDetails { get; set; }
		public long CreatedByUserId { get; set; }
	}

	public class FiberSalesDCDetails
	{
		public long DCDtsId { get; set; }
		public long FiberTypeId { get; set; }
		public long FiberShadeId { get; set; }
		public long BlendId { get; set; }
		public long ProductionWasteDtsId { get; set; }
		public long WasteCategoryId { get; set; }
		public long MixedForYarnShadeId { get; set; }
		public double Quantity { get; set; }
		public long WasteStockId { get; set; }
		//public double Rate { get; set; }
	}
}
