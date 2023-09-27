using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Serialization;
using GAServices.BusinessEntities.Conversion;
using GAServices.BusinessEntities.FibrePOEntities;
using GAServices.BusinessEntities.Party;
using GAServices.BusinessEntities.YarnSales;
using GAServices.Common;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509;
//using static Google.Protobuf.WireFormat;

namespace GAServices.Repositories
{
    public interface IYarnOrderRepository
    {
        public bool ReceiveYarnOrder(YarnOrder order);

        public List<YarnOrder> GetYarnOrderListByParty(long partyId);

        public List<YarnOrder> GetYarnOrderListByDate(string fromDate, string toDate);

        public YarnOrder GetYarnOrderDetailsById(long orderId);

        public bool UpdateYarnOrder(YarnOrder order);

        public bool CloseYarnOrder(long orderId, long closedByUserId);

        public bool ReOpenYarnOrder(long orderId, long openedByUserId);

        public List<OrdersPendingDelivery> GetOrdersPendingDelivery(long partyId);

        public List<YarnStockByOrderId> GetYarnStockByOrderId(long partyId);

        //public string CreateYarnDelivery(CreateYarnDelivery delivery);
        public long CreateYarnDelivery(CreateYarnDelivery delivery);

        public List<YarnDeliverySummary> GetYarnDeliveries(string fromDate, string toDate, long partyId);

        public List<YarnDeliverySummary> GetYarnDeliveriesByOrderId(long orderId);

        public YarnDC GetYarnDCDetailsById(long dcId);

        public List<OrdersPendingInvoice> OrdersPendingInvoiceByPartyId(long partyId);

        public List<DCsPendingInvoice> DCsPendingInvoiceByOrderId(long orderId);

        public long CreateYarnOrderInvoice(CreateYarnInvoice yarnInvoice);

        public YarnInvoice GetYarnOrderInvoiceDtsById(long invoiceId);

        public YarnInvoice GetYarnOrderInvoiceDCDtsById(long invoiceId);

        public List<YarnDeliverySearchResult> SearchYarnDeliveries(long partyId, long countsId, long blendId, long shadeId);

        public bool ReceiveYarnReturn(YarnReturn yarnReturn, long createdUserId);

        public List<YarnStock> GetYarnCurrentStock();

        public List<YarnStock> GetYarnStock(string asOnDate, long blendId, long shadeId, string lot);

        public bool UpdateEInvoiceNo(string eInvoiceNo, long invoiceId);

    }

    public class YarnOrderRepository : IYarnOrderRepository
    {
        private readonly DataAccess _dataAccess;

        public YarnOrderRepository()
        {
            _dataAccess = new DataAccess();
        }

