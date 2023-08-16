using GAServices.BusinessEntities.FibrePOEntities;
using GAServices.Common;
using MySql.Data.MySqlClient;
using GAServices.BusinessEntities.Yarn;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using GAServices.BusinessEntities.Conversion;

namespace GAServices.Repositories
{
    public interface IMasterRepository
    {
        public long AddYarnShade(string shadeName);

        public List<YarnShade> GetYarnShades();

        public List<YarnShade> GetActiveYarnShades();

        public string AddYarnBlend(YarnBlendCreate fibres);

        public List<YarnBlend> GetYarnBlendList();

        public long AddYarnCounts(string counts);

        public List<YarnCounts> GetYarnCountsList();

        public long AddWasteCategory(string wasteCategoryName, long createdByUserId);

        public List<FibreWasteCategory> GetWasteCategories();
    }

    public class MasterRepository : IMasterRepository
    {
        private readonly DataAccess _dataAccess;

        public MasterRepository()
        {
            _dataAccess = new DataAccess();
        }

        //public List<Fibre>

        public long AddYarnShade(string shadeName)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();
            inParam.Add(new MySqlParameter("pYarnShadeName", shadeName));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pYarnShadeId", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("AddYarnShade", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    return response.ReturnData["pYarnShadeId"].ToLong();
                }
            }

            return 0;
        }

        public List<YarnShade> GetYarnShades()
        {
            DataTable dtYarnShade = _dataAccess.DB.GetData("SELECT * FROM tbYarnShades");

            List<YarnShade> yarnShades = Utilities.CreateListFromTable<YarnShade>(dtYarnShade);

            return yarnShades;
        }

        public List<YarnShade> GetActiveYarnShades()
        {
            DataTable dtYarnShade = _dataAccess.DB.GetData("SELECT * FROM vi_active_yarnshades");

            List<YarnShade> yarnShades = Utilities.CreateListFromTable<YarnShade>(dtYarnShade);

            return yarnShades;
        }

        public string AddYarnBlend(YarnBlendCreate fibres)
        {
            //TODO - Check duplication before insert

            List<MySqlParameter> inParam = new List<MySqlParameter>();

            inParam.Add(new MySqlParameter("pFibreDts", fibres.GetXmlString()));
            inParam.Add(new MySqlParameter("pUserId", fibres.CreatedByUserId.ToString()));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pBlendName", MySqlDbType.VarChar));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("AddYarnBlend", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    return response.ReturnData["pBlendName"].GetStringValue();
                }
            }

            return "";
        }

        public List<YarnBlend> GetYarnBlendList()
        {
            DataTable dtBlends;
            List<YarnBlend> fiberBlends = new List<YarnBlend>();
            DataTable dtFibreBlend = _dataAccess.DB.GetData("GetActiveBlendList", null);

            fiberBlends = Utilities.CreateListFromTable<YarnBlend>(dtFibreBlend);

            foreach (YarnBlend blend in fiberBlends)
            {
                dtBlends = _dataAccess.DB.GetData("GetBlendDetails", new List<MySqlParameter>() { new MySqlParameter("pBlendId", blend.BlendId) });
                blend.Fibres = Utilities.CreateListFromTable<YarnBlendFibres>(dtBlends);
            }

            if (fiberBlends.Count > 0)
                return fiberBlends;
            else
                return null;

        }

        public long AddYarnCounts(string countsName)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();
            inParam.Add(new MySqlParameter("pYarnCountsName", countsName));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pYarnCountsId", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("AddYarnCounts", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    return response.ReturnData["pYarnCountsId"].ToLong();
                }
            }

            return 0;
        }

        public List<YarnCounts> GetYarnCountsList()
        {
            List<YarnCounts> yarnCounts = new List<YarnCounts>();
            DataTable dtCounts = _dataAccess.DB.GetData("GetActiveYarnCounts", null);

            yarnCounts = Utilities.CreateListFromTable<YarnCounts>(dtCounts);

            if (yarnCounts.Count > 0)
                return yarnCounts;
            else
                return null;
        }

        public long AddWasteCategory(string wasteCategoryName, long createdByUserId)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();
            inParam.Add(new MySqlParameter("pWasteCategoryName", wasteCategoryName));
            inParam.Add(new MySqlParameter("pUserId", createdByUserId));
            
            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pWasteCategoryId", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("AddFiberWasteCategory", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    return response.ReturnData["pWasteCategoryId"].ToLong();
                }
            }

            return 0;
        }

        public List<FibreWasteCategory> GetWasteCategories()
        {
            List<FibreWasteCategory> wasteCategory = new List<FibreWasteCategory>();
            DataTable dtCounts = _dataAccess.DB.GetData("GetFiberWasteCategories", null);

            wasteCategory = Utilities.CreateListFromTable<FibreWasteCategory>(dtCounts);

            if (wasteCategory.Count > 0)
                return wasteCategory;
            else
                return null;
        }
    }
}
