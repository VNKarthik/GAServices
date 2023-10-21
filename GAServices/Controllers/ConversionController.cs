using System.Collections.Generic;
using GAServices.BusinessEntities.Conversion;
using GAServices.BusinessEntities.FibrePOEntities;
using GAServices.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionRepository _conversionRepository;
        private readonly ICommonRepository _commonRepository;

        public ConversionController(IConversionRepository conversionRepository, ICommonRepository commonRepository)
        {
            _conversionRepository = conversionRepository;
            _commonRepository = commonRepository;
        }

        [HttpPost("CreateConversionProgram")]
        public async Task<ActionResult<string>> CreateConversionProgram(ConversionProgram program)
        {
            //try
            //{
            string programNo = _conversionRepository.CreateConversionProgram(program);

            return programNo;
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            //}
        }


        [HttpGet("GetProgramsForMixing")]
        public List<ProgramForMixing> GetProgramsForMixing()
        {
            return _conversionRepository.ProgramsForMixing();
        }


        [HttpGet("GetProgramDetailsById")]
        public ConversionProgram GetProgramDetailsById(long programId)
        {
            return _conversionRepository.GetProgramDetailsById(programId);
        }

        [HttpPost("IssueFibreForMixing")]
        public async Task<ActionResult<string>> IssueFibreForMixing(FibreMixing fibres)
        {
            bool isIssuedSuccessfully = _conversionRepository.IssueFibreForMixing(fibres);

            if (isIssuedSuccessfully)
                return Ok("Successfully Saved the Mixing details");
            else
                return BadRequest("Could not Save to Mixing details");
        }


        [HttpGet("GetProgramsForProductionEntry")]
        public List<ProgramForProductionEntry> GetProgramsForProductionEntry()
        {
            return _conversionRepository.GetProgramsForProductionEntry();
        }


        [HttpPost("ConversionProduction")]
        public async Task<ActionResult<string>> ConversionProduction(ProductionEntry cp)
        {
            bool isSavedSuccessfully = _conversionRepository.ConversionProduction(cp);

            if (isSavedSuccessfully)
                return Ok("Successfully Saved the Production details");
            else
                return BadRequest("Could not Save to Production details");
        }


        [HttpPost("ConversionWaste")]
        public async Task<ActionResult<string>> ConversionWaste(long programId, List<ProgramWaste> programWaste, long createdByUserId)
        {
            bool isSavedSuccessfully = _conversionRepository.ConversionWaste(programId, programWaste, createdByUserId);

            if (isSavedSuccessfully)
                return Ok("Successfully Saved the Production Waste details");
            else
                return BadRequest("Could not Save to Production Waste details");
        }


        [HttpGet("GetProgramWasteById")]
        public List<ProgramWaste> GetProgramWasteById(long programId)
        {
            return _conversionRepository.GetProgramWasteById(programId);
        }

        [HttpGet("GetProductionWasteStock")]
        public List<ProgramWasteStock> GetProductionWasteStock(string wasteEntryFromDate, string wasteEntryToDate, long shadeId, long blendId)
        {
            return _conversionRepository.GetProductionWasteStock(wasteEntryFromDate, wasteEntryToDate, shadeId, blendId);
        }


        [HttpGet("GetYarnRecoverySummary")]
        public List<YarnRecoverySummary> GetYarnRecoverySummary()
        {
            return _conversionRepository.GetYarnRecoverySummary();
        }


        [HttpGet("GetConversionProgramStatus")]
        public List<ConversionProgramStatus> GetConversionProgramStatus(long shadeId, long blendId, long countsId)
        {
            return _conversionRepository.GetConversionProgramStatus(shadeId, blendId, countsId);
        }

		[HttpGet("GetConversionProgramsByShade")]
		public List<ConversionProgram> GetConversionProgramsByShade(long shadeId, string fromDate, string toDate)
		{
			return _conversionRepository.GetConversionProgramsByShade(shadeId, fromDate, toDate);
		}

		[HttpPost("UpdateConversionProgram")]
		public async Task<ActionResult<string>> UpdateConversionProgram(ConversionProgram program, long updatedByUserId)
		{
			bool isSavedSuccessfully = _conversionRepository.UpdateConversionProgram(program, updatedByUserId);

			if (isSavedSuccessfully)
				return Ok("Successfully Saved the Production Waste details");
			else
				return BadRequest("Could not Save to Production Waste details");
		}

	}
}
