using System;
using System.Data.SqlClient;

namespace WCFServer
{
    public class DBConnection
    {
        //Connection string to the Local DB
        private static readonly string cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tensu\source\repos\E-Commerce\WCFServer\DatabaseE_Commerce.mdf;Integrated Security=True;MultipleActiveResultSets=true";
        
        private static readonly SqlConnection conn;

        static DBConnection()
        {
            conn = new SqlConnection(cs);
        }

        public static void DBOpen()
        {
            conn.Open();
        }

        public static void DBClose()
        {
            conn.Close();
        }

        //Function used to execute queries
        public static bool Execute(string query)
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();
                SqlTransaction tx = connection.BeginTransaction();
                SqlCommand sqlcmd = new SqlCommand(query, connection);
                sqlcmd.Transaction = tx;

                try
                {
                    sqlcmd.ExecuteNonQuery();
                    tx.Commit();
                    connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    tx.Rollback();
                }
            }
            return false;
        }

        //DB data reader function to read a query output
        public static SqlDataReader Read(string query)
        {
            SqlCommand sqlcmd = new SqlCommand(query, conn);
            return sqlcmd.ExecuteReader();
        }

        public static SqlConnection GetConnection()
        {
            return conn;
        }
    }
}

