using System.Diagnostics.Tracing;
using GAServices.BusinessEntities.FibrePOEntities;
using GAServices.BusinessEntities.Party;
using GAServices.Common;
using GAServices.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiberController : ControllerBase
    {
        private readonly IFibreRepository _fibreRepository;
        private readonly ICommonRepository _commonRepository;

        public FiberController(IFibreRepository fibreRepository, ICommonRepository commonRepository)
        {
            _fibreRepository = fibreRepository;
            _commonRepository = commonRepository;
        }

        [HttpGet("GetFibreTypes")]
        public ActionResult<IEnumerable<FiberType>> GetFibreTypes()
        {
            try
            {
                return _fibreRepository.GetFibreTypes();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetActiveFibreTypes")]
        public ActionResult<IEnumerable<FiberType>> GetActiveFibreTypes()
        {
            try
            {
                return _fibreRepository.GetActiveFibreTypes();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetFibreCategories")]
        public ActionResult<List<FibreCategory>> GetFibreCategories()
        {
            return _fibreRepository.GetFibreCategories();
        }

        [HttpPost("AddFibreType")]
        //public async Task<ActionResult<FiberType>> AddFibreType(string fibreTypeName)
        public async Task<ActionResult<FiberType>> AddFibreType(FiberType fibreType)
        {
            //try
            //{
            //long newFibreTypeId = _fibreRepository.AddFibreType(fibreTypeName);
            long newFibreTypeId = _fibreRepository.AddFibreType(fibreType);

            return CreatedAtAction("GetFibreTypes", newFibreTypeId, newFibreTypeId);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            //}
        }

        [HttpGet("GetFibreShades")]
        public ActionResult<IEnumerable<FibreShade>> GetFibreShades()
        {
            try
            {
                return _fibreRepository.GetFibreShades();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetActiveFibreShades")]
        public ActionResult<IEnumerable<FibreShade>> GetActiveFibreShades()
        {
            try
            {
                return _fibreRepository.GetActiveFibreShades();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddFibreShade")]
        public async Task<ActionResult<IEnumerable<FibreShade>>> AddFibreShade(string fibreShadeName)
        {
            try
            {
                long newFibreShadeId = _fibreRepository.AddFibreShade(fibreShadeName);

                return CreatedAtAction("GetFibreShades", newFibreShadeId, newFibreShadeId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("GetPONo")]
        public async Task<ActionResult<string>> GetPONo()
        {
            try
            {
                string poNo = _commonRepository.GeneratePONo(AUTOGEN_TYPE.FIBRE_PO);

                return poNo == null ? NotFound() : Ok(poNo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("CreateFibrePO")]
        public async Task<ActionResult<FibrePO>> CreateFibrePO([FromBody] CreateFibrePO model)
        //public Task<long> CreateFibrePO([FromBody] CreateFibrePO model)
        {
            try
            {
                long poId = _fibreRepository.CreatePO(model);

                return CreatedAtAction("GetPOById", new { poId = poId }, poId);
                //return Task.FromResult(poId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //[HttpGet("GetPOById/{poId}")]
        [HttpGet("GetPOById")]
        public ActionResult<FibrePO> GetPOById(long poId)
        {
            var po = _fibreRepository.GetPOById(poId);

            return po == null ? NotFound() : Ok(po);
        }

        [HttpGet("GetOpenPOCounts")]
        public async Task<ActionResult<long>> GetOpenPOCounts()
        {
            try
            {
                long poCounts;
                poCounts = await _fibreRepository.GetOpenPOCounts();

                return Ok(poCounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetPartywiseOpenPOCounts")]
        public async Task<ActionResult<IEnumerable<PartywisePOCounts>>> GetPartywiseOpenPOCounts()
        {
            List<PartywisePOCounts> po = await _fibreRepository.GetPartywiseOpenPOCounts();

            return po;
        }

        [HttpGet("PendingPODetailsByParty")]
        public async Task<ActionResult<IEnumerable<PendingPODtsByParty>>> PendingPODetailsByParty(long partyId)
        {
            List<PendingPODtsByParty> po = await _fibreRepository.PendingPODetailsByParty(partyId);

            return po;
        }

        [HttpPost("ReceiveFibre")]
        //public IActionResult ReceiveFibre([FromBody] ReceiveFibrePO model)
        public IActionResult ReceiveFibre(ReceiveFibrePO model)
        {
                bool isReceived = _fibreRepository.ReceivePOFibre(model);

                if (isReceived)
                    //return StatusCode(StatusCodes.Status202Accepted);
                    return Ok("Fibre Received Successfully");
                else
                    return BadRequest("Not able to receive the Fibre");
            
        }

        [HttpGet("GetTwelveMonthSummary")]
        public async Task<ActionResult<List<POSummaryFor12Months>>> GetTwelveMonthSummary()
        {
            try
            {
                List<POSummaryFor12Months> po = await _fibreRepository.Last12MonthSummary();

                if (po != null)
                    return po;
                else
                    return Ok("No Summary available");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetFibreStock")]
        public ActionResult<IEnumerable<FibreStock>> GetFibreStock()
        {
            return _fibreRepository.GetFibreStock();
        }

        [HttpGet("GetFibreStockForMixing")]
        public ActionResult<IEnumerable<FibreStock>> GetFibreStockForMixing(long programId)
        {
            return _fibreRepository.GetFibreStockForMixing(programId);
        }

        [HttpGet("GetFibreWasteCategories")]
        public ActionResult<IEnumerable<FibreWasteCategory>> GetFibreWasteCategories()
        {
            return _fibreRepository.GetFibreWasteCategories();
        }

        [HttpGet("GetFibreOrdersPendingToReceive")]
        public ActionResult<IEnumerable<FibrePO>> GetFibreOrdersPendingToReceive()
        {
            return _fibreRepository.GetFibreOrdersPendingToReceive();
        }

        [HttpGet("GetFiberPODetails_WitStatus")]
        public ActionResult<IEnumerable<FibrePO>> GetFiberPODetails_WitStatus(long partyId, string fromDate, string toDate)
        {
            return _fibreRepository.GetFiberPODetails_WitStatus(partyId, fromDate, toDate);
        }

        [HttpGet("GetFiberConsumptionByRecdDtsId")]
        public List<FiberIssueDetails> GetFiberConsumptionByRecdDtsId(long receivedDtsId)
        {
            return _fibreRepository.GetFiberConsumptionByRecdDtsId(receivedDtsId);
        }

		[HttpPost("SaveFiberWaste")]
		public IActionResult SaveFiberWaste(List<CreateFiberWaste> waste, long createdByUserId)
        {
			bool isSaved = _fibreRepository.SaveFiberWaste(waste, createdByUserId);

			if (isSaved)
				return Ok("Waste Stock Saved Successfully");
			else
				return BadRequest("Not able to save the Waste Stock");
		}

		[HttpGet("GetFibreStockSearch")]
        public ActionResult<IEnumerable<FibreStock>> GetFibreStockSearch(string asOnDate, long partyId, long fiberTypeId)
        {
            return _fibreRepository.GetFibreStockSearch(asOnDate, partyId, fiberTypeId);
        }

        [HttpGet("GetFibreWasteStock")]
		public ActionResult<IEnumerable<FiberWasteStock>> GetFibreWasteStock()
		{
			return _fibreRepository.GetFibreWasteStock();
		}

		[HttpPost("CreateWasteSalesDC")]
		public IActionResult CreateWasteSalesDC(FiberSalesDC salesDC)
		{
			bool isReceived = _fibreRepository.CreateWasteSalesDC(salesDC);

			if (isReceived)
				return Ok("Fibre Received Successfully");
			else
				return BadRequest("Not able to receive the Fibre");
		}

		[HttpGet("GetFiberWasteSalesByParty")]
		public ActionResult<IEnumerable<FiberSalesDC>> GetFiberWasteSalesByParty(long partyId, string fromDate, string toDate)
		{
			return _fibreRepository.GetFiberWasteSalesByParty(partyId, fromDate, toDate);
		}
	}
}
