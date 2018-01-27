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
    public partial class returnStock : Form
    {
        string username;
        public void loadMedicines()
        {
            medicine med = new medicine();
            med.loadMedicine();
            dataGridView1.DataSource = med.table;
            med.loadID();
            textBox1.Text = (Convert.ToInt32(med.count) - 1).ToString();
            dataGridView1.ClearSelection();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        public void loadMedicines(string like)
        {
            medicine med = new medicine();
            med.loadMedicine(like);
            dataGridView1.DataSource = med.table;
            dataGridView1.ClearSelection();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        public void clearData()
        {
            textBox6.Text = "";
            textBox7.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox3.Text = "";
        }
        public returnStock(string user)
        {
            InitializeComponent();
            username = user;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMM, yyyy";
        }

        private void returnStock_Load(object sender, EventArgs e)
        {
            loadMedicines();
            clearData();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            loadMedicines();
            clearData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;

            if (i >= 0)
            {
                textBox6.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
                textBox7.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
                textBox11.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
                textBox12.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
                string id = dataGridView1.Rows[i].Cells[0].Value.ToString();
            }
            else
            {
                clearData();
                dataGridView1.ClearSelection();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                if (textBox3.Text != "")
                {
                    int storeStock = Convert.ToInt32(textBox11.Text);
                    int dispStock = Convert.ToInt32(textBox12.Text);
                    int quantity = Convert.ToInt32(textBox3.Text);
                    if (dispStock - quantity >= 0)
                    {
                        if (MessageBox.Show("Do you want to Return this stock to Store?", "Confirm Return?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            store st = new store();
                            st.returnStock(storeStock, dispStock, quantity, textBox6.Text, textBox4.Text, username);
                            loadMedicines();
                            textBox5.Text = "";
                            clearData();
                            MessageBox.Show("Stock Sucessfully Returned to Store!!!");
                        }
                        else
                        {
                            MessageBox.Show("Stock Return Cancelled!!!");
                            loadMedicines();
                            textBox5.Text = "";
                            clearData();
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("WARNING!! You cannot return quantity more than that is available in Dispensary Stock!!!");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter the amount of Stock to be RETURNED!!");
                }
            }
            else
            {
                MessageBox.Show("Please Select a Medicine!!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            loadMedicines(textBox5.Text);
            clearData();
        }
    }
}
