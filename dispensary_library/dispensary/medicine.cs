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
    public class medicine
    {
        //public SqlDataReader dr;
        public MySqlDataReader dr;
        public string error;
        public string count;
        public string check;
        public string issued;
        public string received;
        public string closing;
        public string opening;
        public DataTable table;
        public void addMedicine(string id,string name,string type,int bal,string user)
        {
            dbconnect db = new dbconnect("medicines");
            string query1 = "INSERT INTO medicines (medicine_id, medicine_name, medicine_type, store_stock, disp_stock, created_by) VALUES ('" + id + "','" + name + "','" + type + "'," + bal + ",0,'" + user + "')";
           // string query2 = "CREATE TABLE " + id + " (sno bigint IDENTITY(1,1) NOT NULL,office varchar(50) NULL,transaction_id varchar(50) NULL,username varchar(50) NOT NULL,sender varchar(50) NULL,receiver varchar(50) NULL,type varchar(50) NOT NULL,quantity bigint NOT NULL,store_stock bigint NOT NULL,disp_stock bigint NOT NULL,comments varchar(MAX) NOT NULL,timestamp datetime NULL)";

            string query2 = "CREATE TABLE " + id + " (sno INT( 11 ) NOT NULL AUTO_INCREMENT ,office VARCHAR( 255 ) NULL ,transaction_id VARCHAR( 255 ) NULL ,username VARCHAR( 255 ) NOT NULL ,sender VARCHAR( 255 ) NULL ,receiver VARCHAR( 255 ) NULL ,type VARCHAR( 255 ) NOT NULL ,quantity INT( 11 ) NOT NULL ,store_stock INT( 11 ) NOT NULL ,disp_stock INT( 11 ) NOT NULL ,comments VARCHAR( 255 ) NOT NULL ,timestamp TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ,PRIMARY KEY (sno))";
            
            string query3 = "INSERT INTO " + id + "(office, transaction_id, username, sender, receiver, type, quantity, store_stock, disp_stock, comments) VALUES('store','','" + user + "','','','opening'," + bal + "," + bal + ",0,'" + DateTime.Today.ToString("yyyy-MM-dd") + "')";
            string query4 = "INSERT INTO " + id + "(office, transaction_id, username, sender, receiver, type, quantity, store_stock, disp_stock, comments) VALUES('dispensary','','" + user + "','','','opening',0,0,0,'" + DateTime.Today.ToString("yyyy-MM-dd") + "')";
            check = query2;
            db.command_nonquery(query1, db.con);
            db.command_nonquery(query2, db.con);
            error = db.reader;
            db.command_nonquery(query3, db.con);
            db.command_nonquery(query4, db.con);
            //error = db.reader;
            db.dbclose();
        }
        public void editMedicine(string id, string name, string type)
        {
            dbconnect db = new dbconnect("medicines");
            string query = "UPDATE medicines SET medicine_name='"+name+"',medicine_type='"+type+"' WHERE medicine_id='"+id+"'";
            db.command_nonquery(query, db.con);
            error = db.reader;
            db.dbclose();
        }
        public void listMedicine()
        {
            string qu = "SELECT medicine_id,medicine_name,medicine_type FROM medicines ORDER BY medicine_name ASC";
            table = new DataTable();
            
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(qu, db.con);
            table.TableName = "medicineList";
            db.da.Fill(table);
            table.Columns.Add("medicineDetail", typeof(string), "medicine_name + ' -- ' + medicine_type");
            db.dbclose();
        }
        public void listMedicine(string like)
        {
            dbconnect db = new dbconnect("medicines");
            string query = "SELECT * FROM medicines WHERE medicine_name LIKE '%" + like + "%' ORDER BY medicine_name ASC";
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "medicineList";
            db.da.Fill(table);
            table.Columns.Add("medicineDetail", typeof(string), "medicine_name + ' -- ' + medicine_type");
            db.dbclose();
        }
        public void loadID()
        {
            string load = "SELECT max(sno) FROM medicines";
            dbconnect db = new dbconnect("medicines");
            db.command_reader(load, db.con);
            if (db.dr.Read())
            {
                if (db.dr[0].ToString() == "")
                {
                    count = "1";
                }
                else
                {
                    count = (Convert.ToInt32(db.dr[0].ToString()) + 1).ToString();
                }
            }
            db.dbclose();
        }
        public void fullMedicineList()
        {
            string qu = "SELECT medicine_id AS MedicineID,medicine_name AS MedicineName,medicine_type AS MedicineType,timestamp AS CreatedOn, created_by AS CreatedBy FROM medicines ORDER BY medicine_name ASC";
            table = new DataTable();

            dbconnect db = new dbconnect("medicines");
            db.data_adapter(qu, db.con);
            table.TableName = "medicineList";
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadMedicine()
        {
            string query = "SELECT medicine_id AS ID,medicine_name AS Medicine_Name,medicine_type AS Medicine_Type,store_stock AS Store_Stock, disp_stock AS Dispensary_Stock FROM medicines ORDER BY Medicine_Name";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "patient";
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadMedicine(string like)
        {
            string query = "SELECT medicine_id AS ID,medicine_name AS Medicine_Name,medicine_type AS Medicine_Type,store_stock AS Store_Stock, disp_stock AS Dispensary_Stock FROM medicines WHERE Medicine_Name LIKE '%" + like + "%' ORDER BY Medicine_Name";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "patient";
            db.da.Fill(table);
            db.dbclose();
        }
        public void countQty(string type, string id,string from)
        {
            string query = "SELECT SUM(quantity) FROM " + id + " WHERE type='" + type + "' AND office='" + from + "'";
            dbconnect db = new dbconnect("medicines");
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                if (db.dr[0].ToString() != "")
                {
                    count = db.dr[0].ToString();
                }
                else
                {
                    count = "0";
                }
            }
            db.dbclose();
        }
        public void medicineDetail(string id,dbconnect db)
        {
            string query = "SELECT * FROM medicines WHERE medicine_id='" + id + "'";
            db.command_reader(query, db.con);
            dr = db.dr;
        }
        public void loadMedicineDisp()
        {
            string query = "SELECT medicine_id AS ID,medicine_name AS MedicineName,medicine_type AS MedicineType,disp_stock AS DispStock FROM medicines ORDER BY MedicineName";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "patient";
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadMedicineDisp(string like)
        {
            string query = "SELECT medicine_id AS ID,medicine_name AS MedicineName,medicine_type AS MedicineType, disp_stock AS DispStock FROM medicines WHERE Medicine_Name LIKE '%" + like + "%' ORDER BY MedicineName";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "patient";
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadDispIndent()
        {
            string query = "SELECT medicine_id AS ID, medicine_name AS MedicineName, disp_stock AS DispStock, qty AS IndentQty FROM dispindent WHERE indent_no='" + DateTime.Now.ToString("yyyyMMdd") + "' ORDER BY MedicineName ASC";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "indentDisp";
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadDispIndentIssued(string indentNo)
        {
            string query = "SELECT medicine_id AS ID, medicine_name AS MedicineName, disp_stock AS DispStock, qty AS IndentQty, issued AS IssuedQty FROM dispindent WHERE indent_no='" + indentNo + "' ORDER BY medicine_name ASC";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "indentDisp";
            db.da.Fill(table);
            db.dbclose();
        }
        public void generateDispIndent(string medicineID,string medicineName,string Qty,string dispStock,string username)
        {
            string query = "INSERT INTO dispindent SET indent_no='" + DateTime.Today.ToString("yyyyMMdd") + "',  medicine_id='" + medicineID + "', medicine_name='" + medicineName + "', qty=" + Qty + ",disp_stock=" + dispStock + ",date='" + DateTime.Today.ToString("yyyy-MM-dd") + "', username='" + username + "'";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(query, db.con);
            db.dbclose();
        }
        
        public void storeQty(string id)
        {
            string query = "SELECT store_stock FROM medicines WHERE medicine_id='"+id+"'";
            dbconnect db = new dbconnect("medicines");
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                count = db.dr[0].ToString();
            }
            db.dbclose();
        }
        public void loadMedicineStore()
        {
            string query = "SELECT medicine_id AS ID,medicine_name AS MedicineName,medicine_type AS MedicineType,store_stock AS StoreStock FROM medicines ORDER BY MedicineName";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "patient";
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadMedicineStore(string like)
        {
            string query = "SELECT medicine_id AS ID,medicine_name AS MedicineName,medicine_type AS MedicineType, store_stock AS StoreStock FROM medicines WHERE Medicine_Name LIKE '%" + like + "%' ORDER BY MedicineName";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "patient";
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadStoreIndent()
        {
            string query = "SELECT medicine_id AS ID, medicine_name AS MedicineName, store_stock AS StoreStock, qty AS IndentQty FROM storeindent WHERE indent_no='" + DateTime.Now.ToString("yyyyMMdd") + "' ORDER BY MedicineName ASC";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "indentDisp";
            db.da.Fill(table);
            db.dbclose();
        }
        public void generateStoreIndent(string medicineID, string medicineName, string Qty, string storeStock, string username)
        {
            string query = "INSERT INTO storeindent SET indent_no='" + DateTime.Today.ToString("yyyyMMdd") + "', medicine_id='" + medicineID + "', medicine_name='" + medicineName + "', qty=" + Qty + ",store_stock=" + storeStock + ",date='" + DateTime.Today.ToString("yyyy-MM-dd") + "', username='" + username + "'";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(query, db.con);
            db.dbclose();
        }
        public void generateStoreIndentNo()
        {
            string query = "INSERT INTO storeindentno SET indent_no='" + DateTime.Today.ToString("yyyyMMdd") + "'";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(query, db.con);
            db.dbclose();
        }
        public void getStoreIndentNo(string indentNo)
        {
            string query = "SELECT sno FROM storeindentno WHERE indent_no='"+indentNo+"'";
            dbconnect db = new dbconnect("medicines");
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                count = db.dr[0].ToString();
            }
            db.dbclose();
        }
        public void loadStoreIndentReceived(string indentNo)
        {
            string query = "SELECT medicine_id AS ID, medicine_name AS MedicineName, store_stock AS StoreStock, qty AS IndentQty, received AS ReceivedQty FROM storeindent WHERE indent_no='" + indentNo + "' ORDER BY medicine_name ASC";
            dbconnect db = new dbconnect("medicines");
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "indentStore";
            db.da.Fill(table);
            db.dbclose();
        }
        public void dispDailyExpenses(string date, string id)
        {
            string query = "SELECT SUM(IF(office='dispensary' AND type='sent',quantity,0)) AS issued, SUM(IF(office='store' AND receiver='dispensary',quantity,0)) AS received, SUM(IF(timestamp=(SELECT max(timestamp) FROM " + id + " WHERE transaction_id='" + date + "'),disp_stock,NULL)) AS closing FROM " + id + " WHERE transaction_id='" + date + "'";
            string open = "SELECT disp_stock FROM " + id + " WHERE timestamp=(SELECT max(timestamp) FROM " + id + " WHERE (transaction_id<'" + date + "') OR (type='opening' AND transaction_id='" + date + "'))";
            dbconnect db = new dbconnect("medicines");
            db.command_reader(open, db.con);
            if (db.dr.Read())
            {
                opening = db.dr[0].ToString();
            }
            db.reader_close(); 
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                issued = db.dr[0].ToString();
                received = db.dr[1].ToString();
                if (db.dr[2].ToString() != "")
                {
                    closing = db.dr[2].ToString();
                }
                else
                {
                    closing = opening;
                }
            }
            db.reader_close();
            db.dbclose();
        }
        public void dispMonthlyExpenses(string startDate, string endDate, string id)
        {
            string query = "SELECT SUM(IF(office='dispensary' AND type='sent',quantity,0)) AS issued, SUM(IF(office='store' AND receiver='dispensary',quantity,0)) AS received, SUM(IF(timestamp=(SELECT max(timestamp) FROM " + id + " WHERE transaction_id<'" + endDate + "'),disp_stock,NULL)) AS closing FROM " + id + " WHERE transaction_id>='" + startDate + "' AND transaction_id<'" + endDate + "'";
            string open = "SELECT disp_stock FROM " + id + " WHERE timestamp=(SELECT max(timestamp) FROM " + id + " WHERE (transaction_id<'" + startDate + "') OR (type='opening' AND transaction_id='" + startDate + "'))";
            dbconnect db = new dbconnect("medicines");
            db.command_reader(open, db.con);
            if (db.dr.Read())
            {
                opening = db.dr[0].ToString();
            }
            db.reader_close();
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                issued = db.dr[0].ToString();
                received = db.dr[1].ToString();
                if (db.dr[2].ToString() != "")
                {
                    closing = db.dr[2].ToString();
                }
                else
                {
                    closing = opening;
                }
            }
            db.reader_close();
            db.dbclose();
        }
        public void storeDailyExpenses(string date, string id)
        {
            string query = "SELECT SUM(IF(office='store' AND type='sent',quantity,0)) AS issued, SUM(IF(office='store' AND receiver='store',quantity,0)) AS received, SUM(IF(timestamp=(SELECT max(timestamp) FROM " + id + " WHERE transaction_id='" + date + "' AND office='store') AND office='store',store_stock,NULL)) AS closing FROM " + id + " WHERE transaction_id='" + date + "' AND office='store'";
            string open = "SELECT store_stock FROM " + id + " WHERE timestamp=(SELECT max(timestamp) FROM " + id + " WHERE (transaction_id<'" + date + "') OR (type='opening' AND transaction_id='" + date + "'))";
            dbconnect db = new dbconnect("medicines");
            db.command_reader(open, db.con);
            if (db.dr.Read())
            {
                opening = db.dr[0].ToString();
            }
            db.reader_close();
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                issued = db.dr[0].ToString();
                received = db.dr[1].ToString();
                if (db.dr[2].ToString() != "")
                {
                    closing = db.dr[2].ToString();
                }
                else
                {
                    closing = opening;
                }
            }
            db.reader_close();
            db.dbclose();
        }
        public void storeMonthlyExpenses(string startDate, string endDate, string id)
        {
            string query = "SELECT SUM(IF(office='store' AND type='sent',quantity,0)) AS issued, SUM(IF(office='store' AND receiver='store',quantity,0)) AS received, SUM(IF(timestamp=(SELECT max(timestamp) FROM " + id + " WHERE transaction_id<'" + endDate + "' AND office='store') AND office='store',store_stock,NULL)) AS closing FROM " + id + " WHERE transaction_id>='" + startDate + "' AND transaction_id<'" + endDate + "'";
            string open = "SELECT store_stock FROM " + id + " WHERE timestamp=(SELECT max(timestamp) FROM " + id + " WHERE (transaction_id<'" + startDate + "') OR (type='opening' AND transaction_id='" + startDate + "'))";
            dbconnect db = new dbconnect("medicines");
            db.command_reader(open, db.con);
            if (db.dr.Read())
            {
                opening = db.dr[0].ToString();
            }
            db.reader_close();
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                issued = db.dr[0].ToString();
                received = db.dr[1].ToString();
                if (db.dr[2].ToString() != "")
                {
                    closing = db.dr[2].ToString();
                }
                else
                {
                    closing = opening;
                }
            }
            db.reader_close();
            db.dbclose();
        }
        public void medicineTransferDetails(string id, dbconnect db)
        {
            string query = "SELECT * FROM "+ id +" WHERE office='dispensary' OR (office='store' AND receiver='dispensary')";
            db.command_reader(query, db.con);
            dr = db.dr;
        }
    }
}
