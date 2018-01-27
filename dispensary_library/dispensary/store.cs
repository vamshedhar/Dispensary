using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Dispensary
{
    public class store
    {
        public DataTable table;
        public MySqlDataReader dr;
        public string error;
        public void storeTransfer(int storeStock,int dispStock,int quantity,string id,string comments,string username,string receiver)
        {
            string update = "UPDATE medicines SET store_stock=" + (storeStock - quantity) + " WHERE medicine_id='" + id + "'";
            string insert = "INSERT INTO " +id + " (office, transaction_id, username, sender, receiver, type, quantity, store_stock, disp_stock, comments) VALUES ('store', '" + DateTime.Today.ToString("yyyyMMdd") + "', '" + username + "', 'store', '"+receiver+"', 'sent', " + quantity + ", " + (storeStock - quantity) + ", " + (dispStock) + ", 'Request Details: " + comments + "')";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(update, db.con);
            db.command_nonquery(insert, db.con);
            db.dbclose();
        }
        public void receiveStock(int storeStock, int dispStock, int quantity, string id, string comments, string username)
        {
            string update = "UPDATE medicines SET store_stock=" + (storeStock + quantity) + " WHERE medicine_id='" + id + "'";
            string insert = "INSERT INTO " + id + " (office, transaction_id, username, sender, receiver, type, quantity, store_stock, disp_stock, comments) VALUES ('store', '" + DateTime.Today.ToString("yyyyMMdd") + "', '" + username + "', 'IITRHospital', 'store', 'received', " + quantity + ", " + (storeStock + quantity) + ", " + dispStock + ", 'Bill No.: " + comments + "')";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(update, db.con);
            db.command_nonquery(insert, db.con);
            db.dbclose();
        }
        public void returnStock(int storeStock, int dispStock, int quantity, string id, string comments, string username)
        {
            string update = "UPDATE medicines SET store_stock=" + (storeStock + quantity) + ",disp_stock=" + (dispStock - quantity) + " WHERE medicine_id='" + id + "'";
            string insert = "INSERT INTO " + id + " (office, transaction_id, username, sender, receiver, type, quantity, store_stock, disp_stock, comments) VALUES ('store', '" + DateTime.Today.ToString("yyyyMMdd") + "', '" + username + "', 'store', 'dispensary', 'sent', " + -quantity + ", " + (storeStock + quantity) + ", " + (dispStock - quantity) + ", 'Request Details: " + comments + "')";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(update, db.con);
            db.command_nonquery(insert, db.con);
            db.dbclose();
        }
        public void loadDispIndentStore()
        {
            string query = "SELECT medicine_id AS ID, medicine_name AS MedicineName, disp_stock AS DispStock, qty AS IndentQty, sno AS UniqueID FROM dispindent WHERE indent_no='" + DateTime.Now.ToString("yyyyMMdd") + "' AND issued='0' ORDER BY timestamp ASC";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "indentDisp";
            db.da.Fill(table);
            db.dbclose();
        }
        public void storeTransferIndent(int storeStock, int dispStock, int quantity, string id, string comments, string username, string receiver, string indentNo, string sno)
        {
            string update = "UPDATE medicines SET store_stock=" + (storeStock - quantity) + ",disp_stock= disp_stock + " + (quantity) + " WHERE medicine_id='" + id + "'";
            string insert = "INSERT INTO " + id + " (office, transaction_id, username, sender, receiver, type, quantity, store_stock, disp_stock, comments) VALUES ('store', '" + DateTime.Today.ToString("yyyyMMdd") + "', '" + username + "', 'store', '" + receiver + "', 'sent', " + quantity + ", (SELECT store_stock FROM medicines WHERE medicine_id='" + id + "'), (SELECT disp_stock FROM medicines WHERE medicine_id='" + id + "'), '" + comments + "')";
            string indent = "UPDATE dispindent SET issued='" + quantity + "' WHERE sno='" + sno + "'";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(update, db.con);
            db.command_nonquery(insert, db.con);
            db.command_nonquery(indent, db.con);
            db.dbclose();
        }
        public void storeTransferIndent(string sno)
        {
            string indent = "UPDATE dispindent SET issued='Nil' WHERE sno='" + sno + "'";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(indent, db.con);
            db.dbclose();
        }
        public void loadStoreIndentStore(string indent_no)
        {
            string query = "SELECT medicine_id AS ID, medicine_name AS MedicineName, store_stock AS StoreStock, qty AS IndentQty, sno AS UniqueID FROM storeindent WHERE indent_no='" + indent_no + "' AND received='0' ORDER BY MedicineName ASC";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "indentDisp";
            db.da.Fill(table);
            db.dbclose();
        }
        public void storeReceiveIndent(int storeStock, int dispStock, int quantity, string id, string comments, string username, string receiver, string indentNo, string sno)
        {
            string update = "UPDATE medicines SET store_stock= store_stock + " + (quantity) + " WHERE medicine_id='" + id + "'";
            string insert = "INSERT INTO " + id + " (office, transaction_id, username, sender, receiver, type, quantity, store_stock, disp_stock, comments) VALUES ('store', '" + DateTime.Today.ToString("yyyyMMdd") + "', '" + username + "', 'IITR Hospital', 'store', 'received', " + quantity + ", (SELECT store_stock FROM medicines WHERE medicine_id='" + id + "'), (SELECT disp_stock FROM medicines WHERE medicine_id='" + id + "'), 'Received Indent No. :" + comments + "')";
            string indent = "UPDATE storeindent SET received='" + quantity + "', comment='" + comments + "' WHERE sno='"+sno+"'";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(update, db.con);
            db.command_nonquery(insert, db.con);
            db.command_nonquery(indent, db.con);
            db.dbclose();
        }
        public void storeReceiveIndent(string sno)
        {
            string indent = "UPDATE storeindent SET received='Nil', comment='Not Received' WHERE sno='" + sno + "'";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(indent, db.con);
            db.dbclose();
        }
        public void expiredStock(int storeStock, int quantity, string id, string comments, string username, string receiver, string medicine_name)
        {
            string update = "UPDATE medicines SET store_stock=" + (storeStock - quantity) + " WHERE medicine_id='" + id + "'";
            string insert = "INSERT INTO " + id + " (office, transaction_id, username, sender, receiver, type, quantity, store_stock, disp_stock, comments) VALUES ('store', '" + DateTime.Today.ToString("yyyyMMdd") + "', '" + username + "', 'store', '" + receiver + "', 'sent', " + quantity + ", " + (storeStock - quantity) + ", (SELECT disp_stock FROM medicines WHERE medicine_id='"+id+"'), 'Request Details: " + comments + "')";
            string expired = "INSERT INTO expired (office, transaction_id, medicine_id, medicine_name, quantity, user) VALUES ('store', '" + DateTime.Today.ToString("yyyyMMdd") + "', '"+id+"', '"+medicine_name+"', "+quantity+", '"+username+"')";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(update, db.con);
            db.command_nonquery(insert, db.con);
            db.command_nonquery(expired, db.con);
            db.dbclose();
        }
        public void loadExpiredMedicine(string office)
        {
            string query = "SELECT medicine_id AS ID, medicine_name AS Medicine_Name, medicine_type AS Medicine_Type, SUM(quantity) AS Total_Quantity FROM expired WHERE office='" + office + "' GROUP BY medicine_id ORDER BY Medicine_Name";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "expiredStock";
            db.da.Fill(table);
            db.dbclose();
        }
        public void expiredStockReport(string office, string startDate, string endDate, dbconnect db)
        {
            string query = "SELECT medicine_name, medicine_type, quantity, timestamp, user FROM expired WHERE office='" + office + "' AND transaction_id>='" + startDate + "' AND transaction_id<'" + endDate + "'";
            db.command_reader(query, db.con);
            dr = db.dr;
        }
        public void dispExpiredStock(int dispStock, int quantity, string id, string comments, string username, string receiver, string medicine_name)
        {
            string update = "UPDATE medicines SET disp_stock=" + (dispStock - quantity) + " WHERE medicine_id='" + id + "'";
            string insert = "INSERT INTO " + id + " (office, transaction_id, username, sender, receiver, type, quantity, store_stock, disp_stock, comments) VALUES ('dispensary', '" + DateTime.Today.ToString("yyyyMMdd") + "', '" + username + "', 'dispensary', '" + receiver + "', 'sent', " + quantity + ", (SELECT store_stock FROM medicines WHERE medicine_id='" + id + "'), " + (dispStock - quantity) + ", 'Request Details: " + comments + "')";
            string expired = "INSERT INTO expired (office, transaction_id, medicine_id, medicine_name, quantity, user) VALUES ('dispensary', '" + DateTime.Today.ToString("yyyyMMdd") + "', '" + id + "', '" + medicine_name + "', " + quantity + ", '" + username + "')";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(update, db.con);
            db.command_nonquery(insert, db.con);
            db.command_nonquery(expired, db.con);
            db.dbclose();
        }
    }
}