        public bool ReceiveYarnOrder(YarnOrder order)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pPartyId", order.PartyId));
            inParam.Add(new MySqlParameter("pDeliveryPartyId", order.DeliveryPartyId));
            inParam.Add(new MySqlParameter("pOrderNo", order.OrderNo));
            inParam.Add(new MySqlParameter("pOrderDate", order.OrderDate));
            inParam.Add(new MySqlParameter("pReceivedDate", order.ReceivedDate));
            inParam.Add(new MySqlParameter("pDueDays", order.DueDays));
            inParam.Add(new MySqlParameter("pBrokerName", order.BrokerName));
            inParam.Add(new MySqlParameter("pOrderDts", order.OrderDts.GetXmlString()));
            inParam.Add(new MySqlParameter("pRemarks", order.Remarks));
            inParam.Add(new MySqlParameter("pUserId", order.ReceivedByUserId.ToString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pRecordsInserted", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("ReceiveYarnOrder", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    if (response.ReturnData["pRecordsInserted"].ToLong() > 0)
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }

        public List<YarnOrder> GetYarnOrderListByParty(long partyId)
        {
            List<YarnOrder> yarnOrders;
            DataTable dtOrderDetails;
            DataSet dsYarnOrders = _dataAccess.DB.GetDataSet("GetYarnOrdersByParty", new List<MySqlParameter>() { new MySqlParameter("pPartyId", partyId) });

            if (dsYarnOrders.Tables.Count > 0)
            {
                yarnOrders = Utilities.CreateListFromTable<YarnOrder>(dsYarnOrders.Tables[0]);

                dtOrderDetails = dsYarnOrders.Tables[1];

                foreach (YarnOrder yo in yarnOrders)
                {
                    dtOrderDetails.DefaultView.RowFilter = "OrderId = " + yo.OrderId;
                    yo.OrderDts = Utilities.CreateListFromTable<YarnOrderDetails>(dtOrderDetails.DefaultView.ToTable());
                }
            }
            else
                return null;

            return yarnOrders;
        }

        public List<YarnOrder> GetYarnOrderListByDate(string fromDate, string toDate)
        {
            List<YarnOrder> yarnOrders;
            DataTable dtOrderDetails;
            DataSet dsYarnOrders = _dataAccess.DB.GetDataSet("GetYarnOrdersByDate", new List<MySqlParameter>() { new MySqlParameter("pFromDate", fromDate), new MySqlParameter("pToDate", toDate) });

            if (dsYarnOrders.Tables.Count > 0)
            {
                yarnOrders = Utilities.CreateListFromTable<YarnOrder>(dsYarnOrders.Tables[0]);

                dtOrderDetails = dsYarnOrders.Tables[1];

                foreach (YarnOrder yo in yarnOrders)
                {
                    dtOrderDetails.DefaultView.RowFilter = "OrderId = " + yo.OrderId;
                    yo.OrderDts = Utilities.CreateListFromTable<YarnOrderDetails>(dtOrderDetails.DefaultView.ToTable());
                }
            }
            else
                return null;

            return yarnOrders;
        }

        public YarnOrder GetYarnOrderDetailsById(long orderId)
        {
            YarnOrder yarnOrder;
            DataTable dtOrderDetails;
            DataSet dsYarnOrders = _dataAccess.DB.GetDataSet("GetYarnOrdersById", new List<MySqlParameter>() { new MySqlParameter("pOrderId", orderId) });

            if (dsYarnOrders.Tables.Count > 0)
            {
                yarnOrder = Utilities.CreateListFromTable<YarnOrder>(dsYarnOrders.Tables[0]).FirstOrDefault();

                dtOrderDetails = dsYarnOrders.Tables[1];

                dtOrderDetails.DefaultView.RowFilter = "OrderId = " + yarnOrder.OrderId;
                yarnOrder.OrderDts = Utilities.CreateListFromTable<YarnOrderDetails>(dtOrderDetails.DefaultView.ToTable());
            }
            else
                return null;

            return yarnOrder;
        }

        public bool UpdateYarnOrder(YarnOrder order)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pOrderId", order.OrderId));
            inParam.Add(new MySqlParameter("pPartyId", order.PartyId));
            inParam.Add(new MySqlParameter("pOrderNo", order.OrderNo));
            inParam.Add(new MySqlParameter("pOrderDate", order.OrderDate));
            inParam.Add(new MySqlParameter("pReceivedDate", order.ReceivedDate));
            inParam.Add(new MySqlParameter("pDueDays", order.DueDays));
            inParam.Add(new MySqlParameter("pBrokerName", order.BrokerName));
            inParam.Add(new MySqlParameter("pOrderDts", order.OrderDts.GetXmlString()));
            inParam.Add(new MySqlParameter("pRemarks", order.Remarks));
            inParam.Add(new MySqlParameter("pUserId", order.ReceivedByUserId.ToString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pRecordsUpdated", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("UpdateYarnOrder", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    if (response.ReturnData["pRecordsUpdated"].ToLong() > 0)
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }

        public bool CloseYarnOrder(long orderId, long closedByUserId)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pOrderId", orderId));
            inParam.Add(new MySqlParameter("pUserId", closedByUserId));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pRecordsUpdated", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("CloseYarnOrder", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    if (response.ReturnData["pRecordsUpdated"].ToLong() > 0)
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }

        public bool ReOpenYarnOrder(long orderId, long openedByUserId)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pOrderId", orderId));
            inParam.Add(new MySqlParameter("pUserId", openedByUserId));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pRecordsUpdated", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("ReOpenYarnOrder", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    if (response.ReturnData["pRecordsUpdated"].ToLong() > 0)
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }

        public List<OrdersPendingDelivery> GetOrdersPendingDelivery(long partyId)
        {
            List<OrdersPendingDelivery> yarnOrders;

            yarnOrders = _dataAccess.DB.GetData<OrdersPendingDelivery>("GetOrdersPendingDelivery", new List<MySqlParameter>() { new MySqlParameter("pPartyId", partyId) });

            return yarnOrders;
        }

        public List<YarnStockByOrderId> GetYarnStockByOrderId(long orderId)
        {
            List<YarnStockByOrderId> yarnOrders = null;

            yarnOrders = _dataAccess.DB.GetData<YarnStockByOrderId>("GetYarnStockByOrderId", new List<MySqlParameter>() { new MySqlParameter("pOrderId", orderId) });

            return yarnOrders;
        }

