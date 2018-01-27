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
    public partial class storeIndentHistory : Form
    {
        string username;
        int i = 0;
        public storeIndentHistory(string user)
        {
            InitializeComponent();
            username = user;
        }
        public void loadIndent(string indentNo)
        {
            medicine med = new medicine();
            med.loadStoreIndentReceived(indentNo);
            dataGridView1.DataSource = med.table;
            dataGridView1.ClearSelection();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void storeIndentHistory_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMM, yyyy";
            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker1.Text = DateTime.Today.ToString("dd MMM, yyyy");
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            ((Form)printPreviewDialog1).WindowState = FormWindowState.Maximized;
            loadIndent(dateTimePicker1.Value.Date.ToString("yyyyMMdd"));
            textBox1.Text = dateTimePicker1.Value.Date.ToString("yyyyMMdd");
            textBox2.Text = dataGridView1.Rows.Count.ToString();
            if (dataGridView1.Rows.Count == 0)
            {
                textBox1.Text = "";
            }
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

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, 1155);
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
                g.DrawString("STORE INDENT REPORT", messageFont, Brushes.Black, 65, 21);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 28, 205, 28);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                medicine med = new medicine();
                med.getStoreIndentNo(textBox1.Text);
                g.DrawString("Indent No.: " + med.count, messageFont, Brushes.Black, 15, 32);
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
            messageFont = new Font("Times New Roman", 11, System.Drawing.GraphicsUnit.Point);
            g.DrawString("SNo.", messageFont, Brushes.Black, 10, total + 13);
            g.DrawString("Salt Name", messageFont, Brushes.Black, 22, total + 13);
            g.DrawString("StoreStock", messageFont, Brushes.Black, 147, total + 13);
            g.DrawString("QtyInd.", messageFont, Brushes.Black, 167, total + 13);
            g.DrawString("Received.", messageFont, Brushes.Black, 186, total + 13);
            total += 25;
            for (; i < dataGridView1.Rows.Count; i++)
            {
                if (total >= 275)
                {
                    e.HasMorePages = true;
                    return;
                }
                messageFont = new Font("Times New Roman", 10, System.Drawing.GraphicsUnit.Point);
                g.DrawString((i + 1).ToString() + ".", messageFont, Brushes.Black, 10, total);
                g.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), messageFont, Brushes.Black, 22, total);
                g.DrawString(dataGridView1.Rows[i].Cells[2].Value.ToString(), messageFont, Brushes.Black, 151, total);
                g.DrawString(dataGridView1.Rows[i].Cells[3].Value.ToString(), messageFont, Brushes.Black, 170, total);
                g.DrawString(dataGridView1.Rows[i].Cells[4].Value.ToString(), messageFont, Brushes.Black, 189, total);
                messageFont = new Font("Times New Roman", 5, System.Drawing.GraphicsUnit.Point);
                if (i + 1 != dataGridView1.Rows.Count)
                {
                    g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -", messageFont, Brushes.Black, 10, total + 4);
                }
                total += 6;
                e.HasMorePages = false;
            }

            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);
            messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("STORE OFFICIAL", messageFont, Brushes.Black, 150, total + 25);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
            g.DrawString("(This is an official Computrer Generated Printout.)", messageFont, Brushes.Black, 137, total - 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (dataGridView1.Rows.Count != 0)
                {
                    printDocument1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                    i = 0;
                    printPreviewDialog1.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No Indent History to view or Print!!");
                }
            }
            else
            {
                MessageBox.Show("Plese Select Date and get Indent Details!!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            textBox1.Text = "";
            textBox2.Text = "";
            loadIndent(dateTimePicker1.Value.Date.ToString("yyyyMMdd"));
            textBox1.Text = dateTimePicker1.Value.Date.ToString("yyyyMMdd");
            textBox2.Text = dataGridView1.Rows.Count.ToString();
            if (dataGridView1.Rows.Count == 0)
            {
                textBox1.Text = "";
                //MessageBox.Show("No Indent History for the following Date!!");
            }
            medicine med = new medicine();
            med.getStoreIndentNo(textBox1.Text);
            textBox3.Text = med.count;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (dataGridView1.Rows.Count != 0)
                {
                    if (MessageBox.Show("Are you sure you want to Print today's Indent History??", "Pinrt Indent History??", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            printDocument1.PrinterSettings.PrinterName = comboBox3.SelectedItem.ToString();
                            i = 0;
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
                    MessageBox.Show("No Indent History to view or Print!!");
                }
            }
            else
            {
                MessageBox.Show("Plese Select Date and get Indent Details!!");
            }
        }
    }
}
