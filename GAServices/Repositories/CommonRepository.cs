using System.Data;
using GAServices.Common;

namespace GAServices.Repositories
{
    public interface ICommonRepository
    {
        string GeneratePONo(AUTOGEN_TYPE autoGenerateType);
    }

    public class CommonRepository : ICommonRepository
    {
        private readonly DataAccess _dataAccess;

        public CommonRepository()
        {
            _dataAccess = new DataAccess();
        }

        public string GeneratePONo(AUTOGEN_TYPE autoGenerateType)
        {
            string query = @"SELECT concat(autogenerate_no, ""/"", short_code) PONO from tbautogenerate
	                            INNER JOIN tbautogeneratetype on autogenerate_typeid = autogeneratetype_id
                             WHERE autogeneratetype = '" + autoGenerateType.ToString() + "'";

            string poNO = _dataAccess.DB.ExecuteScalar<string>(query);

            return (poNO);
        }
    }
}