        /// <summary>
        /// Returns Delivery DC No
        /// </summary>
        /// <param name="delivery"></param>
        /// <returns></returns>
        //public string CreateYarnDelivery(CreateYarnDelivery delivery)
        public long CreateYarnDelivery(CreateYarnDelivery delivery)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            //inParam.Add(new MySqlParameter("pOrderDtsId", delivery.OrderDtsId));
            inParam.Add(new MySqlParameter("pDeliveryDate", delivery.DeliveryDate));
            inParam.Add(new MySqlParameter("pDeliveryPartyId", delivery.DeliveryPartyId));
            inParam.Add(new MySqlParameter("pDeliveryAddressId", delivery.DeliveryAddressId));
            inParam.Add(new MySqlParameter("pDeliveryDts", delivery.DeliveryDts.GetXmlString()));
            inParam.Add(new MySqlParameter("pRemarks", delivery.Remarks));
            inParam.Add(new MySqlParameter("pEWayBillNo", delivery.EWayBillNo));
            inParam.Add(new MySqlParameter("pVehicleNo", delivery.VehicleNo));
            inParam.Add(new MySqlParameter("pUserId", delivery.CreatedByUserId.ToString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pDCId", MySqlDbType.VarChar));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("CreateYarnDelivery", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    if (response.ReturnData["pDCId"].GetStringValue() != "")
                        //return response.ReturnData["pDCId"].GetStringValue();
                        return response.ReturnData["pDCId"].ToLong();
                    else
                        return 0; // "";
                }
            }

            return 0; // "";
        }

        public List<YarnDeliverySummary> GetYarnDeliveries(string fromDate, string toDate, long partyId)
        {
            return _dataAccess.DB.GetData<YarnDeliverySummary>("GetYarnDeliveries", new List<MySqlParameter>() { new MySqlParameter("pFromDate", fromDate), new MySqlParameter("pToDate", toDate), new MySqlParameter("pPartyId", partyId) });
        }

        public List<YarnDeliverySummary> GetYarnDeliveriesByOrderId(long orderId)
        {
            return _dataAccess.DB.GetData<YarnDeliverySummary>("GetYarnDeliveriesByOrderId", new List<MySqlParameter>() { new MySqlParameter("pOrderId", orderId) });
        }

        public YarnDC GetYarnDCDetailsById(long dcId)
        {
            YarnDC yarnDC;

            yarnDC = _dataAccess.DB.GetData<YarnDC>("GetYarnDCById", new List<MySqlParameter>() { new MySqlParameter("pDCId", dcId) }).FirstOrDefault();

            if (yarnDC != null)
                yarnDC.YarnDetails = _dataAccess.DB.GetData<YarnDCDetails>("GetYarnDCDetailsById", new List<MySqlParameter>() { new MySqlParameter("pDCId", dcId) });

            return yarnDC;
        }

        public List<OrdersPendingInvoice> OrdersPendingInvoiceByPartyId(long partyId)
        {
            List<OrdersPendingInvoice> ordersPendingInvoiceYarnDC;

            ordersPendingInvoiceYarnDC = _dataAccess.DB.GetData<OrdersPendingInvoice>("GetOrdersPendingInvoiceByParty", new List<MySqlParameter>() { new MySqlParameter("pPartyId", partyId) });

            return ordersPendingInvoiceYarnDC;
        }

        public List<DCsPendingInvoice> DCsPendingInvoiceByOrderId(long orderId)
        {
            List<DCsPendingInvoice> ordersPendingInvoice;

            ordersPendingInvoice = _dataAccess.DB.GetData<DCsPendingInvoice>("GetDCsPendingInvoiceByOrder", new List<MySqlParameter>() { new MySqlParameter("pOrderId", orderId) });

            return ordersPendingInvoice;
        }

        public long CreateYarnOrderInvoice(CreateYarnInvoice yarnInvoice)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pInvoiceDate", yarnInvoice.InvoiceDate));
            inParam.Add(new MySqlParameter("pPartyId", yarnInvoice.PartyId));
            inParam.Add(new MySqlParameter("pDeliveryDts", yarnInvoice.CreateYarnInvoiceDts.GetXmlString()));
            inParam.Add(new MySqlParameter("pUserId", yarnInvoice.CreatedByUserId.ToString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pInvoiceId", MySqlDbType.VarChar));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("CreateYarnInvoice", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    if (response.ReturnData["pInvoiceId"].GetStringValue() != "")
                        return response.ReturnData["pInvoiceId"].ToLong();
                    else
                        return 0;
                }
            }

