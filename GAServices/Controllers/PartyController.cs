using GAServices.BusinessEntities.Party;
using GAServices.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartyController : ControllerBase
    {
        private readonly IPartyRepository _partyRepository;

        public PartyController(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        [HttpGet("GetAllDistricts")]
        public ActionResult<IEnumerable<District>> GetAllDistricts()
        {
            try
            {
                return _partyRepository.GetDistricts();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAllCities")]
        public ActionResult<IEnumerable<City>> GetAllCities()
        {
            try
            {
                return _partyRepository.GetCities();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetStates")]
        public ActionResult<IEnumerable<State>> GetAllStates()
        {
            try
            {
                return Ok(_partyRepository.GetStates());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddParty")]
        public async Task<ActionResult<PartyDetails>> AddParty([FromBody] Party party)
        {
            try
            {
                bool isPartyAlreadyExist = _partyRepository.IsPartyAlreadyExist(party.PartyName, party.BranchName);

                if (isPartyAlreadyExist)
                    return Conflict(party.PartyName + " already exist for te branch " + party.BranchName);

                long newPartyId = _partyRepository.AddParty(party);

                return CreatedAtAction("GetPartyById", new { id = newPartyId }, newPartyId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetPartyById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PartyDetails> GetPartyById(int id)
        {
            try
            {
                var party = _partyRepository.GetPartyById(id);

                if (party == null)
                    return Ok(StatusCodes.Status404NotFound);

                return Ok(party);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //[HttpGet(Name = "GetAllParties")]
        [HttpGet("GetAllParties")]
        public ActionResult<IEnumerable<PartyDetails>> GetAllParties()
        {
            try
            {
                return _partyRepository.GetAllParties();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("UpdateParty")]
        public ActionResult<bool> UpdateParty(PartyDetails party)
        {
            try
            {
                bool isDeleted = _partyRepository.UpdateParty(party);

                if (isDeleted)
                    return Ok("Party updated Successfully");
                else
                    return Ok("Could not update the Party");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("DeleteParty")]
        public ActionResult<bool> DeleteParty(DeleteParty party)
        {
            try
            {
                bool isDeleted = _partyRepository.DeleteParty(party.PartyId, party.DeletedByUserId);

                if (isDeleted)
                    return Ok("Party deleted Successfully");
                else
                    return Ok("Could not delete the Party");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
