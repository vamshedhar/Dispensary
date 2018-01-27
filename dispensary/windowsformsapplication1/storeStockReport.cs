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
    public partial class storeStockReport : Form
    {
        string username;
        int i = 0;
        int j = 1;
        public storeStockReport(string user)
        {
            username = user;
            InitializeComponent();
        }
        public void medicinesLoad()
        {
            medicine med = new medicine();
            med.loadMedicine();
            dataGridView1.DataSource = med.table;
            dataGridView1.ClearSelection();
            textBox2.Text = dataGridView1.Rows.Count.ToString();
        }
        public void medicinesLoad(string like)
        {
            medicine med = new medicine();
            med.loadMedicine(like);
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
        private void storeStockReport_Load(object sender, EventArgs e)
        {
            yearsLoad();
            medicinesLoad();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMM, yyyy";
            dateTimePicker1.MaxDate = DateTime.Today;
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

        private void button1_Click(object sender, EventArgs e)
        {
            medicinesLoad();
            textBox1.Text = "";
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            medicinesLoad(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
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
            Font messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            int total;
            if (i == 0)
            {

                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("STORE STOCK REPORT", messageFont, Brushes.Black, 72, 21);
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
            g.DrawString("Store Stock", messageFont, Brushes.Black, 137, total + 13);
            g.DrawString("Disp Stock", messageFont, Brushes.Black, 167, total + 13);
            total += 25;
            for (; i < dataGridView1.Rows.Count; i++)
            {
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                if (total >= 265)
                {
                    e.HasMorePages = true;
                    return;
                }
                g.DrawString((i + 1).ToString() + ".", messageFont, Brushes.Black, 10, total);
                g.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), messageFont, Brushes.Black, 22, total);
                g.DrawString(dataGridView1.Rows[i].Cells[3].Value.ToString(), messageFont, Brushes.Black, 140, total);
                g.DrawString(dataGridView1.Rows[i].Cells[4].Value.ToString(), messageFont, Brushes.Black, 170, total);
                messageFont = new Font("Times New Roman", 5, System.Drawing.GraphicsUnit.Point);
                g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", messageFont, Brushes.Black, 10, total + 4);
                total += 6;
                e.HasMorePages = false;
            }

            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);

            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            medicinesLoad();
            i = 0;
            printDocument1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Current Stock Report??", "Pinrt Current Stock Report??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    medicinesLoad();
                    i = 0;
                    printDocument1.PrinterSettings.PrinterName = comboBox2.SelectedItem.ToString();
                    printDocument1.Print();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                }
            }
        }

        private void printDocument2_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1155);//(60 + dataGridView1.Rows.Count * 10) * 4
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            int total;
            if (i == 0)
            {

                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("DAILY EXPENSE REPORT", messageFont, Brushes.Black, 65, 21);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 28, 205, 28);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Dated: " + dateTimePicker1.Value.Date.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 150, 32);
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
            g.DrawString("Opening", messageFont, Brushes.Black, 127, total + 13);
            g.DrawString("Received", messageFont, Brushes.Black, 147, total + 13);
            g.DrawString("Issued", messageFont, Brushes.Black, 167, total + 13);
            g.DrawString("Current", messageFont, Brushes.Black, 187, total + 13);
            total += 25;
            for (; i < dataGridView1.Rows.Count; i++)
            {
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                if (total >= 265)
                {
                    e.HasMorePages = true;
                    return;
                }
                medicine med = new medicine();
                med.storeDailyExpenses(dateTimePicker1.Value.Date.ToString("yyyyMMdd"), dataGridView1.Rows[i].Cells[0].Value.ToString());
                if (med.issued != "" || med.received != "")
                {
                    g.DrawString((j).ToString() + ".", messageFont, Brushes.Black, 10, total);
                    g.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), messageFont, Brushes.Black, 22, total);
                    g.DrawString(med.opening, messageFont, Brushes.Black, 130, total);
                    g.DrawString(med.received, messageFont, Brushes.Black, 150, total);
                    g.DrawString(med.issued, messageFont, Brushes.Black, 170, total);
                    g.DrawString(med.closing, messageFont, Brushes.Black, 190, total);
                    messageFont = new Font("Times New Roman", 5, System.Drawing.GraphicsUnit.Point);
                    g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", messageFont, Brushes.Black, 10, total + 4);
                    total += 6;
                    j++;
                }
                e.HasMorePages = false;
            }

            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);

            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            medicinesLoad();
            printDocument2.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            i = 0;
            j = 1;
            printPreviewDialog1.Document = printDocument2;
            printPreviewDialog1.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            medicinesLoad();
            i = 0;
            printDocument3.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            printPreviewDialog1.Document = printDocument3;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument3_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1155);//(60 + dataGridView1.Rows.Count * 10) * 4
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument3_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            int total;
            if (i == 0)
            {

                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("STORE - MONTHLY EXPENSE REPORT (" + monthCalendar1.SelectionStart.Date.ToString("MMMM, yyyy").ToUpper() + ")", messageFont, Brushes.Black, 40, 21);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 28, 205, 28);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Dated: " + dateTimePicker1.Value.Date.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 150, 32);
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
            g.DrawString("Opening", messageFont, Brushes.Black, 127, total + 13);
            g.DrawString("Received", messageFont, Brushes.Black, 147, total + 13);
            g.DrawString("Issued", messageFont, Brushes.Black, 167, total + 13);
            g.DrawString("Current", messageFont, Brushes.Black, 187, total + 13);
            total += 25;
            for (; i < dataGridView1.Rows.Count; i++)
            {
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                if (total >= 265)
                {
                    e.HasMorePages = true;
                    return;
                }
                medicine med = new medicine();
                med.storeMonthlyExpenses(monthCalendar1.SelectionRange.Start.ToString("yyyyMM") + "01", monthCalendar1.SelectionRange.Start.AddMonths(1).ToString("yyyyMM") + "01", dataGridView1.Rows[i].Cells[0].Value.ToString());
                string issued = med.issued;
                string received = med.received;
                if (issued == "")
                {
                    issued = "0";
                }
                if (received == "")
                {
                    received = "0";
                }
                g.DrawString((i + 1).ToString() + ".", messageFont, Brushes.Black, 10, total);
                g.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), messageFont, Brushes.Black, 22, total);
                g.DrawString(med.opening, messageFont, Brushes.Black, 130, total);
                g.DrawString(received, messageFont, Brushes.Black, 150, total);
                g.DrawString(issued, messageFont, Brushes.Black, 170, total);
                g.DrawString(med.closing, messageFont, Brushes.Black, 190, total);
                messageFont = new Font("Times New Roman", 5, System.Drawing.GraphicsUnit.Point);
                g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", messageFont, Brushes.Black, 10, total + 4);
                total += 6;
                e.HasMorePages = false;
            }

            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);

            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
        }

        private void printDocument4_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1155);//(60 + dataGridView1.Rows.Count * 10) * 4
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument4_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            int total;
            if (i == 0)
            {

                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("STORE - ANNUAL EXPENSE REPORT ( APRIL, " + comboBox1.SelectedItem.ToString() + " - MARCH, " + (Convert.ToInt32(comboBox1.SelectedItem.ToString()) + 1) + ")", messageFont, Brushes.Black, 30, 21);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 28, 205, 28);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Dated: " + dateTimePicker1.Value.Date.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 150, 32);
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
            g.DrawString("Opening", messageFont, Brushes.Black, 127, total + 13);
            g.DrawString("Received", messageFont, Brushes.Black, 147, total + 13);
            g.DrawString("Issued", messageFont, Brushes.Black, 167, total + 13);
            g.DrawString("Current", messageFont, Brushes.Black, 187, total + 13);
            total += 25;
            for (; i < dataGridView1.Rows.Count; i++)
            {
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                if (total >= 265)
                {
                    e.HasMorePages = true;
                    return;
                }
                medicine med = new medicine();
                med.storeMonthlyExpenses(comboBox1.SelectedItem.ToString() + "0401", (Convert.ToInt32(comboBox1.SelectedItem.ToString()) + 1).ToString() + "0401", dataGridView1.Rows[i].Cells[0].Value.ToString());
                string issued = med.issued;
                string received = med.received;
                if (issued == "")
                {
                    issued = "0";
                }
                if (received == "")
                {
                    received = "0";
                }
                g.DrawString((i + 1).ToString() + ".", messageFont, Brushes.Black, 10, total);
                g.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), messageFont, Brushes.Black, 22, total);
                g.DrawString(med.opening, messageFont, Brushes.Black, 130, total);
                g.DrawString(received, messageFont, Brushes.Black, 150, total);
                g.DrawString(issued, messageFont, Brushes.Black, 170, total);
                g.DrawString(med.closing, messageFont, Brushes.Black, 190, total);
                messageFont = new Font("Times New Roman", 5, System.Drawing.GraphicsUnit.Point);
                g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", messageFont, Brushes.Black, 10, total + 4);
                total += 6;
                e.HasMorePages = false;
            }

            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);

            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            medicinesLoad();
            i = 0;
            printDocument4.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            printPreviewDialog1.Document = printDocument4;
            printPreviewDialog1.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Montly Expense Report??", "Montly Expense Report??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    medicinesLoad();
                    i = 0;
                    printDocument3.PrinterSettings.PrinterName = comboBox2.SelectedItem.ToString();
                    printDocument3.Print();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Annual Expense Report??", "Annual Expense Report??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    medicinesLoad();
                    i = 0;
                    printDocument4.PrinterSettings.PrinterName = comboBox2.SelectedItem.ToString();
                    printDocument4.Print();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Daily Expense Report??", "Daily Expense Report??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    medicinesLoad();
                    i = 0;
                    j = 1;
                    printDocument2.PrinterSettings.PrinterName = comboBox2.SelectedItem.ToString();
                    printDocument2.Print();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                }
            }
        }

        private void printDocument5_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1155);//(60 + dataGridView1.Rows.Count * 10) * 4
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument5_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            int total;
            if (i == 0)
            {

                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("STORE - COMPLETE EXPENSE REPORT ( 05 April, 2013 - " + DateTime.Today.ToString("dd MMMM, yyyy") + ")", messageFont, Brushes.Black, 15, 21);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 28, 205, 28);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Dated: " + dateTimePicker1.Value.Date.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 150, 32);
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
            g.DrawString("Opening", messageFont, Brushes.Black, 127, total + 13);
            g.DrawString("Received", messageFont, Brushes.Black, 147, total + 13);
            g.DrawString("Issued", messageFont, Brushes.Black, 167, total + 13);
            g.DrawString("Current", messageFont, Brushes.Black, 187, total + 13);
            total += 25;
            for (; i < dataGridView1.Rows.Count; i++)
            {
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                if (total >= 265)
                {
                    e.HasMorePages = true;
                    return;
                }
                medicine med = new medicine();
                med.storeMonthlyExpenses("20130404", DateTime.Now.AddDays(1).ToString("yyyyMMdd"), dataGridView1.Rows[i].Cells[0].Value.ToString());
                string issued = med.issued;
                string received = med.received;
                if (issued == "")
                {
                    issued = "0";
                }
                if (received == "")
                {
                    received = "0";
                }
                g.DrawString((i + 1).ToString() + ".", messageFont, Brushes.Black, 10, total);
                g.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), messageFont, Brushes.Black, 22, total);
                g.DrawString(med.opening, messageFont, Brushes.Black, 130, total);
                g.DrawString(received, messageFont, Brushes.Black, 150, total);
                g.DrawString(issued, messageFont, Brushes.Black, 170, total);
                g.DrawString(med.closing, messageFont, Brushes.Black, 190, total);
                messageFont = new Font("Times New Roman", 5, System.Drawing.GraphicsUnit.Point);
                g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", messageFont, Brushes.Black, 10, total + 4);
                total += 6;
                e.HasMorePages = false;
            }

            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);

            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            medicinesLoad();
            i = 0;
            printDocument5.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            printPreviewDialog1.Document = printDocument5;
            printPreviewDialog1.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Complete Expense Report??", "Complete Expense Report??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    medicinesLoad();
                    i = 0;
                    printDocument5.PrinterSettings.PrinterName = comboBox2.SelectedItem.ToString();
                    printDocument5.Print();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                }
            }
        }
    }
}
