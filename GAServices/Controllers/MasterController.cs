using GAServices.BusinessEntities.FibrePOEntities;
using GAServices.BusinessEntities.Yarn;
using GAServices.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMasterRepository _masterRepository;
        private readonly ICommonRepository _commonRepository;

        public MasterController(IMasterRepository masterRepository, ICommonRepository commonRepository)
        {
            _masterRepository = masterRepository;
            _commonRepository = commonRepository;
        }

        [HttpPost("AddYarnShade")]
        public async Task<ActionResult<YarnShade>> AddYarnShade(string shadeName)
        {
            long newShadeId = _masterRepository.AddYarnShade(shadeName);

            return CreatedAtAction("GetYarnShades", newShadeId, newShadeId);
        }

        [HttpGet("GetYarnShades")]
        public ActionResult<IEnumerable<YarnShade>> GetYarnShades()
        {
            return _masterRepository.GetYarnShades();            
        }

        [HttpGet("GetActiveYarnShades")]
        public ActionResult<IEnumerable<YarnShade>> GetActiveYarnShades()
        {
            return _masterRepository.GetActiveYarnShades();
        }

        [HttpPost("AddYarnBlend")]
        public ActionResult<YarnBlend> AddYarnBlend(YarnBlendCreate fibreBlend)
        {
            string newBlend = _masterRepository.AddYarnBlend(fibreBlend);

            return CreatedAtAction("GetYarnBlendList", newBlend, newBlend);
        }

        [HttpGet("GetYarnBlendList")]
        public ActionResult<List<YarnBlend>> GetYarnBlendList()
        {
            return _masterRepository.GetYarnBlendList();
        }

        [HttpPost("AddYarnCounts")]
        public async Task<ActionResult<YarnShade>> AddYarnCounts(string countsName)
        {
            long newShadeId = _masterRepository.AddYarnCounts(countsName);

            return CreatedAtAction("GetYarnCountsList", newShadeId, newShadeId);
        }

        [HttpGet("GetYarnCountsList")]
        public ActionResult<List<YarnCounts>> GetYarnCountsList()
        {
            return _masterRepository.GetYarnCountsList();
        }

        [HttpPost("AddWasteCategory")]
        public async Task<ActionResult<FibreWasteCategory>> AddWasteCategory(string wasteCategoryName, long createdByUserId)
        {
            long newWasteCategoryId = _masterRepository.AddWasteCategory(wasteCategoryName, createdByUserId);

            return CreatedAtAction("GetWasteCategories", newWasteCategoryId, newWasteCategoryId);
        }

        [HttpGet("GetWasteCategories")]
        public ActionResult<List<FibreWasteCategory>> GetWasteCategories()
        {
            return _masterRepository.GetWasteCategories();
        }
    }
}
