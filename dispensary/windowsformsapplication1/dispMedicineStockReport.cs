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
    public partial class dispMedicineStockReport : Form
    {
        public string username;
        int i = 0;
        int j = 0;
        public dispMedicineStockReport(string user)
        {
            InitializeComponent();
            username = user;
        }
        public void loadMedicines()
        {
            medicine med = new medicine();
            med.listMedicine();
            listBox1.DataSource = med.table;
            listBox1.ValueMember = "medicine_id";
            listBox1.DisplayMember = "medicineDetail";
            listBox1.BindingContext = new BindingContext();
            listBox1.SelectedIndex = -1;
            med.loadID();
            textBox4.Text = (Convert.ToInt32(med.count) - 1).ToString();
        }
        public void loadMedicines(string like)
        {
            medicine med = new medicine();
            med.listMedicine(like);
            listBox1.DataSource = med.table;
            listBox1.ValueMember = "medicine_id";
            listBox1.DisplayMember = "medicineDetail";
            listBox1.BindingContext = new BindingContext();
            listBox1.SelectedIndex = -1;
        }
        public void medicineDetail(string id)
        {
            medicine med = new medicine();
            dbconnect db = new dbconnect("medicines");
            med.medicineDetail(id, db);
            if (med.dr.Read())
            {
                textBox6.Text = med.dr[1].ToString();
                textBox7.Text = med.dr[2].ToString();
                comboBox2.SelectedItem = med.dr[3].ToString();
            }
            db.dbclose();
        }
        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void dispMedicineStockReport_Load(object sender, EventArgs e)
        {
            loadMedicines();
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            ((Form)printPreviewDialog1).StartPosition = FormStartPosition.CenterScreen;
            ((Form)printPreviewDialog1).Width = 880;
            ((Form)printPreviewDialog1).Height = 680;
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            loadMedicines(textBox5.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                medicineDetail(listBox1.SelectedValue.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printDocument1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            j = 1;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1155);//(60 + dataGridView1.Rows.Count * 10) * 4
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            int total;
            if (j == 1)
            {
                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 85, 4);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 47, 9);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("COMPLETE STOCK TRANSFER REPORT", messageFont, Brushes.Black, 65, 14);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 20);
                g.DrawString(textBox7.Text + " -- " + comboBox2.Text, messageFont, Brushes.Black, 12, 20);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 26, 205, 26);
                total = 25;
            }
            else
            {
                total = 0;
            }
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 2);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 12);
            messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Sno.", messageFont, Brushes.Black, 10, total + 8);
            g.DrawString("Timestamp", messageFont, Brushes.Black, 30, total + 8);
            g.DrawString("Issued", messageFont, Brushes.Black, 75, total + 8);
            g.DrawString("Received", messageFont, Brushes.Black, 95, total + 8);
            g.DrawString("Balance", messageFont, Brushes.Black, 115, total + 8);
            g.DrawString("Details", messageFont, Brushes.Black, 135, total + 8);
            total += 17;
            messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            medicine med = new medicine();
            dbconnect db = new dbconnect("medicines");
            med.medicineTransferDetails(textBox6.Text,db);
            int i = 1;
            while (med.dr.Read())
            {
                if (i < j)
                {
                    i++;
                    continue;
                }
                if (total >= 285)
                {
                    messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                    g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, 285);
                    e.HasMorePages = true;
                    return;
                }
                g.DrawString(i + ")", messageFont, Brushes.Black, 10, total);
                g.DrawString(med.dr[11].ToString(), messageFont, Brushes.Black, 30, total);
                
                if(med.dr[1].ToString() == "dispensary" && med.dr[6].ToString() == "sent")
                {
                    g.DrawString(med.dr[7].ToString(), messageFont, Brushes.Black, 75, total);
                    g.DrawString("-", messageFont, Brushes.Black, 95, total);
                }
                if (med.dr[1].ToString() == "store" && med.dr[5].ToString() == "dispensary")
                {
                    g.DrawString("-", messageFont, Brushes.Black, 75, total);
                    g.DrawString(med.dr[7].ToString(), messageFont, Brushes.Black, 95, total);
                }
                g.DrawString(med.dr[9].ToString(), messageFont, Brushes.Black, 115, total);
                g.DrawString(med.dr[10].ToString(), messageFont, Brushes.Black, 135, total);
                total += 5;
                i++;
                j++;
                e.HasMorePages = false;
            }
            messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 5);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 10);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                MessageBox.Show(printer);
            }
        }
    }
}
