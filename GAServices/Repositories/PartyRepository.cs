using System.Data;
using System.IO;
using GAServices.Repositories;
using GAServices.Common;
using GAServices.BusinessEntities.Party;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Mysqlx;

namespace GAServices.Repositories
{
    public interface IPartyRepository
    {

        public List<District> GetDistricts();

        public List<City> GetCities();

        public List<State> GetStates();

        public PartyDetails GetPartyById(int partyId);

        public List<PartyDetails> GetAllParties();

        public bool IsPartyAlreadyExist(string partyName, string branchName);

        public long AddParty([FromBody] Party party);

        public bool UpdateParty(PartyDetails party);

        public bool DeleteParty(long partyId, long userId);
    }

    public class PartyRepository : IPartyRepository
    {
        private readonly DataAccess _dataAccess;

        public PartyRepository()
        {
            _dataAccess = new DataAccess();
        }

        public List<District> GetDistricts()
        {
            string query = @"SELECT DistrictId, DistrictName
                            FROM tbdistricts ";
            DataTable dtDistricts = _dataAccess.DB.GetData(query);

            List<District> districts = Utilities.CreateListFromTable<District>(dtDistricts);

            return districts;
        }

        public List<City> GetCities()
        {
            string query = @"SELECT CityId, CityName
                            FROM tbCities ";
            DataTable dtCities = _dataAccess.DB.GetData(query);

            List<City> cities = Utilities.CreateListFromTable<City>(dtCities);

            return cities;
        }

        public List<State> GetStates()
        {
            string query = @"SELECT StateId, StateName, GSTCode
                            FROM tbStates ";
            DataTable dtStates = _dataAccess.DB.GetData(query);

            List<State> states = Utilities.CreateListFromTable<State>(dtStates);

            return states;
        }

        public PartyDetails GetPartyById(int partyId)
        {
            string query = @"SELECT PartyId, PartyName, BranchName,
                                Address1, Address2, Address3, p.DistrictId, DistrictName, p.CityId, CityName, PinCode, 
                                p.StateId, StateName, GSTCode, GSTNo, EmailId, ContactNo
                            FROM tbparties p INNER JOIN tbCities C on p.CityId = C.CityId
                                INNER JOIN tbDistricts D on p.DistrictId = D.DistrictId
                                INNER JOIN tbStates S on p.StateId = S.StateId
                            WHERE PartyId = " + partyId;

            DataTable dtParty = _dataAccess.DB.GetData(query);

            List<PartyDetails> party = Utilities.CreateListFromTable<PartyDetails>(dtParty);

            return party[0];
        }

        public List<PartyDetails> GetAllParties()
        {
            string query = @"SELECT PartyId, PartyName, BranchName,
                                Address1, Address2, Address3, p.DistrictId, DistrictName, p.CityId, CityName, PinCode, 
                                p.StateId, StateName, GSTCode, GSTNo, EmailId, p.ContactNo
                            FROM tbparties p INNER JOIN tbCities C on p.CityId = C.CityId
                                INNER JOIN tbDistricts D on p.DistrictId = D.DistrictId
                                INNER JOIN tbStates S on p.StateId = S.StateId
                            WHERE p.IsDeleted = 0";
            DataTable dtParties = _dataAccess.DB.GetData(query);

            List<PartyDetails> parties = Utilities.CreateListFromTable<PartyDetails>(dtParties);

            return parties;
        }

        public bool IsPartyAlreadyExist(string partyName, string branchName)
        {
            DataTable dtExistingParty = _dataAccess.DB.GetData("SELECT * FROM TBPARTIES WHERE PartyName = '" + partyName.ToUpper() + "' AND BranchName = '" + branchName + "'");

            if (dtExistingParty.HasRecords())
                return true;
            else
                return false;
        }

        public long AddParty([FromBody] Party party)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();
            inParam.Add(new MySqlParameter("pPartyDts", party.GetXmlString()));
            inParam.Add(new MySqlParameter("pUserId", party.CreatedByUserId));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pPartyId", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("ADDPARTY", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    return response.ReturnData["pPartyId"].ToLong();
                }
            }

            return 0;
        }

        public bool UpdateParty(PartyDetails party)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();
            inParam.Add(new MySqlParameter("pPartyDts", party.GetXmlString()));
            inParam.Add(new MySqlParameter("pModifiedByUserId", party.CreatedByUserId));

            List<MySqlParameter> outParam = new List<MySqlParameter>();
            outParam.Add(new MySqlParameter("pUpdatedRecordCounts", MySqlDbType.Int64));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("UpdateParty", inParam.ToArray(), outParam.ToArray());

            if (response != null)
            {
                if (response.ReturnData != null)
                {
                    if (response.ReturnData["pUpdatedRecordCounts"].ToLong() > 0)
                        return true;
                }
            }

            return false;
        }

        public bool DeleteParty(long partyId, long userId)
        {
            List<MySqlParameter> inParam = new List<MySqlParameter>();
            inParam.Add(new MySqlParameter("pPartyId", partyId));
            inParam.Add(new MySqlParameter("pDeletedByUserId", userId));

            AppResponse response = _dataAccess.DB.Insert_UpdateData("DeleteParty", inParam.ToArray(), null);

            return true;
        }
    }
}
