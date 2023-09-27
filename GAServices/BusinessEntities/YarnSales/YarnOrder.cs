using System.Diagnostics.Eventing.Reader;

namespace GAServices.BusinessEntities.YarnSales
{
    public class YarnOrder
    {
        public long OrderId { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string ReceivedDate { get; set; }
        public long PartyId { get; set; }
        public long DeliveryPartyId { get; set; }
        public double DueDays { get; set; }
        public string BrokerName { get; set; }
        public List<YarnOrderDetails> OrderDts { get; set; }
        public long ReceivedByUserId { get; set; }
        public string Remarks { get; set; }
    }

    public class YarnOrderDetails
    {
        public long OrderDetailId { get; set; }
        public long CountsId { get; set; }
        public string Counts { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public double OrderQuantity { get; set; }
        public double Rate { get; set; }
        public double GSTPercent { get; set; }
    }

    public class OrdersPendingDelivery
    {
        public long OrderId { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public long PartyId { get; set; }
        public string PartyName { get; set; }
        public long DeliveryPartyId { get; set; }
        public string DeliveryPartyName { get; set; }
        public long OrderDtsId { get; set; }
        public long CountsId { get; set; }
        public string Counts { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public double OrderQuantity { get; set; }
        public double Rate { get; set; }
        public double GSTPercent { get; set; }
        public double DeliveredQuantity { get; set; }
        public double BalanceQuantity { get; set; }
    }

    public class YarnStockByOrderId
    {
        public long ProgramId { get; set; }
        public string ProgramNo { get; set; }
        public string ProgramDate { get; set; }
        public long OrderDtsId { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public long CountsId { get; set; }
        public string Counts { get; set; }
        public long ProductionYarnDtsId { set; get; }
        public long YarnReturnDtsId { set; get; }
        public string Lot { get; set; }
        public double ProductionQuantity { get; set; }
        public double StockQuantity { get; set; }
    }

    public class CreateYarnDelivery
    {
        //public long DCId { get; set; }
        public string DCNo { get; set; }
        public string DeliveryDate { get; set; }
        public long OrderId { get; set; }
        public long DeliveryPartyId { get; set; }
        public long DeliveryAddressId { get; set; }
        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
        public string EWayBillNo { get; set; }
        public long CreatedByUserId { get; set; }

        public List<CreateYarnDeliveryDts> DeliveryDts { get; set; }
    }

    public class CreateYarnDeliveryDts
    {
        public long DeliveryDtsId { get; set; }
        public long OrderDtsId { get; set; }
        public long ShadeId { get; set; }
        public long BlendId { get; set; }
        public long CountsId { get; set; }
        public long ProductionDtsId { get; set; }
        public long YarnReturnDtsId { set; get; }
        public double DeliveryQuantity { get; set; }
        public long HSNCode { get; set; }
    }

    public class YarnDeliverySummary
    {
        public long DCId { get; set; }
        public string DCNo { get; set; }
        public string DCDate { get; set; }
        public long PartyId { get; set; }
        public string PartyName { get; set; }
        public long OrderId { get; set; }
        public string OrderNo { get; set; }
        public long OrderDtsId { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public long CountsId { get; set; }
        public string Counts { get; set; }
        public double OrderQuantity { get; set; }
        public double DeliveredQuantity { get; set; }
        public long HSNCode { get; set; }
        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
        public string EWayBillNo { get; set; }
        public long CreatedByUserId { get; set; }

    }

    public class YarnDC
    {
        public long DCId { get; set; }
        public string DCNo { get; set; }
        public string DCDate { get; set; }
        public long PartyId { get; set; }
        public string PartyName { get; set; }
        public string BranchName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string PinCode { get; set; }
        public string StateName { get; set; }
        public Int32 StateCode { get; set; }
        public string GSTNo { get; set; }
        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
        public long DeliveryPartyId { get; set; }
        public string DeliveryPartyName { get; set; }
        public string DeliveryBranchName { get; set; }
        public string DeliveryAddress1 { get; set; }
        public string DeliveryAddress2 { get; set; }
        public string DeliveryAddress3 { get; set; }
        public string DeliveryDistrictName { get; set; }
        public string DeliveryCityName { get; set; }
        public string DeliveryPinCode { get; set; }
        public string DeliveryStateName { get; set; }
        public Int32 DeliveryStateCode { get; set; }
        public string DeliveryGSTNo { get; set; }
        public string EWayBillNo { get; set; }

        public List<YarnDCDetails> YarnDetails { get; set; }
    }

    public class YarnDCDetails
    {
        public long DCDtsId { get; set; }
        public long OrderDtsId { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public long CountsId { get; set; }
        public string Counts { get; set; }
        public string Lot { get; set; }
        public long HSNCode { get; set; }
        public double OrderQuantity { get; set; }
        public double DeliveredQuantity { get; set; }
        public double PreviousDeliveredQuantity { get; set; }
    }

    public class OrdersPendingInvoice
    {
        public long OrderId { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public long OrderDtsId { get; set; }
        public string PartyName { get; set; }
        public string ShadeName { get; set; }
        public string BlendName { get; set; }
        public string Counts { get; set; }
        public double OrderQuantity { get; set; }
        public double DeliveredQuantity { get; set; }
        public double InvoicedQuantity { get; set; }
        public double PendingQuantity { get; set; }
    }

    public class DCsPendingInvoice
    {
        public long DCId { get; set; }
        public string DCNo { get; set; }
        public string DCDate { get; set; }
        public long OrderDtsId { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public long CountsId { get; set; }
        public string Counts { get; set; }
        public double DeliveredQuantity { get; set; }
        public long HSNCode { get; set; }
        public string VehicleNo { get; set; }
        public string Remarks { get; set; }
        public double Rate { get; set; }
        public double GSTPercent { get; set; }
    }

    public class CreateYarnInvoice
    {
        public string InvoiceDate { get; set; }
        public long PartyId { get; set; }
        public List<CreateYarnInvoiceDts> CreateYarnInvoiceDts { get; set; }
        public long CreatedByUserId { get; set; }
    }

    public class CreateYarnInvoiceDts
    {
        public long DCId { get; set; }
        public long OrderDtsId { get; set; }
        public double Rate { get; set; }
        public double GSTPercent { get; set; }
    }

    public class YarnInvoice
    {
        public long InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public long PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress1 { get; set; }
        public string PartyAddress2 { get; set; }
        public string PartyAddress3 { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public long StateCode { get; set; }
        public string PartyPinCode { get; set; }
        public string PartyGSTNo { get; set; }
        public List<InvoiceDts> InvoiceDts { get; set; }
        public List<InvoiceDCDts> InvoiceDCDts { get; set; }
    }

    public class InvoiceDts
    {
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public long CountsId { get; set; }
        public string Counts { get; set; }
        public double InvoiceQty { get; set; }
        public double Rate { get; set; }
        public double GSTPercent { get; set; }
        public long HSNCode { get; set; }
    }

    public class InvoiceDCDts : InvoiceDts
    {
        public long DCId { get; set; }
        public string DCNo { get; set; }
        public string DCDate { get; set; }
        public long OrderDtsId { get; set; }
        public double DeliveryQty { get; set; }
        public long HSNCode { get; set; }
    }

    public class YarnReturn
    {
        public long ReturnId { get; set; }
        public string ReturnDate { get; set; }
        public string ReturnDCNo { get; set; }
        public long PartyNo { get; set; }
        public string ReturnReason { get; set; }
        public string Remarks { get; set; }

        public List<YarnReturnDetails> YarnReturnDetails { get; set; }
    }

    public class YarnReturnDetails
    {
        public long ReturnDtsId { get; set; }
        public long IssuedDCId { get; set; }
        public string IssuedDCNo { get; set; }
        //public string InvoiceNo { get; set; }
        public long CountsId { get; set; }
        public string Counts { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public string Lot { get; set; }
        public double IssuedQty { get; set; }
        public double ReturnQty { get; set; }
    }

    public class YarnDeliverySearchResult : YarnDCDetails
    {
        public long DCId { get; set; }
        public string DCNo { get; set; }
        public string DCDate { get; set; }
        public long PartyId { get; set; }
        public string PartyName { get; set; }
    }

    public class YarnStock
    {
        public string ProductionDate { get; set; }
        public string ReturnDate { get; set; }
        public string ReturnDCNo { get; set; }
        public long ShadeId { get; set; }
        public string ShadeName { get; set; }
        public long BlendId { get; set; }
        public string BlendName { get; set; }
        public long CountsId { get; set; }
        public string Counts { get; set; }
        public string Lot { get; set; }
        public double ProductionQuantity { get; set; }
        public double ReturnQuantity { get; set; }
        public double StockQuantity { get; set; }
    }
}
