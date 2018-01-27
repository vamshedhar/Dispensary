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
    public partial class generateDispIndent : Form
    {
        string username;
        int i = 0;
        public generateDispIndent(string user)
        {
            InitializeComponent();
            username = user;
        }
        public void medicinesLoad()
        {
            medicine med = new medicine();
            med.loadMedicineDisp();
            dataGridView2.DataSource = med.table;
            dataGridView2.ClearSelection();
        }
        public void medicinesLoad(string like)
        {
            medicine med = new medicine();
            med.loadMedicineDisp(like);
            dataGridView2.DataSource = med.table;
            dataGridView2.ClearSelection();
        }
        public void indentLoad()
        {
            medicine med = new medicine();
            med.loadDispIndent();
            dataGridView1.DataSource = med.table;
            dataGridView1.ClearSelection();
            textBox7.Text = dataGridView1.Rows.Count.ToString();
        }
        public void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }
        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void generateDispIndent_Load(object sender, EventArgs e)
        {
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            ((Form)printPreviewDialog1).WindowState = FormWindowState.Maximized;
            textBox6.Text = DateTime.Today.ToString("yyyyMMdd");
            medicinesLoad();
            indentLoad();
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                comboBox3.Items.Add(printer);
            }
            PrinterSettings settings = new PrinterSettings();
            comboBox3.SelectedItem = settings.PrinterName;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            medicinesLoad(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            medicinesLoad();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                textBox2.Text = dataGridView2.Rows[i].Cells[1].Value.ToString();
                textBox3.Text = dataGridView2.Rows[i].Cells[3].Value.ToString();
                textBox5.Text = dataGridView2.Rows[i].Cells[0].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox5.Text != "")
            {
                if (textBox4.Text != "" && Convert.ToInt32(textBox4.Text) != 0)
                {
                    medicine med = new medicine();
                    med.generateDispIndent(textBox5.Text, textBox2.Text, textBox4.Text, textBox3.Text, username);
                    indentLoad();
                    clear();
                }
                else
                {
                    MessageBox.Show("Please enter Valid Quantity!!");
                }
            }
            else
            {
                MessageBox.Show("Please Select a Medicine!!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
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
                g.DrawString("DISPENSARY INDENT REQUEST", messageFont, Brushes.Black, 60, 21);
                Pen blackPen = new Pen(Color.Black, 1);
                g.DrawLine(blackPen, 5, 28, 205, 28);
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                g.DrawString("Indent No.: " + textBox6.Text, messageFont, Brushes.Black, 15, 32);
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
            messageFont = new Font("Times New Roman", 11, System.Drawing.GraphicsUnit.Point);
            g.DrawString("SNo.", messageFont, Brushes.Black, 10, total + 13);
            g.DrawString("Salt Name", messageFont, Brushes.Black, 22, total + 13);
            g.DrawString("DispStock", messageFont, Brushes.Black, 152, total + 13);
            g.DrawString("Indent Req.", messageFont, Brushes.Black, 177, total + 13);
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
                g.DrawString(dataGridView1.Rows[i].Cells[2].Value.ToString(), messageFont, Brushes.Black, 156, total);
                g.DrawString(dataGridView1.Rows[i].Cells[3].Value.ToString(), messageFont, Brushes.Black, 180, total);
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
            g.DrawString("MEDICAL OFFICER", messageFont, Brushes.Black, 25, total + 25);
            g.DrawString("INDENTOR", messageFont, Brushes.Black, 160, total + 25);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
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

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar < 48 || e.KeyChar > 57)
            {
                e.Handled = true;
            }
            if (e.KeyChar == 8)
            {
                e.Handled = false;
            }
        }
    }
}
