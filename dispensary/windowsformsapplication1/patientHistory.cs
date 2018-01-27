using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispensary;

namespace WindowsFormsApplication1
{
    public partial class patientHistory : UserControl
    {
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
        public patientHistory()
        {
            InitializeComponent();
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            ((Form)printPreviewDialog1).StartPosition = FormStartPosition.CenterScreen;
            ((Form)printPreviewDialog1).Width = 880;
            ((Form)printPreviewDialog1).Height = 680;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool details = false;
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.getPatientDetailsHistory(textBox1.Text, db);
            if (opd.dr.Read())
            {
                id = textBox1.Text;
                name = opd.dr[0].ToString();
                gender = opd.dr[1].ToString();
                familyHead = opd.dr[2].ToString();
                details = true;

            }
            else
            {
                MessageBox.Show("Invalid Patient ID!!");
                db.dbclose();
            }
            db.reader_close();
            if (details)
            {
                opdCount = 0;
                medicineCount = 0;
                opd.patientAge(id);
                age = opd.age.ToString();
                opd.getPatientOPDHistory(textBox1.Text, db);
                while (opd.dr.Read())
                {
                    medicine_id = opd.dr[2].ToString().Split(',');
                    medicineCount += medicine_id.Length;
                    opdCount += 1;
                }
                db.dbclose();
                printPreviewDialog1.ShowDialog();
            }
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, Convert.ToInt32(Math.Round((56 + (20 + 5) * opdCount + medicineCount * 6 + 8) * 3.9, 1, MidpointRounding.AwayFromZero)));
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
            g.DrawString("PATIENT MEDICAL HISTORY", messageFont, Brushes.Black, 60, 25);
            Pen blackPen = new Pen(Color.Black, 1);
            g.DrawLine(blackPen, 5, 38, 205, 38);
            messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Patient Name: "+name, messageFont, Brushes.Black, 10, 42);
            g.DrawString("Patient ID: " + id, messageFont, Brushes.Black, 10, 49);
            g.DrawString("Age: "+age+" Years", messageFont, Brushes.Black, 120, 42);
            g.DrawString("Gender: "+gender, messageFont, Brushes.Black, 160, 42);
            if (familyHead != "")
            {
                g.DrawString("Family Head ID: " + familyHead, messageFont, Brushes.Black, 120, 49);
            }
            g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 30);
            g.DrawLine(blackPen, 5, 56, 205, 56);
            int total = 56;
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.getPatientOPDHistory(textBox1.Text,db);
            int sno = 1;
            while (opd.dr.Read())
            {
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                medicine_id = new string[0];
                medicines = new string[0];
                dosage = new string[0];
                quantity = new string[0];
                g.DrawString(sno+")", messageFont, Brushes.Black, 10, total + 2);
                g.DrawString("OPD: "+opd.dr[0].ToString(), messageFont, Brushes.Black, 20, total + 2);
                g.DrawString("Doctor: " + opd.dr[8].ToString(), messageFont, Brushes.Black, 55, total + 2);
                g.DrawString("Symptoms: " + opd.dr[6].ToString(), messageFont, Brushes.Black, 105, total + 2);
                g.DrawString("Visit Date: " + opd.dr[1].ToString().Substring(0, 4) + "-" + opd.dr[1].ToString().Substring(4, 2) + "-" + opd.dr[1].ToString().Substring(6, 2), messageFont, Brushes.Black, 20, total + 10);
                g.DrawString("Remarks: " + opd.dr[7].ToString(), messageFont, Brushes.Black, 70, total + 10);
                medicine_id = opd.dr[2].ToString().Split(',');
                medicines = opd.dr[3].ToString().Split(',');
                dosage = opd.dr[4].ToString().Split(',');
                quantity = opd.dr[5].ToString().Split(',');
                total += 20;
                for (int i = 0; i < medicine_id.Length; i++)
                {
                    g.DrawString((i+1)+".", messageFont, Brushes.Black, 20, total);
                    g.DrawString(medicines[i] + " -- ( "+quantity[i]+" ) -- ( "+dosage[i]+" )", messageFont, Brushes.Black, 27, total);
                    total += 6;
                }
                if (sno < opdCount)
                {
                    messageFont = new Font("Times New Roman", 8,FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
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
    }
}
