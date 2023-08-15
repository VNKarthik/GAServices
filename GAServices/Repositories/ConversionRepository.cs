using System.Data;
using System.Xml.Serialization;
using GAServices.BusinessEntities.Conversion;
using GAServices.BusinessEntities.FibrePOEntities;
using GAServices.BusinessEntities.Yarn;
using GAServices.Common;
using MySql.Data.MySqlClient;

namespace GAServices.Repositories
{
    public interface IConversionRepository
    {
        public string CreateConversionProgram(ConversionProgram program);

        public List<ProgramForMixing> ProgramsForMixing();

        public ConversionProgram GetProgramDetailsById(long programId);

        public List<ConversionYarn> GetProgramYarnDetailsById(long programId);

        public bool IssueFibreForMixing(FibreMixing fibres);

        public List<ProgramForProductionEntry> GetProgramsForProductionEntry();

        public bool ConversionProduction(ProductionEntry cp);

        public bool ConversionWaste(long programId, List<ProgramWaste> programWaste, long createdByUserId);

        //public ConversionProgramWaste GetProgramByIdWithFibreWaste(long programId);

        public List<ProgramWaste> GetProgramWasteById(long programId);

        public List<YarnRecoverySummary> GetYarnRecoverySummary();

        public List<ConversionProgramStatus> GetConversionProgramStatus(long shadeId, long blendId, long countsId);
    }

    public class ConversionRepository : IConversionRepository
    {
        private readonly DataAccess _dataAccess;

        public ConversionRepository()
        {
            _dataAccess = new DataAccess();
        }

        public string CreateConversionProgram(ConversionProgram program)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();
            inParam.Add(new MySqlParameter("pProgramDate", program.ProgramDate));
            inParam.Add(new MySqlParameter("pShadeId", program.ShadeId));
            inParam.Add(new MySqlParameter("pBlendId", program.BlendId));
            inParam.Add(new MySqlParameter("pCreatedByUserId", program.CreatedByUserId));
            inParam.Add(new MySqlParameter("pRemarks", program.Remarks));
            inParam.Add(new MySqlParameter("pYarnDts", program.YarnCounts.GetXmlString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pProgramNo", MySqlDbType.VarChar));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("CreateConversionProgram", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    return response.ReturnData["pProgramNo"].GetStringValue();
                }
            }

            return "";
        }

        public List<ProgramForMixing> ProgramsForMixing()
        {
            List<ProgramForMixing> programsForMixing = new List<ProgramForMixing>();
            DataTable dtPrograms = _dataAccess.DB.GetData("GetProgramsForMixing", null);

            programsForMixing = Utilities.CreateListFromTable<ProgramForMixing>(dtPrograms);

            return programsForMixing;
        }

        public ConversionProgram GetProgramDetailsById(long programId)
        {
            ConversionProgram programDtsForMixing;
            DataTable dtProgramDetails = _dataAccess.DB.GetData("GetProgramDetailsById", new List<MySqlParameter>() { new MySqlParameter("pProgramId", programId) });

            programDtsForMixing = Utilities.CreateListFromTable<ConversionProgram>(dtProgramDetails).Distinct().FirstOrDefault();

            programDtsForMixing.YarnCounts = GetProgramYarnDetailsById(programId);
            
            return programDtsForMixing;
        }

        public List<ConversionYarn> GetProgramYarnDetailsById(long programId)
        {
            List<ConversionYarn> conversionYarn;
            conversionYarn = _dataAccess.DB.GetData<ConversionYarn>("GetProgramYarnDetailsById", new List<MySqlParameter>() { new MySqlParameter("pProgramId", programId) });

            return conversionYarn;
        }

        public bool IssueFibreForMixing(FibreMixing fibres)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pProgramId", fibres.ProgramId.ToString()));
            inParam.Add(new MySqlParameter("pMixingDate", fibres.MixingDate));
            inParam.Add(new MySqlParameter("pFibreDts", fibres.Fibres.GetXmlString()));
            inParam.Add(new MySqlParameter("pUserId", fibres.IssuedByUseId.ToString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pRecordsInserted", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("IssueFibreForMixing", inParam.ToArray(), outParam.ToArray());

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

        public List<ProgramForProductionEntry> GetProgramsForProductionEntry()
        {
            List<ProgramForProductionEntry> programForProductionEntry = new List<ProgramForProductionEntry>();
            DataTable dtPrograms = _dataAccess.DB.GetData("GetProgramsForProductionEntry", null);

            programForProductionEntry = Utilities.CreateListFromTable<ProgramForProductionEntry>(dtPrograms);

            return programForProductionEntry;
        }

        public bool ConversionProduction(ProductionEntry cp)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pProgramId", cp.ProgramId.ToString()));
            inParam.Add(new MySqlParameter("pMixingId", cp.MixingId));
            inParam.Add(new MySqlParameter("pProductionDate", cp.ProductionDate));
            inParam.Add(new MySqlParameter("pYarnDts", cp.YarnDetails.GetXmlString()));
            inParam.Add(new MySqlParameter("pUserId", cp.CreatedByUserId.ToString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pRecordsInserted", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("ProductionEntry", inParam.ToArray(), outParam.ToArray());

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

        public bool ConversionWaste(long programId, List<ProgramWaste> programWaste, long createdByUserId)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pProgramId", programId.ToString()));
            inParam.Add(new MySqlParameter("pWasteDts", programWaste.GetXmlString()));
            inParam.Add(new MySqlParameter("pUserId", createdByUserId.ToString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pRecordsInserted", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("ProductionWasteEntry", inParam.ToArray(), outParam.ToArray());

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

        public List<ProgramWaste> GetProgramWasteById(long programId)
        {
            List<ProgramWaste> programWaste;
            programWaste = _dataAccess.DB.GetData<ProgramWaste>("GetProgramWasteById", new List<MySqlParameter>() { new MySqlParameter("pProgramId", programId) });

            return programWaste;
        }

        public List<YarnRecoverySummary> GetYarnRecoverySummary()
        {
            List<YarnRecoverySummary> yarnRecoverySummary;
            yarnRecoverySummary = _dataAccess.DB.GetData<YarnRecoverySummary>("GetYarnRecoverySummary", null);

            return yarnRecoverySummary;
        }

        public List<ConversionProgramStatus> GetConversionProgramStatus(long shadeId, long blendId, long countsId)
        {
            return _dataAccess.DB.GetData<ConversionProgramStatus>("GetConversionProgramStatus", new List<MySqlParameter>() { new MySqlParameter("pShadeId", shadeId), new MySqlParameter("pBlendId", blendId), new MySqlParameter("pCountsId", countsId) });
        }
    }
}