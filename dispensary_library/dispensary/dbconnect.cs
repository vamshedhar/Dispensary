using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Dispensary
{
    /*
     * 
     *  Created By -- Vam$hedhar Reddy C
     *  All Rights Reserved
     *  Product of IMG Labs IIT Roorkee, Saharanpur Campus
     *  
     * */

    /// <summary>
    /// 
    /// Class consists of all ADO.NET functions currently for using a MySQL database
    /// Incase in future if it comes to change database host from MySQL to anything, changing ADO.NET functions here is just enough for the whole project.
    /// 
    /// </summary>
    public class dbconnect
    {
        public MySqlConnection con;
        MySqlCommand cmd;
        public MySqlDataReader dr;
        public MySqlDataAdapter da;
        public string nonquery;
        public string reader;

        // Connection string of MySQL Host for 'dispensary' database.
        // Create a Database Connection.
        public dbconnect()
        {
            con = new MySqlConnection("Server=172.23.12.240; Port=3306;Database=dispensary;Uid=dispensary;Pwd=sql_sre!;");
          //  con = new MySqlConnection("Server=127.0.0.1; Database=dispensary;Uid=root;Pwd=;");
            con.Open();
        }

        // Connection string of MySQL Host for other databases. (dbname -- Database Name)
        // Create a Database Connection.
        public dbconnect(string dbname)
        {
            con = new MySqlConnection("Server=172.23.12.240; Port=3306;Database=" + dbname + ";Uid=dispensary;Pwd=sql_sre!;");
         //  con = new MySqlConnection("Server=127.0.0.1; Database=" + dbname + " ;Uid=root;Pwd=;");
            con.Open();
        }

        // For Insert and Update queries.
        public void command_nonquery(string query,MySqlConnection connect)
        {
            cmd = new MySqlCommand(query, connect);
            try
            {
                cmd.ExecuteNonQuery();
                reader = "Sucess";
            }
            catch (Exception ex)
            {
                reader = ex.Message;
            }
            
        }

        // For Select queries. Returns a DataReader with the MySQL query result.
        public void command_reader(string query,MySqlConnection connect)
        {
            cmd = new MySqlCommand(query, connect);
            try
            {
                dr = cmd.ExecuteReader();
                reader = "Sucess";
            }
            catch (Exception ex)
            {
                reader = ex.Message;
            }
        
        }
        public void data_adapter(string query, MySqlConnection connect)
        {
            cmd = new MySqlCommand(query, connect);
            da = new MySqlDataAdapter(cmd);
        }

        // Close DataReader
        public void reader_close()
        {
            dr.Close();
        }

        // Close Database Connection.
        public void dbclose()
        {
            con.Close();
        }
    }
}
