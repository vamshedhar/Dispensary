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
    public partial class patientHistoryReports : Form
    {
        string username;
        int age = 0;
        string opdCount;
        public int medicineCount = 0;
        public string[] medicine_id = new string[0];
        public string[] medicines = new string[0];
        public string[] dosage = new string[0];
        public string[] quantity = new string[0];
        int m = 0;
        int j = 0;
        public patientHistoryReports(string user)
        {
            InitializeComponent();
            username = user;
            usertab1.user_name = username;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            ((Form)printPreviewDialog1).StartPosition = FormStartPosition.CenterScreen;
            ((Form)printPreviewDialog1).Width = 880;
            ((Form)printPreviewDialog1).Height = 680;
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
            opd.totalPatientVisits(textBox1.Text);
            opdCount = opd.visits;
        }
        public void clearData()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            comboBox1.SelectedIndex = -1;
            opdCount = "0";
            age = 0;
            loadData();
        }
        private void patientHistoryReports_Load(object sender, EventArgs e)
        {
            loadData();
            for (int i = 2013; i <= DateTime.Today.Year; i++)
            {
                comboBox2.Items.Add(i.ToString());
            }
            comboBox2.Text = DateTime.Today.Year.ToString();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMM, yyyy";
            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd MMM, yyyy";
            dateTimePicker2.MaxDate = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                comboBox3.Items.Add(printer);
            }
            PrinterSettings settings = new PrinterSettings();
            comboBox3.SelectedItem = settings.PrinterName;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            clearData();
            loadData(textBox1.Text);
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
                textBox5.Text = opdCount;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearData();
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                opd opd = new opd();
                dbconnect db = new dbconnect();
                medicineCount = 0;
                opd.getPatientOPDHistory(textBox1.Text, db);
                while (opd.dr.Read())
                {
                    medicine_id = opd.dr[2].ToString().Split(',');
                    medicineCount += medicine_id.Length;
                }
                db.dbclose();
                j = 1;
                printDocument1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Select a Patient from the list!!!");
            }
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
            Font messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            Pen blackPen = new Pen(Color.Black, 1);
            int total;
            if (j == 1)
            {
                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 7);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 12);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("PATIENT MEDICAL HISTORY", messageFont, Brushes.Black, 65, 18);
                g.DrawLine(blackPen, 5, 26, 205, 26);
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Patient Name: " + textBox2.Text, messageFont, Brushes.Black, 10, 27);
                g.DrawString("Patient ID: " + textBox1.Text, messageFont, Brushes.Black, 10, 33);
                g.DrawString("Age: " + textBox4.Text + " Years", messageFont, Brushes.Black, 120, 27);
                g.DrawString("Gender: " + comboBox2.Text, messageFont, Brushes.Black, 160, 27);
                if (textBox3.Text != "")
                {
                    g.DrawString("Family Head ID: " + textBox3.Text, messageFont, Brushes.Black, 120, 33);
                }
                g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 20);
                g.DrawLine(blackPen, 5, 39, 205, 39);
                total = 40;
            }
            else
            {
                total = 5;
            }
            
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.getPatientOPDHistory(textBox1.Text, db);
            int sno = 1;
            while (opd.dr.Read())
            {
                if (sno < j)
                {
                    sno++;
                    continue;
                }
                if (total >= 275)
                {
                    e.HasMorePages = true;
                    return;
                }
                
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                medicine_id = new string[0];
                medicines = new string[0];
                dosage = new string[0];
                quantity = new string[0];
                if (m == 0)
                {
                    g.DrawString(j + ")", messageFont, Brushes.Black, 10, total + 1);
                    g.DrawString("OPD: " + opd.dr[0].ToString(), messageFont, Brushes.Black, 20, total + 1);
                    g.DrawString("Doctor: " + opd.dr[8].ToString(), messageFont, Brushes.Black, 55, total + 1);
                    g.DrawString("Symptoms: " + opd.dr[6].ToString(), messageFont, Brushes.Black, 105, total + 1);
                    g.DrawString("Visit Date: " + opd.dr[1].ToString().Substring(0, 4) + "-" + opd.dr[1].ToString().Substring(4, 2) + "-" + opd.dr[1].ToString().Substring(6, 2), messageFont, Brushes.Black, 20, total + 6);
                    g.DrawString("Remarks: " + opd.dr[7].ToString(), messageFont, Brushes.Black, 70, total + 6);
                    total += 12;
                }
                medicine_id = opd.dr[2].ToString().Split(',');
                medicines = opd.dr[3].ToString().Split(',');
                dosage = opd.dr[4].ToString().Split(',');
                quantity = opd.dr[5].ToString().Split(',');
                for (int i = 0; i < medicine_id.Length; i++)
                {
                    if (i < m - 1)
                    {
                        continue;
                    }
                    m++;
                    if (total >= 275)
                    {
                        
                        e.HasMorePages = true;
                        return;
                    }
                    g.DrawString((i + 1) + ".", messageFont, Brushes.Black, 20, total);
                    g.DrawString(medicines[i] + " -- ( " + quantity[i] + " ) -- ( " + dosage[i] + " )", messageFont, Brushes.Black, 27, total);
                    total += 4;
                    //m++;
                }
                m = 0;
                if (sno < Convert.ToInt32(opdCount))
                {
                    messageFont = new Font("Times New Roman", 8, FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                    g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ", messageFont, Brushes.Black, 10, total - 1);
                    total += 1;
                    
                }
                j++;
                sno++;
            }
            db.dbclose();
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
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
            Font messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            Pen blackPen = new Pen(Color.Black, 1);
            int total;
            if (j == 1)
            {
                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 5);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 10);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("PATIENT MEDICAL HISTORY", messageFont, Brushes.Black, 65, 15);
                messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
                g.DrawString("(From 01 Jan, " +comboBox2.Text + "   To   31 Dec, " + (Convert.ToInt32(comboBox2.Text)).ToString() + ")", messageFont, Brushes.Black, 77, 20);
                g.DrawLine(blackPen, 5, 26, 205, 26);
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Patient Name: " + textBox2.Text, messageFont, Brushes.Black, 10, 27);
                g.DrawString("Patient ID: " + textBox1.Text, messageFont, Brushes.Black, 10, 33);
                g.DrawString("Age: " + textBox4.Text + " Years", messageFont, Brushes.Black, 120, 27);
                g.DrawString("Gender: " + comboBox2.Text, messageFont, Brushes.Black, 160, 27);
                if (textBox3.Text != "")
                {
                    g.DrawString("Family Head ID: " + textBox3.Text, messageFont, Brushes.Black, 120, 33);
                }
                g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 20);
                g.DrawLine(blackPen, 5, 39, 205, 39);
                total = 40;
            }
            else
            {
                total = 5;
            }

            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.getPatientOPDHistory(textBox1.Text, comboBox2.Text + "0101", (Convert.ToInt32(comboBox2.Text) + 1).ToString() + "0101", db);
            int sno = 1;
            while (opd.dr.Read())
            {
                if (sno < j)
                {
                    sno++;
                    continue;
                }
                if (total >= 275)
                {
                    e.HasMorePages = true;
                    return;
                }

                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                medicine_id = new string[0];
                medicines = new string[0];
                dosage = new string[0];
                quantity = new string[0];
                if (m == 0)
                {
                    g.DrawString(j + ")", messageFont, Brushes.Black, 10, total + 1);
                    g.DrawString("OPD: " + opd.dr[0].ToString(), messageFont, Brushes.Black, 20, total + 1);
                    g.DrawString("Doctor: " + opd.dr[8].ToString(), messageFont, Brushes.Black, 55, total + 1);
                    g.DrawString("Symptoms: " + opd.dr[6].ToString(), messageFont, Brushes.Black, 105, total + 1);
                    g.DrawString("Visit Date: " + opd.dr[1].ToString().Substring(0, 4) + "-" + opd.dr[1].ToString().Substring(4, 2) + "-" + opd.dr[1].ToString().Substring(6, 2), messageFont, Brushes.Black, 20, total + 6);
                    g.DrawString("Remarks: " + opd.dr[7].ToString(), messageFont, Brushes.Black, 70, total + 6);
                    total += 12;
                }
                medicine_id = opd.dr[2].ToString().Split(',');
                medicines = opd.dr[3].ToString().Split(',');
                dosage = opd.dr[4].ToString().Split(',');
                quantity = opd.dr[5].ToString().Split(',');
                for (int i = 0; i < medicine_id.Length; i++)
                {
                    if (i < m - 1)
                    {
                        continue;
                    }
                    m++;
                    if (total >= 275)
                    {

                        e.HasMorePages = true;
                        return;
                    }
                    g.DrawString((i + 1) + ".", messageFont, Brushes.Black, 20, total);
                    g.DrawString(medicines[i] + " -- ( " + quantity[i] + " ) -- ( " + dosage[i] + " )", messageFont, Brushes.Black, 27, total);
                    total += 4;
                    //m++;
                }
                m = 0;
                if (sno < Convert.ToInt32(opdCount))
                {
                    messageFont = new Font("Times New Roman", 8, FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                    g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ", messageFont, Brushes.Black, 10, total - 1);
                    total += 1;

                }
                j++;
                sno++;
            }
            db.dbclose();
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                opd opd = new opd();
                dbconnect db = new dbconnect();
                medicineCount = 0;
                opd.getPatientOPDHistory(textBox1.Text,comboBox2.Text+"0101",(Convert.ToInt32(comboBox2.Text)+1).ToString()+"0101", db);
                while (opd.dr.Read())
                {
                    medicine_id = opd.dr[2].ToString().Split(',');
                    medicineCount += medicine_id.Length;
                }
                db.dbclose();
                j = 1;
                printDocument2.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                printPreviewDialog1.Document = printDocument2;
                printPreviewDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Select a Patient from the list!!!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                opd opd = new opd();
                dbconnect db = new dbconnect();
                medicineCount = 0;
                opd.getPatientOPDHistory(textBox1.Text, dateTimePicker2.Value.ToString("yyyyMMdd"), dateTimePicker1.Value.ToString("yyyyMMdd"), db);
                while (opd.dr.Read())
                {
                    medicine_id = opd.dr[2].ToString().Split(',');
                    medicineCount += medicine_id.Length;
                }
                db.dbclose();
                j = 1;
                printDocument3.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                printPreviewDialog1.Document = printDocument3;
                printPreviewDialog1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Select a Patient from the list!!!");
            }
        }

        private void printDocument3_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1157);
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument3_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            Pen blackPen = new Pen(Color.Black, 1);
            int total;
            if (j == 1)
            {
                g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 5);
                g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 10);
                messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
                g.DrawString("PATIENT MEDICAL HISTORY", messageFont, Brushes.Black, 65, 15);
                messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
                g.DrawString("(From " + dateTimePicker2.Value.ToString("dd MMM, yyyy") + "   To   " + dateTimePicker1.Value.ToString("dd MMM, yyyy") + ")", messageFont, Brushes.Black, 77, 20);
                g.DrawLine(blackPen, 5, 26, 205, 26);
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Patient Name: " + textBox2.Text, messageFont, Brushes.Black, 10, 27);
                g.DrawString("Patient ID: " + textBox1.Text, messageFont, Brushes.Black, 10, 33);
                g.DrawString("Age: " + textBox4.Text + " Years", messageFont, Brushes.Black, 120, 27);
                g.DrawString("Gender: " + comboBox2.Text, messageFont, Brushes.Black, 160, 27);
                if (textBox3.Text != "")
                {
                    g.DrawString("Family Head ID: " + textBox3.Text, messageFont, Brushes.Black, 120, 33);
                }
                g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 20);
                g.DrawLine(blackPen, 5, 39, 205, 39);
                total = 40;
            }
            else
            {
                total = 5;
            }

            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.getPatientOPDHistory(textBox1.Text, dateTimePicker2.Value.ToString("yyyyMMdd"), dateTimePicker1.Value.ToString("yyyyMMdd"), db);
            int sno = 1;
            while (opd.dr.Read())
            {
                if (sno < j)
                {
                    sno++;
                    continue;
                }
                if (total >= 275)
                {
                    e.HasMorePages = true;
                    return;
                }

                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                medicine_id = new string[0];
                medicines = new string[0];
                dosage = new string[0];
                quantity = new string[0];
                if (m == 0)
                {
                    g.DrawString(j + ")", messageFont, Brushes.Black, 10, total + 1);
                    g.DrawString("OPD: " + opd.dr[0].ToString(), messageFont, Brushes.Black, 20, total + 1);
                    g.DrawString("Doctor: " + opd.dr[8].ToString(), messageFont, Brushes.Black, 55, total + 1);
                    g.DrawString("Symptoms: " + opd.dr[6].ToString(), messageFont, Brushes.Black, 105, total + 1);
                    g.DrawString("Visit Date: " + opd.dr[1].ToString().Substring(0, 4) + "-" + opd.dr[1].ToString().Substring(4, 2) + "-" + opd.dr[1].ToString().Substring(6, 2), messageFont, Brushes.Black, 20, total + 6);
                    g.DrawString("Remarks: " + opd.dr[7].ToString(), messageFont, Brushes.Black, 70, total + 6);
                    total += 12;
                }
                medicine_id = opd.dr[2].ToString().Split(',');
                medicines = opd.dr[3].ToString().Split(',');
                dosage = opd.dr[4].ToString().Split(',');
                quantity = opd.dr[5].ToString().Split(',');
                for (int i = 0; i < medicine_id.Length; i++)
                {
                    if (i < m - 1)
                    {
                        continue;
                    }
                    m++;
                    if (total >= 275)
                    {

                        e.HasMorePages = true;
                        return;
                    }
                    g.DrawString((i + 1) + ".", messageFont, Brushes.Black, 20, total);
                    g.DrawString(medicines[i] + " -- ( " + quantity[i] + " ) -- ( " + dosage[i] + " )", messageFont, Brushes.Black, 27, total);
                    total += 4;
                    //m++;
                }
                m = 0;
                if (sno < Convert.ToInt32(opdCount))
                {
                    messageFont = new Font("Times New Roman", 8, FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                    g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ", messageFont, Brushes.Black, 10, total - 1);
                    total += 1;

                }
                j++;
                sno++;
            }
            db.dbclose();
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (MessageBox.Show("Are you sure you want to Print Year wise Patient History??", "Patient History Print??", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    opd opd = new opd();
                    dbconnect db = new dbconnect();
                    medicineCount = 0;
                    opd.getPatientOPDHistory(textBox1.Text, dateTimePicker2.Value.ToString("yyyyMMdd"), dateTimePicker1.Value.ToString("yyyyMMdd"), db);
                    while (opd.dr.Read())
                    {
                        medicine_id = opd.dr[2].ToString().Split(',');
                        medicineCount += medicine_id.Length;
                    }
                    db.dbclose();
                    try
                    {
                        j = 1;
                        printDocument3.PrinterSettings.PrinterName = comboBox3.SelectedItem.ToString();
                        printDocument3.Print();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Select a Patient from the list!!!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (MessageBox.Show("Are you sure you want to Print Complete Patient History??", "Patient History Print??", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    opd opd = new opd();
                    dbconnect db = new dbconnect();
                    medicineCount = 0;
                    opd.getPatientOPDHistory(textBox1.Text, db);
                    while (opd.dr.Read())
                    {
                        medicine_id = opd.dr[2].ToString().Split(',');
                        medicineCount += medicine_id.Length;
                    }
                    db.dbclose();
                    try
                    {
                        j = 1;
                        printDocument1.PrinterSettings.PrinterName = comboBox3.SelectedItem.ToString();
                        printDocument1.Print();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Select a Patient from the list!!!");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (MessageBox.Show("Are you sure you want to Print Year wise Patient History??", "Patient History Print??", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    opd opd = new opd();
                    dbconnect db = new dbconnect();
                    medicineCount = 0;
                    opd.getPatientOPDHistory(textBox1.Text, comboBox2.Text + "0101", (Convert.ToInt32(comboBox2.Text) + 1).ToString() + "0101", db);
                    while (opd.dr.Read())
                    {
                        medicine_id = opd.dr[2].ToString().Split(',');
                        medicineCount += medicine_id.Length;
                    }
                    db.dbclose();
                    try
                    {
                        j = 1;
                        printDocument2.PrinterSettings.PrinterName = comboBox3.SelectedItem.ToString();
                        printDocument2.Print();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Select a Patient from the list!!!");
            }
        }
    }
}
