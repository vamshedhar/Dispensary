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
    public partial class registrationReports : Form
    {
        string username;
        int j = 0;
        int totalAmount = 0;
        public registrationReports(string user)
        {
            username = user;
            InitializeComponent();
        }
        public void yearsLoad()
        {
            for (int i = 2013; i <= Convert.ToInt32(DateTime.Today.Year.ToString()); i++)
            {
                comboBox1.Items.Add(i.ToString());
            }
        }
        private void registrationReports_Load(object sender, EventArgs e)
        {
            yearsLoad();
            opd opd = new opd();
            opd.registrationStats();
            label4.Text = opd.count;
            label6.Text = opd.token;
            label9.Text = opd.opdMax;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMM, yyyy";
            dateTimePicker1.Value = DateTime.Today;
            monthCalendar1.SelectionRange.Start = DateTime.Today;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            ((Form)printPreviewDialog1).StartPosition = FormStartPosition.CenterScreen;
            ((Form)printPreviewDialog1).Width = 880;
            ((Form)printPreviewDialog1).Height = 680;
            comboBox1.SelectedItem = DateTime.Today.Year.ToString();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                comboBox3.Items.Add(printer);
            }
            PrinterSettings settings = new PrinterSettings();
            comboBox3.SelectedItem = settings.PrinterName;
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            printDocument2.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            j = 1;
            totalAmount = 0;
            printPreviewDialog1.Document = printDocument2;
            printPreviewDialog1.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printDocument1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            j = 1;
            totalAmount = 0;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1157);
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 14, System.Drawing.GraphicsUnit.Point);
            int total;
            if (j == 1)
            {
                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
                messageFont = new Font("Times New Roman", 20, System.Drawing.GraphicsUnit.Point);
                g.DrawString("DAILY REPORT", messageFont, Brushes.Black, 75, 25);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Dated: " + dateTimePicker1.Value.Date.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 30);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 38, 205, 38);
                total = 38;
            }
            else
            {
                total = 5;
            }
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 2);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 12);
            messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Sno.", messageFont, Brushes.Black, 10, total + 8);
            g.DrawString("OPD", messageFont, Brushes.Black, 20, total + 8);
            g.DrawString("Booklet No.", messageFont, Brushes.Black, 35, total + 8);
            g.DrawString("Patient Name", messageFont, Brushes.Black, 60, total + 8);
            g.DrawString("Age", messageFont, Brushes.Black, 115, total + 8);
            g.DrawString("Gender", messageFont, Brushes.Black, 125, total + 8);
            g.DrawString("Family Head", messageFont, Brushes.Black, 140, total + 8);
            g.DrawString("Amt.", messageFont, Brushes.Black, 165, total + 8);
            g.DrawString("Doctor", messageFont, Brushes.Black, 178, total + 8);
            total += 17;
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.dailyReportRegistration(dateTimePicker1.Value.Date.ToString("yyyy-MM-dd"), db);
            int i = 1;
            while (opd.dr.Read())
            {
                if (i < j)
                {
                    i++;
                    continue;
                }
                if (total >= 270)
                {
                    e.HasMorePages = true;
                    return;
                }
                g.DrawString(i + ")", messageFont, Brushes.Black, 10, total);
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
                j++;
                e.HasMorePages = false;
            }
            messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Total Amount :" + totalAmount.ToString(), messageFont, Brushes.Black, 139, total + 2);
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 5);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 10);
        }

        private void printDocument2_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1157);
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
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
                g.DrawString("MONTHLY REPORT(" + monthCalendar1.SelectionRange.Start.ToString("MMMM, yyyy").ToUpper() + ")", messageFont, Brushes.Black, 65, 25);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 30);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 38, 205, 38);
                total = 38;
            }
            else
            {
                total = 5;
            }
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 2);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 12);
            messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Sno.", messageFont, Brushes.Black, 10, total + 8);
            g.DrawString("OPD", messageFont, Brushes.Black, 20, total + 8);
            g.DrawString("Booklet No.", messageFont, Brushes.Black, 35, total + 8);
            g.DrawString("Patient Name", messageFont, Brushes.Black, 60, total + 8);
            g.DrawString("Age", messageFont, Brushes.Black, 115, total + 8);
            g.DrawString("Gender", messageFont, Brushes.Black, 125, total + 8);
            g.DrawString("Family Head", messageFont, Brushes.Black, 140, total + 8);
            g.DrawString("Amt.", messageFont, Brushes.Black, 165, total + 8);
            g.DrawString("Doctor", messageFont, Brushes.Black, 178, total + 8);
            total += 17;
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.monthlyReportRegistration(monthCalendar1.SelectionRange.Start.ToString("yyyy-MM-") + "01",monthCalendar1.SelectionRange.Start.AddMonths(1).ToString("yyyy-MM-") + "01", db);
            int i = 1;
            while (opd.dr.Read())
            {
                if (i < j)
                {
                    i++;
                    continue;
                }
                if (total >= 270)
                {
                    e.HasMorePages = true;
                    return;
                }
                g.DrawString(j + ")", messageFont, Brushes.Black, 10, total);
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
                j++;
                e.HasMorePages = false;
            } messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Total Amount :" + totalAmount.ToString(), messageFont, Brushes.Black, 139, total + 2);
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 5);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 10);
        }

        private void printDocument3_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1157);
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printDocument3.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            printPreviewDialog1.Document = printDocument3;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument3_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 14, System.Drawing.GraphicsUnit.Point);
            g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
            g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("ANNUAL REPORT(" + comboBox1.SelectedItem.ToString() + "-" + (Convert.ToInt32(comboBox1.SelectedItem.ToString())+1).ToString() + ")", messageFont, Brushes.Black, 75, 25);
            messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 30);
            Pen blackPen = new Pen(Color.Black, 1);
            g.DrawLine(blackPen, 5, 38, 205, 38);
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, 38 + 2);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, 38 + 12);
            messageFont = new Font("Times New Roman", 13, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Sno.", messageFont, Brushes.Black, 10, 46);
            g.DrawString("Month", messageFont, Brushes.Black, 30, 46);
            g.DrawString("Total Patients", messageFont, Brushes.Black, 90, 46);
            g.DrawString("Total Amount", messageFont, Brushes.Black, 155, 46);
            DateTime month = new DateTime(Convert.ToInt32(comboBox1.SelectedItem.ToString()),4,1);
            int total = 60;
            int amount = 0;
            int patients = 0;
            for (int i = 1; i < 13; i++)
            {
                opd opd = new opd();
                opd.annualReport(month.AddMonths(i - 1).ToString("yyyy-MM-dd"), month.AddMonths(i).ToString("yyyy-MM-dd"));
                g.DrawString(i.ToString()+")", messageFont, Brushes.Black, 10, total);
                g.DrawString(month.AddMonths(i-1).ToString("MMMM, yyyy").ToUpper(), messageFont, Brushes.Black, 30, total);
                g.DrawString(opd.count, messageFont, Brushes.Black, 95, total);
                g.DrawString(opd.token, messageFont, Brushes.Black, 160, total);
                total += 10;
                amount += Convert.ToInt32(opd.token);
                patients += Convert.ToInt32(opd.count);
            }
            messageFont = new Font("Times New Roman", 13, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Total Patients : " + patients.ToString(), messageFont, Brushes.Black, 63, total + 2);
            g.DrawString("Total Amount : "+amount.ToString(), messageFont, Brushes.Black, 130, total + 2);
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 5);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 10);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            printDocument4.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            printPreviewDialog1.Document = printDocument4;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument4_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1157);
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument4_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 14, System.Drawing.GraphicsUnit.Point);
            g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
            g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("MONTHLY DATE WISE REPORT(" + monthCalendar1.SelectionRange.Start.ToString("MMMM, yyyy") + ")", messageFont, Brushes.Black, 55, 23);
            messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 30);
            Pen blackPen = new Pen(Color.Black, 1);
            g.DrawLine(blackPen, 5, 38, 205, 38);
            int total = 38;
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 2);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 12);
            messageFont = new Font("Times New Roman", 11, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Sno.", messageFont, Brushes.Black, 15, total + 8);
            g.DrawString("Date", messageFont, Brushes.Black, 45, total + 8);
            g.DrawString("Total Patients", messageFont, Brushes.Black, 95, total + 8);
            g.DrawString("Amount Collected", messageFont, Brushes.Black, 160, total + 8);
            total += 17;
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.monthlyReportDatewise(monthCalendar1.SelectionRange.Start.ToString("yyyy-MM-") + "01",monthCalendar1.SelectionRange.Start.AddMonths(1).ToString("yyyy-MM-") + "01", db);
            int i = 1;
            int amount = 0;
            while (opd.dr.Read())
            {
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                 g.DrawString(i.ToString(), messageFont, Brushes.Black, 15, total);
                 g.DrawString(opd.dr[0].ToString(), messageFont, Brushes.Black, 45, total);
                 g.DrawString(opd.dr[1].ToString(), messageFont, Brushes.Black, 100, total);
                 g.DrawString(opd.dr[2].ToString(), messageFont, Brushes.Black, 165, total);
                 messageFont = new Font("Times New Roman", 5, System.Drawing.GraphicsUnit.Point);
                 g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", messageFont, Brushes.Black, 10, total + 4);
                 total += 6;
                 amount += Convert.ToInt32(opd.dr[2].ToString());
                 i++;
            }
            messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Total Amount :" + amount.ToString(), messageFont, Brushes.Black, 139, total + 2);
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total + 5);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 10);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Complete Monthly Report??", "Monthly Report Print??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    j = 1;
                    totalAmount = 0;
                    printDocument2.PrinterSettings.PrinterName = comboBox3.SelectedItem.ToString();
                    printDocument2.Print();
                    //printDocument4.PrinterSettings.PrinterName = "\\\\STORE\\HP LaserJet P1007";
                    //printDocument4.Print();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Daily Report??", "Daily Report Print??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    j = 1;
                    totalAmount = 0;
                    printDocument1.PrinterSettings.PrinterName = comboBox3.SelectedItem.ToString();
                    printDocument1.Print();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Print Yearly Report??", "Yearly Report Print??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    printDocument3.PrinterSettings.PrinterName = comboBox3.SelectedItem.ToString();
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
            if (MessageBox.Show("Are you sure you want to Print Datewise Monthly Report??", "Monthly Report Print??", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    j = 1;
                    totalAmount = 0;
                    //printDocument2.PrinterSettings.PrinterName = "\\\\STORE\\HP LaserJet P1007";
                    //printDocument2.Print();
                    printDocument4.PrinterSettings.PrinterName = comboBox3.SelectedItem.ToString();
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
            this.Close();
        }
    }
}
