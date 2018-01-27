using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispensary;

namespace WindowsFormsApplication1
{
    public partial class dispensaryMain : Form
    {
        string username;
        public int opdCount = 0;
        public int medicineCount = 0;
        public string[] medicine_id = new string[0];
        public string[] medicines = new string[0];
        public string[] dosage = new string[0];
        public string[] quantity = new string[0];
        public string name;
        public string gender;
        public string id;
        public string age;
        public string familyHead = "";
        public dispensaryMain(string user)
        {
            InitializeComponent();
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd MMM, yyyy";
            username = user;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            ((Form)printPreviewDialog1).StartPosition = FormStartPosition.CenterScreen;
            ((Form)printPreviewDialog1).Width = 880;
            ((Form)printPreviewDialog1).Height = 680;
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form registration = new Registration(username);
            registration.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form change = new changePassword(username);
            change.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form stockReport = new dispStockReport(username);
            stockReport.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form indent = new generateDispIndent(username);
            indent.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form indentHistory = new dispIndentHistory(username);
            indentHistory.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.dailyReportDoctor(dateTimePicker2.Value.Date.ToString("yyyyMMdd"), db);
            opdCount = 0;
            medicineCount = 0;
            while (opd.dr.Read())
            {
                medicine_id = opd.dr[9].ToString().Split(',');
                medicineCount += medicine_id.Length;
                opdCount += 1;
            }
            //MessageBox.Show(medicineCount.ToString());
            db.dbclose();
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, Convert.ToInt32(Math.Round((38 + (27 + 5) * opdCount + medicineCount * 6 + 8) * 3.9, 1, MidpointRounding.AwayFromZero)));
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 14, System.Drawing.GraphicsUnit.Point);
            g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
            g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
            messageFont = new Font("Times New Roman", 20, System.Drawing.GraphicsUnit.Point);
            g.DrawString("DAILY REPORT", messageFont, Brushes.Black, 75, 25);
            messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Dated: " + dateTimePicker2.Value.Date.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 30);
            g.DrawString("Doctor: Dr. Manoj Jain", messageFont, Brushes.Black, 15, 30);
            Pen blackPen = new Pen(Color.Black, 1);
            g.DrawLine(blackPen, 5, 38, 205, 38);

            int total = 38;
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.dailyReportDoctor(dateTimePicker2.Value.Date.ToString("yyyyMMdd"), db);
            int sno = 1;
            while (opd.dr.Read())
            {
                int age = DateTime.Today.Year - Convert.ToInt32(db.dr[4].ToString().Substring(0, 4));
                if (Convert.ToInt32(db.dr[4].ToString().Substring(5, 2)) > DateTime.Today.Month)
                {
                    age--;
                }
                medicine_id = new string[0];
                medicines = new string[0];
                dosage = new string[0];
                quantity = new string[0];
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Patient Name: " + opd.dr[3].ToString(), messageFont, Brushes.Black, 20, total + 4);
                g.DrawString("Patient ID: " + opd.dr[0].ToString(), messageFont, Brushes.Black, 55, total + 11);
                g.DrawString("Age: " + age + " Years", messageFont, Brushes.Black, 110, total + 4);
                g.DrawString("Gender: " + opd.dr[5].ToString(), messageFont, Brushes.Black, 150, total + 4);
                g.DrawString("Family Head ID: " + opd.dr[6].ToString(), messageFont, Brushes.Black, 110, total + 11);
                g.DrawString(sno + ")", messageFont, Brushes.Black, 10, total + 4);
                g.DrawString("OPD: " + opd.dr[7].ToString(), messageFont, Brushes.Black, 20, total + 11);
                g.DrawString("Symptoms: " + opd.dr[1].ToString(), messageFont, Brushes.Black, 20, total + 18);
                g.DrawString("Remarks: " + opd.dr[2].ToString(), messageFont, Brushes.Black, 110, total + 18);
                total += 27;

                medicine_id = opd.dr[8].ToString().Split(',');
                medicines = opd.dr[9].ToString().Split(',');
                dosage = opd.dr[10].ToString().Split(',');
                quantity = opd.dr[11].ToString().Split(',');
                for (int i = 0; i < medicine_id.Length; i++)
                {
                    g.DrawString((i + 1) + ".", messageFont, Brushes.Black, 20, total);
                    g.DrawString(medicines[i] + " -- ( " + quantity[i] + " ) -- ( " + dosage[i] + " )", messageFont, Brushes.Black, 27, total);
                    total += 6;
                }
                if (sno < opdCount)
                {
                    messageFont = new Font("Times New Roman", 8, FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                    g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ", messageFont, Brushes.Black, 10, total - 1);
                    total += 3;
                }
                sno++;
            }
            db.dbclose();
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form issue = new issueMedicines(username);
            issue.ShowDialog();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Form emergency = new doctorMain(username);
            emergency.ShowDialog();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Form expired = new dispExpiredMedicines(username);
            expired.ShowDialog();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Form expiredReports = new dispensaryExpiredMedicinesReport(username);
            expiredReports.ShowDialog();
        }
    }
}
