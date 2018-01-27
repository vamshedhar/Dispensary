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
    public partial class duplicateToken : UserControl
    {
        public duplicateToken()
        {
            InitializeComponent();
        }
        public string opd;
        public string token;
        public string doc;
        public string code;
        public int age;
        public string cat;
        public string name;
        private void button7_Click(object sender, EventArgs e)
        {
            //SELECT patients.name, token.* FROM patients INNER JOIN token ON patients.patient_id=token.patient_id WHERE patient_id='"+textBox4.Text+"' AND date='"+DateTime.Today.ToString("yyyy-MM-dd")+"'
            //SELECT * FROM token WHERE patient_id='"+textBox4.Text+"' AND date='"+DateTime.Today.ToString("yyyy-MM-dd")+"'

            string query2 = "SELECT name, dob FROM patients WHERE patient_id='" + textBox4.Text + "'";
            dbconnect db2 = new dbconnect();
            db2.command_reader(query2, db2.con);
            // MessageBox.Show(db.reader);
            if (db2.dr.Read())
            {
                name = db2.dr[0].ToString();
                age = DateTime.Today.Year - Convert.ToInt32(db2.dr[1].ToString().Substring(0, 4));
                //MessageBox.Show(age.ToString());
                if (Convert.ToInt32(db2.dr[1].ToString().Substring(5, 2)) > DateTime.Today.Month)
                {
                    age--;
                }
            }
            

            string query = "SELECT * FROM  token WHERE patient_id='" + textBox4.Text + "' AND date='" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            dbconnect db = new dbconnect("opd");
            db.command_reader(query, db.con);
           // MessageBox.Show(db.reader);
            if (db.dr.Read())
            {
                opd = db.dr[0].ToString();
                token = db.dr[2].ToString();
                doc = db.dr[7].ToString();
                code = db.dr[8].ToString();
                cat = db.dr[6].ToString();
                printPreviewDialog1.ShowDialog();
                textBox4.Text = "";
            }
            else
            {
                MessageBox.Show("No token was issued for following Patient today.");
            }
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("token", 325, 315);
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Pixel;
            Font messageFont = new Font("Times New Roman", 11, System.Drawing.GraphicsUnit.Point);

            g.DrawString("DISPENSARY, IIT ROORKEE", messageFont, Brushes.Black, 350, 100);
            g.DrawString("SAHARANPUR CAMPUS", messageFont, Brushes.Black, 400, 210);
            g.DrawString("(Duplicate Token Slip)", messageFont, Brushes.Black, 500, 330);
            g.DrawString("----------------------------------------------------------", messageFont, Brushes.Black, 50, 440);
            g.DrawString("Date : " + DateTime.Now.ToString("MMM dd, yyyy hh:mm tt"), messageFont, Brushes.Black, 650, 550);
            g.DrawString("OPD No. : " + opd , messageFont, Brushes.Black, 100, 750);
            g.DrawString("CODE : " + code, messageFont, Brushes.Black, 1120, 750);
            g.DrawString("Name : " + name, messageFont, Brushes.Black, 100, 950);
            g.DrawString("Age : "+age+" Years", messageFont, Brushes.Black, 100, 1070);
           if (cat == "general")
            { g.DrawString("Fee Collected : Rs. 1 Only.", messageFont, Brushes.Black, 100, 1190); }
            else 
            { g.DrawString("Fee Collected : Rs. 0 Only.", messageFont, Brushes.Black, 100, 1190); }
            g.DrawString(doc, messageFont, Brushes.Black, 100, 1430);
            g.DrawString("Token No. :  "+token, messageFont, Brushes.Black, 1120, 1430);
            
            g.DrawString("----------------------------------------------------------", messageFont, Brushes.Black, 50, 1570);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 350, 1690);
        }
    }
}
