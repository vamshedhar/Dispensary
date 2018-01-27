using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispensary;
using System.Drawing.Printing;

namespace WindowsFormsApplication1
{
    public partial class storeExpiredMedicinesReports : Form
    {
        string username;
        int j = 0;
        public storeExpiredMedicinesReports(string user)
        {
            username = user;
            InitializeComponent();
        }

        public void expiredMedicinesLoad()
        {
            store med = new store();
            med.loadExpiredMedicine("store");
            dataGridView1.DataSource = med.table;
            dataGridView1.ClearSelection();
        }
        public void yearsLoad()
        {
            for (int i = 2013; i <= Convert.ToInt32(DateTime.Today.Year.ToString()); i++)
            {
                comboBox1.Items.Add(i.ToString());
            }
        }

        private void storeExpiredMedicinesReports_Load(object sender, EventArgs e)
        {
            yearsLoad();
            expiredMedicinesLoad();
            monthCalendar1.SelectionRange.Start = DateTime.Today;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            ((Form)printPreviewDialog1).WindowState = FormWindowState.Maximized;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            comboBox1.SelectedItem = DateTime.Today.Year.ToString();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                comboBox2.Items.Add(printer);
            }
            PrinterSettings settings = new PrinterSettings();
            comboBox2.SelectedItem = settings.PrinterName;
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printDocument1_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1155);//(60 + dataGridView1.Rows.Count * 10) * 4
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 14, System.Drawing.GraphicsUnit.Point);
            int total;
            if (j == 1)
            {
                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("STORE - MONTHLY EXPIRED REPORT (" + monthCalendar1.SelectionStart.Date.ToString("MMMM, yyyy").ToUpper() + ")", messageFont, Brushes.Black, 40, 21);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 28, 205, 28);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 150, 32);
                total = 30;
            }
            else
            {
                total = 5;
            }
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 7);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 17);
            messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            g.DrawString("SNo.", messageFont, Brushes.Black, 10, total + 13);
            g.DrawString("Salt Name", messageFont, Brushes.Black, 22, total + 13);
            g.DrawString("Type", messageFont, Brushes.Black, 87, total + 13);
            g.DrawString("Quantity", messageFont, Brushes.Black, 107, total + 13);
            g.DrawString("Entry On", messageFont, Brushes.Black, 127, total + 13);
            g.DrawString("User", messageFont, Brushes.Black, 167, total + 13);
            total += 25;
            store store = new store();
            dbconnect db = new dbconnect("medicines");
            //MessageBox.Show("SELECT medicine_name, medicine_type, quantity, timestamp, user FROM expired WHERE transaction_id>='" + monthCalendar1.SelectionRange.Start.ToString("yyyyMM") + "01" + "' AND transaction_id<'" + monthCalendar1.SelectionRange.Start.AddMonths(1).ToString("yyyyMM") + "01" + "'");
            store.expiredStockReport("store", monthCalendar1.SelectionRange.Start.ToString("yyyyMM") + "01", monthCalendar1.SelectionRange.Start.AddMonths(1).ToString("yyyyMM") + "01", db);
            int i = 1;
            while (store.dr.Read())
            {
                if (i < j)
                {
                    i++;
                    continue;
                }
                if (total >= 265)
                {
                    e.HasMorePages = true;
                    return;
                }
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                g.DrawString((j).ToString() + ".", messageFont, Brushes.Black, 10, total);
                g.DrawString(store.dr[0].ToString(), messageFont, Brushes.Black, 22, total);
                g.DrawString(store.dr[1].ToString(), messageFont, Brushes.Black, 87, total);
                g.DrawString(store.dr[2].ToString(), messageFont, Brushes.Black, 107, total);
                g.DrawString(store.dr[3].ToString(), messageFont, Brushes.Black, 127, total);
                users userDetails = new users(store.dr[4].ToString());
                g.DrawString(userDetails.fname + " " + userDetails.lname, messageFont, Brushes.Black, 167, total);
                messageFont = new Font("Times New Roman", 5, System.Drawing.GraphicsUnit.Point);
                g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", messageFont, Brushes.Black, 10, total + 4);
                total += 6;
                j++;
                i++;
                e.HasMorePages = false;
            }
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 5);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 10);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            printDocument1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            j = 1;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            printDocument3.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            j = 1;
            printPreviewDialog1.Document = printDocument3;
            printPreviewDialog1.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            printDocument2.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            j = 1;
            printPreviewDialog1.Document = printDocument2;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument2_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1155);//(60 + dataGridView1.Rows.Count * 10) * 4
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument3_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1155);//(60 + dataGridView1.Rows.Count * 10) * 4
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 14, System.Drawing.GraphicsUnit.Point);
            int total;
            if (j == 1)
            {
                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("STORE - ANNUAL EXPENSE REPORT ( APRIL, " + comboBox1.SelectedItem.ToString() + " - MARCH, " + (Convert.ToInt32(comboBox1.SelectedItem.ToString()) + 1) + ")", messageFont, Brushes.Black, 20, 21);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 28, 205, 28);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 150, 32);
                total = 30;
            }
            else
            {
                total = 5;
            }
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 7);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 17);
            messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            g.DrawString("SNo.", messageFont, Brushes.Black, 10, total + 13);
            g.DrawString("Salt Name", messageFont, Brushes.Black, 22, total + 13);
            g.DrawString("Type", messageFont, Brushes.Black, 87, total + 13);
            g.DrawString("Quantity", messageFont, Brushes.Black, 107, total + 13);
            g.DrawString("Entry On", messageFont, Brushes.Black, 127, total + 13);
            g.DrawString("User", messageFont, Brushes.Black, 167, total + 13);
            total += 25;
            store store = new store();
            dbconnect db = new dbconnect("medicines");
            //MessageBox.Show("SELECT medicine_name, medicine_type, quantity, timestamp, user FROM expired WHERE transaction_id>='" + monthCalendar1.SelectionRange.Start.ToString("yyyyMM") + "01" + "' AND transaction_id<'" + monthCalendar1.SelectionRange.Start.AddMonths(1).ToString("yyyyMM") + "01" + "'");
            store.expiredStockReport("store", comboBox1.SelectedItem.ToString() + "0401", (Convert.ToInt32(comboBox1.SelectedItem.ToString()) + 1).ToString() + "0401", db);
            int i = 1;
            while (store.dr.Read())
            {
                if (i < j)
                {
                    i++;
                    continue;
                }
                if (total >= 265)
                {
                    e.HasMorePages = true;
                    return;
                }
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                g.DrawString((j).ToString() + ".", messageFont, Brushes.Black, 10, total);
                g.DrawString(store.dr[0].ToString(), messageFont, Brushes.Black, 22, total);
                g.DrawString(store.dr[1].ToString(), messageFont, Brushes.Black, 87, total);
                g.DrawString(store.dr[2].ToString(), messageFont, Brushes.Black, 107, total);
                g.DrawString(store.dr[3].ToString(), messageFont, Brushes.Black, 127, total);
                users userDetails = new users(store.dr[4].ToString());
                g.DrawString(userDetails.fname + " " + userDetails.lname, messageFont, Brushes.Black, 167, total);
                messageFont = new Font("Times New Roman", 5, System.Drawing.GraphicsUnit.Point);
                g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", messageFont, Brushes.Black, 10, total + 4);
                total += 6;
                j++;
                i++;
                e.HasMorePages = false;
            }
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 5);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 10);
        }

        private void printDocument3_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 14, System.Drawing.GraphicsUnit.Point);
            int total;
            if (j == 1)
            {
                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("STORE - COMPLETE EXPENSE REPORT ( 05 April, 2013 - " + DateTime.Today.ToString("dd MMMM, yyyy") + ")", messageFont, Brushes.Black, 15, 21);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 28, 205, 28);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 150, 32);
                total = 30;
            }
            else
            {
                total = 5;
            }
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 7);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 17);
            messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            g.DrawString("SNo.", messageFont, Brushes.Black, 10, total + 13);
            g.DrawString("Salt Name", messageFont, Brushes.Black, 22, total + 13);
            g.DrawString("Type", messageFont, Brushes.Black, 87, total + 13);
            g.DrawString("Quantity", messageFont, Brushes.Black, 107, total + 13);
            g.DrawString("Entry On", messageFont, Brushes.Black, 127, total + 13);
            g.DrawString("User", messageFont, Brushes.Black, 167, total + 13);
            total += 25;
            store store = new store();
            dbconnect db = new dbconnect("medicines");
            //MessageBox.Show("SELECT medicine_name, medicine_type, quantity, timestamp, user FROM expired WHERE transaction_id>='" + monthCalendar1.SelectionRange.Start.ToString("yyyyMM") + "01" + "' AND transaction_id<'" + monthCalendar1.SelectionRange.Start.AddMonths(1).ToString("yyyyMM") + "01" + "'");
            store.expiredStockReport("store", "20130404", DateTime.Now.AddDays(1).ToString("yyyyMMdd"), db);
            int i = 1;
            while (store.dr.Read())
            {
                if (i < j)
                {
                    i++;
                    continue;
                }
                if (total >= 265)
                {
                    e.HasMorePages = true;
                    return;
                }
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                g.DrawString((j).ToString() + ".", messageFont, Brushes.Black, 10, total);
                g.DrawString(store.dr[0].ToString(), messageFont, Brushes.Black, 22, total);
                g.DrawString(store.dr[1].ToString(), messageFont, Brushes.Black, 87, total);
                g.DrawString(store.dr[2].ToString(), messageFont, Brushes.Black, 107, total);
                g.DrawString(store.dr[3].ToString(), messageFont, Brushes.Black, 127, total);
                users userDetails = new users(store.dr[4].ToString());
                g.DrawString(userDetails.fname + " " + userDetails.lname, messageFont, Brushes.Black, 167, total);
                messageFont = new Font("Times New Roman", 5, System.Drawing.GraphicsUnit.Point);
                g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", messageFont, Brushes.Black, 10, total + 4);
                total += 6;
                j++;
                i++;
                e.HasMorePages = false;
            }
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 5);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 10);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Annual Report??", "Annual Expired Stock Report??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    printDocument2.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                    j = 1;
                    printPreviewDialog1.Document = printDocument2;
                    printDocument2.Print();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Montly Report??", "Montly Expired Stock Report??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    printDocument1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                    j = 1;
                    printPreviewDialog1.Document = printDocument1;
                    printDocument1.Print();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Complete Report??", "Complete Expired Stock Report??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    printDocument3.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                    j = 1;
                    printPreviewDialog1.Document = printDocument3;
                    printDocument3.Print();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                }
            }
        }
    }
}
