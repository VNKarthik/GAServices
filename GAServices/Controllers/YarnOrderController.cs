using GAServices.BusinessEntities.Conversion;
using GAServices.BusinessEntities.FibrePOEntities;
using GAServices.BusinessEntities.YarnSales;
using GAServices.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GAServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YarnOrderController : ControllerBase
    {
        private readonly IYarnOrderRepository _yarnOrderRepository;
        private readonly ICommonRepository _commonRepository;

        public YarnOrderController(IYarnOrderRepository yarnOrderRepository, ICommonRepository commonRepository)
        {
            _yarnOrderRepository = yarnOrderRepository;
            _commonRepository = commonRepository;
        }

        [HttpPost("ReceiveYarnOrder")]
        public IActionResult ReceiveYarnOrder(YarnOrder order)
        {
                bool isReceived = _yarnOrderRepository.ReceiveYarnOrder(order);

                if (isReceived)
                    return Ok("YarnOrder Received Successfully");
                else
                    return Ok("Not able to receive the YarnOrder");
        }

        [HttpGet("GetYarnOrderListByParty")]
        public List<YarnOrder> GetYarnOrderListByParty(long partyId)
        {
            return _yarnOrderRepository.GetYarnOrderListByParty(partyId);
        }

        [HttpGet("GetYarnOrderListByDate")]
        public List<YarnOrder> GetYarnOrderListByDate(string fromDate, string toDate)
        {
            return _yarnOrderRepository.GetYarnOrderListByDate(fromDate, toDate);
        }

        [HttpGet("GetYarnOrderDetailsById")]
        public YarnOrder GetYarnOrderDetailsById(long orderId)
        {
            return _yarnOrderRepository.GetYarnOrderDetailsById(orderId);
        }


        [HttpPut("UpdateYarnOrder")]
        public IActionResult UpdateYarnOrder(YarnOrder order)
        {
            bool isReceived = _yarnOrderRepository.UpdateYarnOrder(order);

            if (isReceived)
                return Ok("YarnOrder Updated Successfully");
            else
                return Ok("Not able to update the YarnOrder");
        }

        [HttpPut("CloseYarnOrder")]
        public IActionResult CloseYarnOrder(long orderId, long closedByUserId)
        {
            bool isReceived = _yarnOrderRepository.CloseYarnOrder(orderId, closedByUserId);

            if (isReceived)
                return Ok("YarnOrder Closed Successfully");
            else
                return Ok("Not able to Close the YarnOrder");
        }

        [HttpPut("ReOpenYarnOrder")]
        public IActionResult ReOpenYarnOrder(long orderId, long reopenedByUserId)
        {
            bool isReceived = _yarnOrderRepository.ReOpenYarnOrder(orderId, reopenedByUserId);

            if (isReceived)
                return Ok("YarnOrder Reopened Successfully");
            else
                return Ok("Not able to reopen the YarnOrder");
        }

        [HttpGet("GetOrdersPendingDelivery")]
        public List<OrdersPendingDelivery> GetOrdersPendingDelivery(long partyId)
        {
            return _yarnOrderRepository.GetOrdersPendingDelivery(partyId);
        }

        [HttpGet("GetYarnStockByOrderId")]
        public List<YarnStockByOrderId> GetYarnStockByOrderId(long orderId)
        {
            return _yarnOrderRepository.GetYarnStockByOrderId(orderId);
        }

        [HttpPost("CreateYarnDelivery")]
        public YarnDC CreateYarnDelivery(CreateYarnDelivery delivery)
        {
            //string dcNo = _yarnOrderRepository.CreateYarnDelivery(delivery);
            long dcId = _yarnOrderRepository.CreateYarnDelivery(delivery);

            YarnDC dcDts = _yarnOrderRepository.GetYarnDCDetailsById(dcId);

            return dcDts;// dcNo;
        }

        [HttpGet("GetYarnDeliveries")]
        public List<YarnDeliverySummary> GetYarnDeliveries(string fromDate, string toDate, long partyId)
        {
            return _yarnOrderRepository.GetYarnDeliveries(fromDate, toDate, partyId);
        }


        [HttpGet("GetYarnDeliveriesByOrderId")]
        public List<YarnDeliverySummary> GetYarnDeliveriesByOrderId(long orderId)
        {
            return _yarnOrderRepository.GetYarnDeliveriesByOrderId(orderId);
        }

        [HttpGet("GetYarnDCDetailsById")]
        public YarnDC GetYarnDCDetailsById(long dcId)
        {
            return _yarnOrderRepository.GetYarnDCDetailsById(dcId);
        }

        [HttpGet("OrdersPendingInvoiceByPartyId")]
        public List<OrdersPendingInvoice> OrdersPendingInvoiceByParty(long partyId)
        {
            return _yarnOrderRepository.OrdersPendingInvoiceByPartyId(partyId);
        }

        [HttpGet("DCsPendingInvoiceByOrderId")]
        public List<DCsPendingInvoice> DCsPendingInvoiceByOrderId(long orderId)
        {
            return _yarnOrderRepository.DCsPendingInvoiceByOrderId(orderId);
        }

        [HttpPost("CreateYarnOrderInvoice")]
        public YarnInvoice CreateYarnOrderInvoice(CreateYarnInvoice yarnInvoice)
        {
            long invoiceId = _yarnOrderRepository.CreateYarnOrderInvoice(yarnInvoice);

            YarnInvoice invoiceDts = _yarnOrderRepository.GetYarnOrderInvoiceDtsById(invoiceId);

            return invoiceDts;
        }

        [HttpGet("GetYarnOrderInvoiceDtsById")]
        public YarnInvoice GetYarnOrderInvoiceDtsById(long invoiceId)
        {
            return _yarnOrderRepository.GetYarnOrderInvoiceDtsById(invoiceId);
        }

        [HttpGet("GetYarnOrderInvoiceDCDtsById")]
        public YarnInvoice GetYarnOrderInvoiceDCDtsById(long invoiceId)
        {
            return _yarnOrderRepository.GetYarnOrderInvoiceDCDtsById(invoiceId);
        }
    }
}