            return 0;
        }

        public YarnInvoice GetYarnOrderInvoiceDtsById(long invoiceId)
        {
            YarnInvoice? yarnInvoiceDts;

            yarnInvoiceDts = _dataAccess.DB.GetData<YarnInvoice>("GetYarnOrderInvoiceById", new List<MySqlParameter>() { new MySqlParameter("pInvoiceId", invoiceId) }).FirstOrDefault();

            if (yarnInvoiceDts != null)
                yarnInvoiceDts.InvoiceDts = _dataAccess.DB.GetData<InvoiceDts>("GetYarnOrderInvoiceDtsById", new List<MySqlParameter>() { new MySqlParameter("pInvoiceId", invoiceId) });

            return yarnInvoiceDts;
        }

        public YarnInvoice GetYarnOrderInvoiceDCDtsById(long invoiceId)
        {
            YarnInvoice? yarnInvoiceDts;

            yarnInvoiceDts = _dataAccess.DB.GetData<YarnInvoice>("GetYarnOrderInvoiceById", new List<MySqlParameter>() { new MySqlParameter("pInvoiceId", invoiceId) }).FirstOrDefault();

            if (yarnInvoiceDts != null)
                yarnInvoiceDts.InvoiceDCDts = _dataAccess.DB.GetData<InvoiceDCDts>("GetYarnOrderInvoiceDCDtsById", new List<MySqlParameter>() { new MySqlParameter("pInvoiceId", invoiceId) });

            return yarnInvoiceDts;
        }

        public List<YarnDeliverySearchResult> SearchYarnDeliveries(long partyId, long countsId, long blendId, long shadeId)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pPartyId", partyId));
            inParam.Add(new MySqlParameter("pCountsId", countsId));
            inParam.Add(new MySqlParameter("pBlendId", blendId));
            inParam.Add(new MySqlParameter("pShadeId", shadeId));

            return _dataAccess.DB.GetData<YarnDeliverySearchResult>("SearchYarnDeliveries", inParam);
        }

        public bool ReceiveYarnReturn(YarnReturn yarnReturn, long createdUserId)
        {
            List<YarnReturnDetails> lstYarnDts = yarnReturn.YarnReturnDetails;

            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pReturnDCNo", yarnReturn.ReturnDCNo));
            inParam.Add(new MySqlParameter("pPartyId", yarnReturn.PartyNo.ToString()));
            inParam.Add(new MySqlParameter("pReturnDate", yarnReturn.ReturnDate));
            //inParam.Add(new MySqlParameter("pIssuedDCNo", yarnReturn.IssuedDCNo));
            //inParam.Add(new MySqlParameter("pInvoiceNo", yarnReturn.InvoiceNo));
            inParam.Add(new MySqlParameter("pReturnReason", yarnReturn.ReturnReason));
            inParam.Add(new MySqlParameter("pRemarks", yarnReturn.Remarks));
            inParam.Add(new MySqlParameter("pReturnDts", lstYarnDts.GetXmlString()));
            inParam.Add(new MySqlParameter("pUserId", createdUserId.ToString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pYarnReturnId", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("ReceiveYarnReturn", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    if (response.ReturnData["pYarnReturnId"].ToLong() > 0)
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }

        public List<YarnStock> GetYarnCurrentStock()
        {
            return GetYarnStock("", 0, 0, "");
        }

        public List<YarnStock> GetYarnStock(string asOnDate, long blendId, long shadeId, string lot)
        {
            List<MySqlParameter> lstInParams = new List<MySqlParameter>();

            lstInParams.Add(new MySqlParameter("pAsOnDate", asOnDate));
            lstInParams.Add(new MySqlParameter("pBlendId", blendId));
            lstInParams.Add(new MySqlParameter("pShadeId", shadeId));
            lstInParams.Add(new MySqlParameter("pLot", lot));

            return _dataAccess.DB.GetData<YarnStock>("GetYarnStock", lstInParams);
        }

        public bool UpdateEInvoiceNo(string eInvoiceNo, long invoiceId)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pEInvoiceNo", eInvoiceNo));
            inParam.Add(new MySqlParameter("pInvoiceId", invoiceId));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pRecordsUpdated", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("UpdateEInvoiceNo", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    if (response.ReturnData["pRecordsUpdated"].ToLong() > 0)
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }
    }
}
