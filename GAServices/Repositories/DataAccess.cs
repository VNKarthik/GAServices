using Microsoft.Extensions.Configuration;
using GAServices.DataAccessHelper;

namespace GAServices.Repositories
{
    public class DataAccess
    {
        private readonly string _connectionString;
        private DataHelper dB;
        internal DataHelper DB { get => dB; set => dB = value; }

        public DataAccess()
        {
            var builder = (new ConfigurationBuilder()).SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var config = builder.Build();

            _connectionString = config.GetSection("ConnectionStrings").GetSection("mySqlConnection").Value;
            //string conStr = (new ConfigurationManager()).GetConnectionString("ConnectionString:mySqlConnection");

            dB = new DataHelper(_connectionString);
    }

        

        
    }
}
