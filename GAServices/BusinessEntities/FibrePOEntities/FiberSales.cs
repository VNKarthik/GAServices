using System.Security.Policy;

namespace GAServices.BusinessEntities.FibrePOEntities
{
	public class FiberSalesDCMaster
	{
		public long DCId { get; set; }
		public string DCNo { get; set; }
		public long PartyId { get; set; }
		public long PartyName { get; set; }
		public string DCDate { get; set; }
		
		public long CreatedByUserId { get; set; }
	}

	public class FiberSalesDC // : FiberSalesDCMaster
	{
		public long DCId { get; set; }
		public string DCNo { get; set; }
		public long PartyId { get; set; }
		public string PartyName { get; set; }
		public string DCDate { get; set; }
		public List<FiberSalesDCDetails> SalesDCDetails { get; set; }
		public long CreatedByUserId { get; set; }
	}

	public class FiberSalesDCDetails
	{
		public long DCId { get; set; }
		public long DCDtsId { get; set; }
		public long FiberTypeId { get; set; }
		public long FiberTypeName { get; set; }
		public long FiberShadeId { get; set; }
		public long FiberShadeName { get; set; }
		public long BlendId { get; set; }
		public long BlendName { get; set; }
		public long ProductionWasteDtsId { get; set; }
		public long WasteCategoryId { get; set; }
		public string WasteCategoryName { get; set; }
		public long MixedForYarnShadeId { get; set; }
		public string YarnShade { get; set; }
		public double Quantity { get; set; }
		public long WasteStockId { get; set; }
		//public double Rate { get; set; }
	}
}
