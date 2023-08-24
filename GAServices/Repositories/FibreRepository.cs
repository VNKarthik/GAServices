using System.Data;
using System.Xml.Serialization;
using GAServices.BusinessEntities.Conversion;
using GAServices.BusinessEntities.FibrePOEntities;
using GAServices.BusinessEntities.Party;
using GAServices.Common;
using MySql.Data.MySqlClient;
using static Google.Protobuf.WireFormat;

namespace GAServices.Repositories
{
    public interface IFibreRepository
    {
        public List<FiberType> GetFibreTypes();

        public List<FiberType> GetActiveFibreTypes();

        public List<FibreCategory> GetFibreCategories();

        public long AddFibreType(FiberType fibreType);

        public List<FibreShade> GetFibreShades();

        public List<FibreShade> GetActiveFibreShades();

        public long AddFibreShade(string fibreShadeName);

        public long CreatePO(CreateFibrePO fibrePO);

        public Task<FibrePO> GetPOById(long poId);

        public Task<long> GetOpenPOCounts();

        public Task<List<PartywisePOCounts>> GetPartywiseOpenPOCounts();

        public Task<List<PendingPODtsByParty>> PendingPODetailsByParty(long partyId);

        //public bool IsAlreadyReceived();

        public bool ReceivePOFibre(ReceiveFibrePO fibrePO);

        public Task<List<POSummaryFor12Months>> Last12MonthSummary();

        public List<FibreStock> GetFibreStock();

        public List<FibreStock> GetFibreStockForMixing(long programId);

        public List<FibreWasteCategory> GetFibreWasteCategories();

        public List<FibrePO> GetFibreOrdersPendingToReceive();

        public List<FibrePO> GetFiberPODetails_WitStatus(long partyId, string fromDate, string toDate);

        public List<FiberIssueDetails> GetFiberConsumptionByRecdDtsId(long receivedDtsId);

        public List<FibreStock> GetFibreStockSearch(string asOnDate, long partyId, long fiberTypeId);
    }

    public class FibreRepository : IFibreRepository
    {
        private readonly DataAccess _dataAccess;

        public FibreRepository()
        {
            _dataAccess = new DataAccess();
        }

        public List<FibreCategory> GetFibreCategories()
        {
            DataTable dtFibreCategories = _dataAccess.DB.GetData("SELECT FibreCategoryId, CategoryName FibreCategoryName, CategoryCode, CategoryOrder FROM tbfibrecategory");

            List<FibreCategory> fiberCategories = Utilities.CreateListFromTable<FibreCategory>(dtFibreCategories);

            return fiberCategories;
        }

