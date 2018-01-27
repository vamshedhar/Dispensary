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
    public partial class storeExpiredMedicines : Form
    {
        string username;
        public void loadMedicines()
        {
            medicine med = new medicine();
            med.loadMedicineStore();
            dataGridView1.DataSource = med.table;
            med.loadID();
            textBox1.Text = (Convert.ToInt32(med.count) - 1).ToString();
            
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.ClearSelection();
        }
        public void loadMedicines(string like)
        {
            medicine med = new medicine();
            med.loadMedicineStore(like);
            dataGridView1.DataSource = med.table;
            
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.ClearSelection();
        }
        public void totalTransfer(string id)
        {
            medicine med = new medicine();
            med.countQty("opening", id, "store");
            textBox8.Text = med.count.ToString();
            med.countQty("received", id, "store");
            textBox9.Text = med.count.ToString();
            med.countQty("sent", id, "store");
            textBox10.Text = med.count.ToString();

        }
        public void clearData()
        {
            textBox4.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox13.Text = "";
        }
        public storeExpiredMedicines(string user)
        {
            InitializeComponent();
            username = user;
            loadMedicines();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            loadMedicines();
            clearData();
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            loadMedicines(textBox5.Text);
            clearData();
        }

        private void storeExpiredMedicines_Load(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMM, yyyy";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;

            if (i >= 0)
            {
                textBox6.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
                textBox7.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
                textBox11.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
                string id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                totalTransfer(id);
            }
            else
            {
                clearData();
                dataGridView1.ClearSelection();
            }
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
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
                if (textBox4.Text != "")
                {
                    if (textBox13.Text != "")
                    {
                        int storeStock = Convert.ToInt32(textBox11.Text);
                        int quantity = Convert.ToInt32(textBox13.Text);
                        if (storeStock - quantity >= 0)
                        {
                            string receiver = "miscellaneous";
                            store st = new store();
                            st.expiredStock(storeStock, quantity, textBox6.Text, textBox4.Text, username, receiver, textBox7.Text);
                            loadMedicines();
                            textBox5.Text = "";
                            clearData();
                        }
                        else
                        {
                            MessageBox.Show("WARNING!! You cannot transfer quantity more than that is available in Stock!!!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter the amount of Stock to be transfered!!");
                        textBox13.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter request details of transfer.");
                    textBox4.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please Select a Medicine!!");
            }
        }
    }
}
