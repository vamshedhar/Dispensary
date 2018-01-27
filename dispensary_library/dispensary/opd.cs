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
    public class opd
    {
        public string token;
        public string opdMax;
        public string amount;
        public int age;
        public string visits;
        public string dob;
        public DataTable table;
        public MySqlDataReader dr;
        public string message;
        public string count;
        public void tokenLoad()
        {
            string tokenLoad = "SELECT max(token_no) FROM token WHERE date='" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            string opdLoad = "SELECT max(opd) FROM token";
            string amountLoad = "SELECT count(opd) FROM token WHERE date='" + DateTime.Today.ToString("yyyy-MM-dd") + "' AND category='general'";
            dbconnect db = new dbconnect();
            db.command_reader(tokenLoad, db.con);
            if (db.dr.Read())
            {
                if (db.dr[0].ToString() == "")
                {
                    token = "0";
                }
                else
                {
                    token = db.dr[0].ToString();
                }
            }
            db.reader_close();
            db.command_reader(opdLoad, db.con);
            if (db.dr.Read())
            {
                if (db.dr[0].ToString() == "")
                {
                    opdMax = "0";
                }
                else
                {
                    opdMax = db.dr[0].ToString();
                }
            }
            db.reader_close();
            db.command_reader(amountLoad, db.con);
            if (db.dr.Read())
            {
                if (db.dr[0].ToString() == "")
                {
                    amount = "0";
                }
                else
                {
                    amount = db.dr[0].ToString();
                }
            }
            db.reader_close();
            db.dbclose();
        }
        public void addPatient(string patientID, string name,string dob,string gender,string familyHead,string cat)
        {
            dbconnect db = new dbconnect();
            //string insert = "INSERT INTO patients VALUES ('" + patientID + "','" + name + "','" + dob + "','" + gender + "','" + familyHead + "','" + cat + "')";
            string insert = "INSERT INTO patients SET patient_id='" + patientID + "',name='" + name + "',dob='" + dob + "',gender='" + gender + "',family_head='" + familyHead + "',category='" + cat + "'";
            db.command_nonquery(insert, db.con);
            if (db.reader != "Sucess")
            {
                message = "Details already exist with following Patient ID.";
                //message = db.reader;
            }
            else
            {
                message = "Details added Sucessfully!";
            }
            db.dbclose();
        }
        public void updatePatient(string ID,string name,string dob,string gender,string familyHead,string cat)
        {
            string query = "UPDATE patients SET name='" + name + "', dob='" + dob + "', gender='" + gender + "', family_head='" + familyHead + "', category='" + cat + "' WHERE patient_id='" + ID + "'";
            dbconnect db = new dbconnect();
            db.command_nonquery(query, db.con);
            if (db.reader != "Sucess")
            {
                message = "Some Error occured!! Please Check if all Details are Valid!!";
            }
            else
            {
                message = "Details UPDATED Sucessfully!!";
            }
            db.dbclose();
        }
        public void loadPatients()
        {
            string query = "SELECT patient_id AS Patient_ID,name AS Patient_Name,gender AS Gender,family_head AS Family_Head,category AS Category FROM patients ORDER BY Patient_Name ASC";
            dbconnect db = new dbconnect();
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "patient";
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadPatients(string like)
        {
            string query = "SELECT patient_id AS Patient_ID,name AS Patient_Name,gender AS Gender,family_head AS Family_Head,category AS Category FROM patients WHERE patient_id LIKE '" + like + "%' ORDER BY name ASC";
            dbconnect db = new dbconnect();
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "patient";
            db.da.Fill(table);
            db.dbclose();
        }
        public void patientAge(string ID)
        {
            dbconnect db = new dbconnect();
            db.command_reader("SELECT dob FROM patients WHERE patient_id ='" + ID + "'", db.con);
            if (db.dr.Read())
            {
                dob = db.dr[0].ToString();
                age = DateTime.Today.Year - Convert.ToInt32(db.dr[0].ToString().Substring(0, 4));
                if (Convert.ToInt32(db.dr[0].ToString().Substring(5, 2)) > DateTime.Today.Month)
                {
                    age--;
                }
            }
            db.dbclose();
        }
        public void totalPatientVisits(string ID)
        {
            dbconnect db = new dbconnect();
            db.command_reader("SELECT COUNT(opd) FROM opd WHERE patient_id ='" + ID + "'", db.con);
            if (db.dr.Read())
            {
                visits = db.dr[0].ToString();
            }
            db.dbclose();
        }
        public void issueToken(string username,string tokenNo,string patientID,string category,string doctor,string code)
        {
            string issue = "INSERT INTO token (transaction_id, date, token_no, patient_id, category, doctor, token_key) VALUES ('" + username + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "'," + tokenNo + ",'" + patientID + "','" + category + "','" + doctor + "'," + code + ")";
            dbconnect insert = new dbconnect();
            insert.command_nonquery(issue, insert.con);
            insert.dbclose();
        }
        public void cancelToken(string opd)
        {
            string query = "DELETE FROM token WHERE opd=" + opd;
            dbconnect db = new dbconnect();
            db.command_nonquery(query, db.con);
            db.dbclose();
        }
        public void patientsToVisit()
        {
            string query = "SELECT token.opd AS OPD, token.patient_id AS PatientID, token.date AS Date, patients.name AS PatientName, patients.gender AS Gender, patients.dob AS DateOfBirth, token.token_no AS Token FROM token INNER JOIN patients on token.patient_id=patients.patient_id WHERE token.date='" + DateTime.Today.ToString("yyyy-MM-dd") + "' AND status=0 ORDER BY token.token_no";
            dbconnect db = new dbconnect();
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "PatientList";
            db.da.Fill(table);
            db.dbclose();
        }
        public void patientsToVisit(string token)
        {
            string query = "SELECT token.opd AS OPD, token.patient_id AS PatientID, token.date AS Date, patients.name AS PatientName, patients.gender AS Gender, patients.dob AS DateOfBirth, token.token_no AS Token FROM token INNER JOIN patients on token.patient_id=patients.patient_id WHERE token.date='" + DateTime.Today.ToString("yyyy-MM-dd") + "' AND status=0 AND token.token_no=" + token + " ORDER BY token.token_no";
            dbconnect db = new dbconnect();
            db.data_adapter(query, db.con);
            table = new DataTable();
            table.TableName = "PatientList";
            db.da.Fill(table);
            db.dbclose();
        }
        public void patientsVisited()
        {
            string query = "SELECT COUNT(opd) FROM token WHERE date='" + DateTime.Today.ToString("yyyy-MM-dd") + "' AND status=1";
            dbconnect db = new dbconnect();
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                count = db.dr[0].ToString();
            }
            else
            {
                count = "error";
            }
            db.dbclose();
        }
        public void getCode(string opd)
        {
            string query = "SELECT token_key FROM token WHERE opd=" + opd;
            dbconnect db = new dbconnect();
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                count = db.dr[0].ToString();
            }
            else
            {
                count = db.reader;
            }
            db.dbclose();
        }
        public void getPatientDetails(string opd,dbconnect db)
        {
            string query = "SELECT token.patient_id AS PatientID, patients.name AS PatientName, patients.gender AS Gender, patients.dob AS DateOfBirth, token.token_no AS Token FROM token INNER JOIN patients on token.patient_id=patients.patient_id WHERE token.opd=" + opd;
            db.command_reader(query, db.con);
            dr = db.dr;
            message = db.reader;
        }
        public void getPatientOPDHistory(string ID, dbconnect db)
        {
            string visits = "SELECT opd,date,medicine_id,medicines,dosage,quantity,symptoms,remarks,doctor FROM opd WHERE patient_id='" + ID + "' ORDER BY opd DESC";
            db.command_reader(visits, db.con);
            dr = db.dr;
        }
        public void getPatientOPDHistory(string ID,string startDate,string endDate,dbconnect db)
        {
            string visits = "SELECT opd,date,medicine_id,medicines,dosage,quantity,symptoms,remarks,doctor FROM opd WHERE patient_id='" + ID + "' AND date<='"+endDate+"' AND date>='"+startDate+"' ORDER BY opd DESC";
            db.command_reader(visits, db.con);
            dr = db.dr;
        }
        public void getPatientMedicineHistory(string opd, dbconnect db)
        {
            string query = "SELECT medicine_id,medicines,dosage,quantity FROM opd WHERE opd=" + opd;
            db.command_reader(query, db.con);
            dr = db.dr;
        }
        public void updateIssuedMedicine(string medicineID,string qty,string opd,string patientID,string patientName,string username)
        {
            string query = "UPDATE medicines SET disp_stock=disp_stock-" + qty + " WHERE medicine_id='" + medicineID + "'";
            string insert = "INSERT INTO " + medicineID + " (office,transaction_id,username,sender,receiver,type,quantity,store_stock,disp_stock,comments) VALUES ('dispensary','" + DateTime.Today.ToString("yyyyMMdd") + "','" + username + "','dispensary','" + patientID + "','sent'," + qty + ",(SELECT store_stock FROM medicines WHERE medicine_id='" + medicineID + "'),(SELECT disp_stock FROM medicines WHERE medicine_id='" + medicineID + "'),'Issued To: " + patientName + " (OPD: " + opd + ")')";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(query, db.con);
            db.command_nonquery(insert, db.con);
            db.dbclose();
        }
        public void updateIssuedMedicineStore(string medicineID, string qty, string opd, string patientID, string patientName, string username)
        {
            string query = "UPDATE medicines SET store_stock=store_stock -" + qty + " WHERE medicine_id='" + medicineID + "'";
            string insert = "INSERT INTO " + medicineID + " (office,transaction_id,username,sender,receiver,type,quantity,store_stock,disp_stock,comments) VALUES ('store','" + DateTime.Today.ToString("yyyyMMdd") + "','" + username + "','store','" + patientID + "','sent'," + qty + ",(SELECT store_stock FROM medicines WHERE medicine_id='" + medicineID + "'),(SELECT disp_stock FROM medicines WHERE medicine_id='" + medicineID + "'),'Issued To: " + patientName + " (OPD: " + opd + ")')";
            dbconnect db = new dbconnect("medicines");
            db.command_nonquery(query, db.con);
            db.command_nonquery(insert, db.con);
            db.dbclose();
        }
        public void updateOPD(string OPD, string patientID, string medicinesID, string medicines, string qty, string storeMedicineID, string storeQty, string dosage, string symptoms, string remarks, string doctor)
        {
            string opd = "INSERT INTO opd (opd,patient_id,medicine_id,medicines,dosage,quantity,storemedicine_id,storequantity,symptoms,remarks, doctor,date) VALUES ('" + OPD + "','" + patientID + "','" + medicinesID + "','" + medicines + "','" + dosage + "','" + qty + "','" + storeMedicineID + "','" + storeQty + "','" + symptoms + "','" + remarks + "','" + doctor + "','" + DateTime.Today.ToString("yyyyMMdd") + "')";
            string status = "UPDATE token SET status=1 WHERE opd=" + OPD;
            dbconnect db = new dbconnect();
            db.command_nonquery(opd, db.con);
            db.command_nonquery(status, db.con);
            db.dbclose();
        }
        public void duplicatePatientSlip(string opd, dbconnect db)
        {
            string query = "SELECT opd.patient_id, opd.symptoms, opd.remarks, opd.doctor, opd.date, patients.name, patients.dob, patients.gender, patients.family_head, opd.medicine_id,opd.medicines,opd.dosage,opd.quantity FROM opd INNER JOIN patients ON opd.patient_id=patients.patient_id WHERE opd.opd=" + opd;
            db.command_reader(query, db.con);
            dr = db.dr;
        }
        public void getPatientDetailsHistory(string id,dbconnect db)
        {
            string query = "SELECT name, gender, family_head FROM patients WHERE patient_id='" + id+"'";
            db.command_reader(query, db.con);
            dr = db.dr;
        }
        public void dailyReportDoctor(string date, dbconnect db)
        {
            string query = "SELECT opd.patient_id, opd.symptoms, opd.remarks, patients.name, patients.dob, patients.gender, patients.family_head, opd.opd, opd.medicine_id,opd.medicines,opd.dosage,opd.quantity FROM opd INNER JOIN patients ON opd.patient_id=patients.patient_id WHERE opd.date='" + date + "'";
            db.command_reader(query, db.con);
            dr = db.dr;
        }
        public void dailyReportRegistration(string date, dbconnect db)
        {
            string query = "SELECT token.opd, token.patient_id, patients.name, patients.dob, patients.gender, patients.family_head, patients.category, token.doctor  FROM token INNER JOIN patients ON token.patient_id=patients.patient_id WHERE token.date='" + date +"'";
            db.command_reader(query, db.con);
            dr = db.dr;
        }
        public void monthlyReportRegistration(string startDate, string endDate, dbconnect db)
        {
            string query = "SELECT token.opd, token.patient_id, patients.name, patients.dob, patients.gender, patients.family_head, patients.category, token.doctor  FROM token INNER JOIN patients ON token.patient_id=patients.patient_id WHERE token.date>='" + startDate + "' AND token.date<'"+endDate+"'";
            db.command_reader(query, db.con);
            dr = db.dr;
        }
        public void monthlyReportDatewise(string startDate, string endDate, dbconnect db)
        {
            string query = "SELECT date, count(opd), SUM(IF(category='general',1,0)) AS Amount FROM token WHERE date>='" + startDate + "' AND date<'" + endDate + "' GROUP BY date";
            db.command_reader(query, db.con);
            dr = db.dr;
        }
        public void dailyOPDCount(string date)
        {
            dbconnect db = new dbconnect();
            string query = "SELECT count(opd) FROM token WHERE date='" + date + "'";
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                count = db.dr[0].ToString();
            }
            else
            {
                count = db.reader;
            }
            db.dbclose();
        }
        public void monthlyOPDCount(string startDate, string endDate)
        {
            dbconnect db = new dbconnect();
            string query = "SELECT count(opd) FROM token WHERE date>='" + startDate + "' AND date<'" + endDate + "'";
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                count = db.dr[0].ToString();
            }
            else
            {
                count = db.reader;
            }
            db.dbclose();
        }
        public void registrationStats()
        {
            dbconnect db = new dbconnect();
            string query = "SELECT count(opd) FROM token";
            string amount = "SELECT count(opd) FROM token WHERE category='general'";
            string opd = "SELECT max(opd) FROM token";
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                count = db.dr[0].ToString();
            }
            db.reader_close();
            db.command_reader(amount, db.con);
            if (db.dr.Read())
            {
                token = db.dr[0].ToString();
            }
            db.reader_close();
            db.command_reader(opd, db.con);
            if (db.dr.Read())
            {
                opdMax = db.dr[0].ToString();
            }
            db.reader_close();
            db.dbclose();
        }
        public void annualReport(string startDate, string endDate)
        {
            dbconnect db = new dbconnect();
            string query = "SELECT count(opd) FROM token WHERE date>='"+startDate+"' AND date<'"+endDate+"'";
            string amount = "SELECT count(opd) FROM token WHERE category='general' AND date>='" + startDate + "' AND date<'" + endDate + "'";
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                count = db.dr[0].ToString();
            }
            db.reader_close();
            db.command_reader(amount, db.con);
            if (db.dr.Read())
            {
                token = db.dr[0].ToString();
            }
            db.reader_close();
            db.dbclose();
        }
        public void loadVisitedPatients()
        {
            string query = "SELECT opd.opd AS OPD, opd.patient_id AS PatientID, patients.name AS PatientsName, patients.gender AS Gender, patients.family_head AS FamilyHead, opd.doctor AS Doctor, (opd.timestamp)  AS VisitTime FROM opd INNER JOIN patients ON opd.patient_id = patients.patient_id WHERE opd.date='" + DateTime.Today.ToString("yyyyMMdd") + "' ORDER BY opd.timestamp DESC";
            dbconnect db = new dbconnect();
            db.data_adapter(query, db.con);
            table = new DataTable();
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadVisitedPatients(string opd)
        {
            string query = "SELECT opd.opd AS OPD, opd.patient_id AS PatientID, patients.name AS PatientsName, patients.gender AS Gender, patients.family_head AS FamilyHead, opd.doctor AS Doctor, (opd.timestamp)  AS VisitTime FROM opd INNER JOIN patients ON opd.patient_id = patients.patient_id WHERE opd.date='" + DateTime.Today.ToString("yyyyMMdd") + "' AND opd.opd LIKE '%"+opd+"%' ORDER BY opd.timestamp DESC";
            dbconnect db = new dbconnect();
            db.data_adapter(query, db.con);
            table = new DataTable();
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadVisitedPatientsName(string name)
        {
            string query = "SELECT opd.opd AS OPD, opd.patient_id AS PatientID, patients.name AS PatientsName, patients.gender AS Gender, patients.family_head AS FamilyHead, opd.doctor AS Doctor, (opd.timestamp)  AS VisitTime FROM opd INNER JOIN patients ON opd.patient_id = patients.patient_id WHERE opd.date='" + DateTime.Today.ToString("yyyyMMdd") + "' AND patients.name LIKE '%" + name + "%' ORDER BY opd.timestamp DESC";
            dbconnect db = new dbconnect();
            db.data_adapter(query, db.con);
            table = new DataTable();
            db.da.Fill(table);
            db.dbclose();
        }
        public void loadOPDDetails(string opd,dbconnect db)
        {
            string query = "SELECT opd.opd, opd.patient_id, patients.name, patients.dob, patients.gender, opd.doctor, patients.category, patients.family_head, opd.symptoms, opd.remarks, opd.medicine_id, opd.medicines, opd.dosage, opd.quantity, opd.storemedicine_id, opd.storequantity FROM opd INNER JOIN patients ON opd.patient_id=patients.patient_id WHERE opd.opd="+opd;
            db.command_reader(query, db.con);
            dr = db.dr;
        }
    }
}
