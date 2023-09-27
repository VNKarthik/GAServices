namespace GAServices.BusinessEntities.Conversion
{
    public class YarnConversionProgram
    {
        public long ProgramId { get; set; }
        public string ProgramNo { get; set; }
        public string ProgramDate { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
    }

    public class ConversionProgram : YarnConversionProgram
    {
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsClosed { get; set; }
        public long CreatedByUserId { get; set; }
        public long ClosedByUserId { get; set; }

        public List<ConversionYarn> YarnCounts { get; set; }

        public List<ProgramFibersMixed> MixingDetails { get; set; }
    }

    public class ConversionYarn
    {
        public long ConversionYarnId { get; set; }
        public long CountsId { get; set; }
        public string Counts { get; set; }
        //public double Quantity { get; set; }
        public double ProgramQuantity { get; set; }
        public double ProductionQuantity { get; set; }
    }

    public class ProgramForMixing
    {
        public long ProgramId { get; set; }
        public string ProgramNo { get; set; }
        public string ProgramDate { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public string YarnCounts { get; set; }
        public double PlannedQuantity { get; set; }
        public double ProducedQuantity { get; set; }
    }

    public class ProgramForProductionEntry
    {
        public long ProgramId { get; set; }
        public string ProgramNo { get; set; }
        public string ProgramDate { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public string YarnCounts { get; set; }
        public double PlannedQuantity { get; set; }
        public double ProducedQuantity { get; set; }
        public long MixingId { get; set; }
        public string MixingDate { get; set; }
        public double MixedQuantity { get; set; }
    }

    public class ProductionEntry
    {
        public long ProgramId { get; set; }
        public long MixingId { get; set; }
        public string ProductionDate { get; set; }
        public List<ProductionYarn> YarnDetails { get; set; }
        public long CreatedByUserId { get; set; }
    }

    public class ProductionYarn
    {
        public long CountsId { get; set; }
        public string Lot { get; set; }
        public double ProductionQuantity { get; set; }
        public bool IsWinded { get; set; }
    }

    public class ConversionProgramWaste : ConversionProgram
    {
        public List<ProgramWaste> ProgramWaste { get; set; }
    }

    public class ProgramWaste
    {
        public long WasteCategoryId { get; set; }
        public string WasteCategoryName { get; set; }
        public double WasteQuantity { get; set; }
    }

    public class ProgramFibersMixed
    {
        public long FiberCategoryid { get; set; }
        public string FiberCategory { get; set; }
        public long FiberTypeId { get; set; }
        public string FiberType { get; set; }
        public long FiberShadeId { get; set; }
        public string FiberShade { get; set; }
        public string ReceivedDCNo { get; set; }
        public string ReceivedDate { get; set; }
        public string Lot { get; set; }
        public double Rate { get; set; }
        public double IssuedQuantity { get; set; }
    }

    public class YarnRecoverySummary
    {
        public long ProgramId { get; set; }
        public string ProgramNo { get; set; }
        public string ProgramDate { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public double PlannedQty { get; set; }
        public double MixedQuantity { get; set; }
        public string YarnLot { get; set; }
        public double ProductionQty { get; set; }
        public double WasteQuantity { get; set; }
        public double YarnRecoveryPercent { get; set; }
    }

    public class ConversionProgramStatus : YarnConversionProgram
    {
        public long CountsId { get; set; }
        public string Counts { get; set; }
        public double ProgramQty { get; set; }
        public double ProductionQuantity { get; set; }
        public double DeliveredQuantity { get; set; }
        public double StockAvailable { get; set; }
    }

    public class ProgramWasteStock
    {
        public string ProgramNo { get; set; }
        public string ProgramDate { get; set; }
        public string ShadeName { get; set; }
        public string BlendName { get; set;}
        public string WasteCategoryName { get; set;}
        public long WasteCategoryId { get; set; }
        public double WasteQuantity { get; set; }
        public double StockQuantity { get; set; }
    }
}
