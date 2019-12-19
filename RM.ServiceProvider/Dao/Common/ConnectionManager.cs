using System.Configuration;

namespace RM.ServiceProvider.Dao
{
    public class ConnectionManager
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["SqlServer_RM_DB"]; }

            //get { return ConfigurationManager.ConnectionStrings["SqlServer_RM_DB"].ToString(); }
        }
    }
}