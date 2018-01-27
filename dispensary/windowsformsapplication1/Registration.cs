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
    /*
     * 
     *  Created By -- Vam$hedhar Reddy C
     *  All Rights Reserved
     *  Product of IMG Labs IIT Roorkee, Saharanpur Campus
     *  
     * */

    /// <summary>
    /// 
    /// Registration Home.
    /// Main Features:
    ///     -- Issue Token
    ///     -- Cancel Token
    ///     -- Add Patient Details
    ///     -- Edit Patient Details
    ///     -- Duplicate Patient Slip
    /// 
    /// </summary>
    public partial class Registration : Form
    {
        string username;
        int code = 0;
        int age = 0;
        public void tokens()
        {
            opd opd = new opd();
            opd.tokenLoad();
            label8.Text = opd.token;
            label9.Text = opd.opdMax;
            label10.Text = opd.amount;
        }
        public void tokens2()
        {
            opd opd = new opd();
            opd.tokenLoad();
            label8.Text = opd.token;
            label10.Text = opd.amount;
        }
        public void loadData()
        {
            opd opd = new opd();
            opd.loadPatients();
            dataGridView1.DataSource = opd.table;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.ClearSelection();
        }
        public void loadData(string like)
        {
            opd opd = new opd();
            opd.loadPatients(like);
            dataGridView1.DataSource = opd.table;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.ClearSelection();
        }
        public void ageLoad()
        {
            opd opd = new opd();
            opd.patientAge(textBox1.Text);
            age = opd.age;
        }
        public void issueToken()
        {
            tokens();
            opd opd = new opd();
            opd.issueToken(username, (Convert.ToInt32(label8.Text) + 1).ToString(), textBox1.Text, comboBox1.Text, comboBox3.Text, code.ToString());
            try
            {
                //printPreviewDialog1.ShowDialog();
                printDocument1.Print();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and Issue the token!!");
            }
            loadData();
            clearData();
            tokens();   
        }
        public void clearData()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            code = 0;
            age = 0;
        }
        public Registration(string user)
        {
            InitializeComponent();
            username = user;
            tokens();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMM, yyyy";
            dateTimePicker1.Value = DateTime.Today;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            ((Form)printPreviewDialog1).StartPosition = FormStartPosition.CenterScreen;
            ((Form)printPreviewDialog1).Width = 880;
            ((Form)printPreviewDialog1).Height = 680;
        }

        private void Registration_Load(object sender, EventArgs e)
        {
            loadData();
            comboBox3.SelectedIndex = 0;
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                textBox1.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
                comboBox2.SelectedItem = dataGridView1.Rows[i].Cells[2].Value.ToString();
                comboBox1.SelectedItem = dataGridView1.Rows[i].Cells[4].Value.ToString();
                ageLoad();
                textBox4.Text = age.ToString();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadData();
            clearData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && comboBox1.Text != "" && comboBox2.Text != "")
            {
                Random rand = new Random();
                code = rand.Next(1000, 9999);
                issueToken();
            }
            else
            {
                MessageBox.Show("Invalid Details. Please enter correct details!!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form patient = new newPatient(username);
            patient.ShowDialog();
            loadData();
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("token", 275, 300);
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Pixel;
            Font messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            g.DrawString("DISPENSARY, IIT ROORKEE", messageFont, Brushes.Black, 60, 0);
            g.DrawString("SAHARANPUR CAMPUS", messageFont, Brushes.Black, 80, 30);
            g.DrawString("----------------------------------------------------------------", messageFont, Brushes.Black, 0, 50);
            g.DrawString("Date : " + DateTime.Now.ToString("MMM dd, yyyy hh:mm tt"), messageFont, Brushes.Black, 105, 70);
            g.DrawString("OPD No. : " + (Convert.ToInt32(label9.Text) + 1), messageFont, Brushes.Black, 0, 105);
            g.DrawString("CODE : " + code, messageFont, Brushes.Black, 250, 105);
            g.DrawString("Name : " + textBox2.Text, messageFont, Brushes.Black, 0, 140);
            g.DrawString("Age : "+ age +" Years", messageFont, Brushes.Black, 0, 175);
            if (comboBox1.SelectedIndex == 0)
            { g.DrawString("Fee Collected : Rs. 1 Only.", messageFont, Brushes.Black, 0, 210); }
            else { g.DrawString("Fee Collected : Rs. 0 Only.", messageFont, Brushes.Black, 0, 210); }
            g.DrawString("Patient ID : " + textBox1.Text, messageFont, Brushes.Black, 180, 175);
            g.DrawString(comboBox3.Text, messageFont, Brushes.Black, 0, 245);
            g.DrawString("Token No. :  " + (Convert.ToInt32(label8.Text) + 1), messageFont, Brushes.Black, 240, 245);
            g.DrawString("----------------------------------------------------------------", messageFont, Brushes.Black, 0, 265);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 55, 285);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form update = new updatePatientDetails(username);
            update.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form cancel = new cancelToken(username);
            cancel.ShowDialog();
            tokens2();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form change = new changePassword(username);
            change.ShowDialog();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            loadData(textBox1.Text);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                panel5.Visible = true;
            }
            else
            {
                panel5.Visible = false;
            }
        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 14, System.Drawing.GraphicsUnit.Point);
            g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
            g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
            messageFont = new Font("Times New Roman", 20, System.Drawing.GraphicsUnit.Point);
            g.DrawString("DAILY REPORT", messageFont, Brushes.Black, 75, 25);
            messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Dated: "+dateTimePicker1.Value.Date.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 30);
            Pen blackPen = new Pen(Color.Black, 1);
            g.DrawLine(blackPen, 5, 38, 205, 38);
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, 38 + 2);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, 38 + 12);
            messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Sno.", messageFont, Brushes.Black, 10, 46);
            g.DrawString("OPD", messageFont, Brushes.Black, 20, 46);
            g.DrawString("Booklet No.", messageFont, Brushes.Black, 35, 46);
            g.DrawString("Patient Name", messageFont, Brushes.Black, 60, 46);
            g.DrawString("Age", messageFont, Brushes.Black, 115, 46);
            g.DrawString("Gender", messageFont, Brushes.Black, 125, 46);
            g.DrawString("Family Head", messageFont, Brushes.Black, 140, 46);
            g.DrawString("Amt.", messageFont, Brushes.Black, 165, 46);
            g.DrawString("Doctor", messageFont, Brushes.Black, 178, 46);
            int total = 55;
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.dailyReportRegistration(dateTimePicker1.Value.Date.ToString("yyyy-MM-dd"), db);
            int i = 1;
            int totalAmount = 0;
            while (opd.dr.Read())
            {
                g.DrawString(i+")", messageFont, Brushes.Black, 10, total);
                g.DrawString(opd.dr[0].ToString(), messageFont, Brushes.Black, 20, total);
                g.DrawString(opd.dr[1].ToString(), messageFont, Brushes.Black, 35, total);
                g.DrawString(opd.dr[2].ToString(), messageFont, Brushes.Black, 60, total);
                int age2 = DateTime.Today.Year - Convert.ToInt32(db.dr[3].ToString().Substring(0, 4));
                if (Convert.ToInt32(db.dr[3].ToString().Substring(5, 2)) > DateTime.Today.Month)
                {
                    age2--;
                }
                g.DrawString(age2.ToString(), messageFont, Brushes.Black, 115, total);
                g.DrawString(opd.dr[4].ToString(), messageFont, Brushes.Black, 125, total);
                g.DrawString(opd.dr[5].ToString(), messageFont, Brushes.Black, 140, total);
                int amt = 0;
                if (opd.dr[6].ToString() == "general")
                {
                    amt = 1;
                    totalAmount += 1;
                }
                g.DrawString(amt.ToString(), messageFont, Brushes.Black, 165, total);
                g.DrawString(opd.dr[7].ToString(), messageFont, Brushes.Black, 178, total);
                total += 5;
                i++;
            } messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Total Amount :"+totalAmount.ToString(), messageFont, Brushes.Black, 139, total+2);
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total+5);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 10);
        }

        private void printDocument2_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            opd opd = new opd();
            opd.dailyOPDCount(dateTimePicker1.Value.Date.ToString("yyyy-MM-dd"));
            int patients = Convert.ToInt32(opd.count);
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, (55 + (patients * 5) + 13) * 4);
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form reports = new registrationReports(username);
            reports.ShowDialog();
        } 
    }
}