        public long AddFibreType(FiberType fibreType)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();
            //inParam.Add(new MySqlParameter("pFibreTypeName", fibreTypeName));
            inParam.Add(new MySqlParameter("pFibreTypeName", fibreType.FibreType));
            inParam.Add(new MySqlParameter("pFibreCategoryId", fibreType.FibreCategoryId));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pFibreTypeId", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("AddFibreType", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    return response.ReturnData["pFibreTypeId"].ToLong();
                }
            }

            return 0;
        }

        public List<FiberType> GetFibreTypes()
        {
            DataTable dtFibres = _dataAccess.DB.GetData("SELECT * FROM TBFIBRETYPES");

            List<FiberType> fiberTypes = Utilities.CreateListFromTable<FiberType>(dtFibres);

            return fiberTypes;
        }

        public List<FiberType> GetActiveFibreTypes()
        {
            DataTable dtFibres = _dataAccess.DB.GetData("SELECT * FROM vi_active_fibre_types");
            //List<FiberType> fiberTypes = new List<FiberType>();

            //foreach (DataRow dr in dtFibres.Rows)
            //{
            //    fiberTypes.Add(new FiberType()
            //    {
            //        FibreTypeId = dr["FibreType_Id"].ToLong(),
            //        FibreType = dr["FibreType"].GetStringValue()
            //    });
            //}

            List<FiberType> fiberTypes = Utilities.CreateListFromTable<FiberType>(dtFibres);

            return fiberTypes;
        }

        public long AddFibreShade(string fibreShadeName)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();
            inParam.Add(new MySqlParameter("pFibreShadeName", fibreShadeName));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pFibreShadeId", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("AddFibreShade", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    return response.ReturnData["pFibreShadeId"].ToLong();
                }
            }

            return 0;
        }

        public List<FibreShade> GetFibreShades()
        {
            DataTable dtFibreShades = _dataAccess.DB.GetData("SELECT * FROM tbFibreShades");

            List<FibreShade> fiberShades = Utilities.CreateListFromTable<FibreShade>(dtFibreShades);

            return fiberShades;
        }

        public List<FibreShade> GetActiveFibreShades()
        {
            DataTable dtFibreShades = _dataAccess.DB.GetData("SELECT * FROM vi_active_FibreShades");

            List<FibreShade> fiberShades = Utilities.CreateListFromTable<FibreShade>(dtFibreShades);

            return fiberShades;
        }

        //public async Task<string> GeneratePONo()
        //{
        //    string fibrePOAutoNumber = await (new CommonRepository()).GeneratePONo(AUTOGEN_TYPE.FIBRE_PO);

        //    return await Task.FromResult(fibrePOAutoNumber);
        //}

        public long CreatePO(CreateFibrePO fibrePO)
        {
            //string poNo, poDate;
            //long partyId;

            List<CreateFibrePODts> lstFibres = fibrePO.FibrePODts;

            //XmlSerializer xmlFibreDts = new XmlSerializer(lstFibres.GetType());

            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pPONo", fibrePO.PONo.ToString()));
            inParam.Add(new MySqlParameter("pPOdate", fibrePO.POdate.ToString("yyyy-MM-dd")));
            inParam.Add(new MySqlParameter("pPartyId", fibrePO.PartyId.ToString()));
            inParam.Add(new MySqlParameter("pFibreDts", lstFibres.GetXmlString()));
            inParam.Add(new MySqlParameter("pUserId", fibrePO.CreatedBy.ToString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pFibrePOId", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("CREATEFIBREPO", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    return response.ReturnData["pFibrePOId"].ToLong();
                }
            }

            return 0;
        }

        public Task<FibrePO> GetPOById(long poId)
        {
            DataTable dtFibreDetails;
            List<FibrePO> fiberPOs = new List<FibrePO>();
            DataSet dsFibrePO = _dataAccess.DB.GetDataSet("GETFIBERPO", new List<MySqlParameter>() { new MySqlParameter("pPOId", poId) });

            if (dsFibrePO.Tables.Count > 0)
            {
                if (!dsFibrePO.Tables[0].HasRecords())
                    return null;

                fiberPOs = Utilities.CreateListFromTable<FibrePO>(dsFibrePO.Tables[0]);

                dtFibreDetails = dsFibrePO.Tables[1];

                foreach (FibrePO po in fiberPOs)
                {
                    dtFibreDetails.DefaultView.RowFilter = "POId = " + po.FibrePoid;
                    po.FibrePODts = Utilities.CreateListFromTable<FibrePODts>(dtFibreDetails.DefaultView.ToTable());
                }

                if (fiberPOs.Count > 0)
                    return Task.FromResult(fiberPOs[0]);
                else
                    return null;
            }
            else
                return null;
        }

        public Task<long> GetOpenPOCounts()
        {
            long openFibrePOs = _dataAccess.DB.ExecuteScalar<long>("SELECT OPEN_POCOUNTS FROM VI_FIBRE_OPENPOS");

            return Task.FromResult(openFibrePOs);
        }

        public Task<List<PartywisePOCounts>> GetPartywiseOpenPOCounts()
        {
            List<PartywisePOCounts> partywisePOCounts = new List<PartywisePOCounts>();
            DataTable dtPartywisePOCounts = _dataAccess.DB.GetData("SELECT PartyId, PartyName, OPEN_POCOUNTS POCounts FROM VI_FIBRE_PARTYWISE_OPENPOCOUNTS");

            partywisePOCounts = Utilities.CreateListFromTable<PartywisePOCounts>(dtPartywisePOCounts);

            return Task.FromResult(partywisePOCounts);
        }

        public async Task<List<PendingPODtsByParty>> PendingPODetailsByParty(long partyId)
        {
            List<PendingPODtsByParty> partywisePOCounts = new List<PendingPODtsByParty>();
            DataSet dsPartywisePOCounts = _dataAccess.DB.GetDataSet("GET_PENDING_PODTS_BYPARTY", new List<MySqlParameter>() { new MySqlParameter("pPartyID", partyId) });

            partywisePOCounts = Utilities.CreateListFromTable<PendingPODtsByParty>(dsPartywisePOCounts.Tables[0]);

            return await Task.FromResult(partywisePOCounts);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fibrePO"></param>
        /// <returns></returns>
        public bool ReceivePOFibre(ReceiveFibrePO fibrePO)
        {
            //Need to do duplicate check before receiving

            List<ReceiveFibrePODts> lstFibres = fibrePO.FibrePODts;

            //XmlSerializer xmlFibreDts = new XmlSerializer(lstFibres.GetType());

            List<MySqlParameter> inParam = new List<MySqlParameter>();

            try
            {
                inParam.Add(new MySqlParameter("pPartyId", fibrePO.PartyId.ToString()));
                inParam.Add(new MySqlParameter("pRecdDCNo", fibrePO.RecdDCNo.ToString()));
                //inParam.Add(new MySqlParameter("pRecdDate", fibrePO.RecdDate.ToString("yyyy-MM-dd")));
                //inParam.Add(new MySqlParameter("pDCDate", fibrePO.DCDate.ToString("yyyy-MM-dd")));
                inParam.Add(new MySqlParameter("pRecdDate", fibrePO.RecdDate));
                inParam.Add(new MySqlParameter("pDCDate", fibrePO.DCDate));
                inParam.Add(new MySqlParameter("pFibreDts", lstFibres.GetXmlString()));
                inParam.Add(new MySqlParameter("pUserId", fibrePO.ReceivedByUserId.ToString()));

                List<MySqlParameter> outParam = new List<MySqlParameter>();
                outParam.Add(new MySqlParameter("pRecordsInserted", MySqlDbType.Int64));

                AppResponse response = _dataAccess.DB.Insert_UpdateData("RECEIVE_POFIBRE", inParam.ToArray(), outParam.ToArray());

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
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public async Task<List<POSummaryFor12Months>> Last12MonthSummary()
        {
            List<POSummaryFor12Months> summary = new List<POSummaryFor12Months>();
            DataTable dtSummary = _dataAccess.DB.GetData("spLast12MonthsPOSummary");

            summary = Utilities.CreateListFromTable<POSummaryFor12Months>(dtSummary);

            return await Task.FromResult(summary);
        }

        public List<FibreStock> GetFibreStock()
        {
            return _dataAccess.DB.GetData<FibreStock>("GetFibreStock", null);
        }

        public List<FibreStock> GetFibreStockForMixing(long conversionProgramId)
        {
            return _dataAccess.DB.GetData<FibreStock>("GetFibreStockForMixing", new List<MySqlParameter>() { new MySqlParameter("pProgramId", conversionProgramId) });
        }

        public List<FibreWasteCategory> GetFibreWasteCategories()
        {
            return _dataAccess.DB.GetData<FibreWasteCategory>("GetFibreWasteCategories", null);
        }

        public List<FibrePO> GetFibreOrdersPendingToReceive()
        {
            return _dataAccess.DB.GetData<FibrePO>("GetFibreOrdersPendingToReceive", null);
        }

        public List<FibrePO> GetFiberPODetails_WitStatus(long partyId, string fromDate, string toDate)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pPartyId", partyId));
            inParam.Add(new MySqlParameter("pFromDate", fromDate));
            inParam.Add(new MySqlParameter("pToDate", toDate));

            return _dataAccess.DB.GetData<FibrePO>("GetFiberPODetails_WitStatus", inParam);
        }
    
        public List<FiberIssueDetails> GetFiberConsumptionByRecdDtsId (long receivedDtsId)
        {
            return _dataAccess.DB.GetData<FiberIssueDetails>("GetFiberConsumptionByRecdDtsId", new List<MySqlParameter>() { new MySqlParameter("pReceivedDtsId", receivedDtsId) });
        }

        public List<FibreStock> GetFibreStockSearch(string asOnDate, long partyId, long fiberTypeId)
        {
            List<MySqlParameter> inParams = new List<MySqlParameter>();
            inParams.Add(new MySqlParameter("pAsOnDate", asOnDate));
            inParams.Add(new MySqlParameter("pPartyId", partyId));
            inParams.Add(new MySqlParameter("pFiberTypeId", fiberTypeId));

            return _dataAccess.DB.GetData<FibreStock>("GetFibreStockSearch", inParams);
        }
    }
}
