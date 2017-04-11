using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using OLX_3821.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace OLX_3821.DataAccess
{
    public class DatabaseManager
    {
        public static string ConnectionStr
        {
            get
            {
                return WebConfigurationManager
                    .ConnectionStrings["ConnStrDB"]
                    .ConnectionString;
            }
        }

        public static bool DatabaseExists()
        {
            try
            {
                var allCustomers = new UsersManager().GetAllUsers();
                return allCustomers.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void CreateDatabase()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/00003821_DBSD_CW2.sql");

            string script = File.ReadAllText(path);

            SqlConnection conn = new SqlConnection(ConnectionStr);

            Server server = new Server(new ServerConnection(conn));

            server.ConnectionContext.ExecuteNonQuery(script);
        }

        public static void CreateDatabaseIfNotExists()
        {
            if (!DatabaseExists())
                CreateDatabase();
        }

    }

}