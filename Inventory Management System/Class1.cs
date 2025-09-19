//add first this library
using MySql.Data.MySqlClient;
//for SQL server using System.Data.SqlClient;
using System.Data;

namespace inventory_management
{
    class Class1
    {
        private string sqlConString;
        public int rowAffected = 0;

        public Class1(string server_address, string database, string username, string password)
        {
            //Server = server name(xampp) Uid = username Pwd = password
            sqlConString = "Server=" + server_address + ";Database=" + database
                + ";Uid=" + username + ";Pwd=" + password + ";CharSet=utf8;Pooling=true;Max Pool Size=100;";
        }

        //select
        public DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection Sqlcon = new MySqlConnection(sqlConString))
            {
                using (MySqlCommand SQLcom = new MySqlCommand(sql, Sqlcon))
                {
                    using (MySqlDataAdapter SQLadap = new MySqlDataAdapter(SQLcom))
                    {
                        Sqlcon.Open();
                        SQLadap.Fill(dt);
                    }
                }
            } // ✅ connection auto-closed and disposed here

            return dt;
        }

        //insert, update, delete
        public void executeSQL(string sql)
        {
            using (MySqlConnection Sqlcon = new MySqlConnection(sqlConString))
            {
                using (MySqlCommand SQLcom = new MySqlCommand(sql, Sqlcon))
                {
                    Sqlcon.Open();
                    rowAffected = SQLcom.ExecuteNonQuery();
                }
            } // ✅ connection auto-closed and disposed here
        }

        public string SqlConString { get; set; }
    }
}
