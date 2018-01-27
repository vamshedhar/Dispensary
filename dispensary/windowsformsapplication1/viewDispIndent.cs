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
    public partial class viewDispIndent : Form
    {
        string username;
        public void totalTransfer(string id)
        {
            medicine med = new medicine();
            med.countQty("opening", id, "store");
            textBox8.Text = med.count.ToString();
            med.countQty("received", id, "store");
            textBox9.Text = med.count.ToString();
            med.countQty("sent", id, "store");
            textBox10.Text = med.count.ToString();
            med.storeQty(id);
            textBox11.Text = med.count.ToString();
        }
        public void loadIndent()
        {
            store store = new store();
            store.loadDispIndentStore();
            dataGridView1.DataSource = store.table;
            dataGridView1.ClearSelection();
        }
        public void clearData()
        {
            textBox1.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
        }
        public viewDispIndent(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void viewDispIndent_Load(object sender, EventArgs e)
        {
            loadIndent();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            textBox4.Text = DateTime.Today.ToString("yyyyMMdd");
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMM, yyyy";
            label21.Text = DateTime.Today.ToString("yyyyMMdd");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                textBox6.Text = dataGridView1.Rows[i].Cells[0].Value.ToString();
                textBox7.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
                textBox1.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
                textBox12.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
                label23.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
                string id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                totalTransfer(id);
            }
            else
            {
                clearData();
                dataGridView1.ClearSelection();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                if (textBox13.Text != "")
                {
                    if (Convert.ToInt32(textBox13.Text) != 0)
                    {
                        int storeStock = Convert.ToInt32(textBox11.Text);
                        int dispStock = Convert.ToInt32(textBox12.Text);
                        int quantity = Convert.ToInt32(textBox13.Text);
                        if (storeStock - quantity >= 0)
                        {
                            string receiver = "dispensary";
                            store st = new store();
                            st.storeTransferIndent(storeStock, dispStock, quantity, textBox6.Text, "Dispensary Indent No." + textBox4.Text, username, receiver, label21.Text, label23.Text);
                            loadIndent();
                            //textBox5.Text = "";
                            clearData();
                        }
                        else
                        {
                            MessageBox.Show("WARNING!! You cannot issue quantity more than that is available in Stock!!!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Quantity Cannot be ZERO!! Please use Nill option if you dont want to issue medicines!!");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter the amount of Stock to be transfered!!");
                }
            }
            else
            {
                MessageBox.Show("Please Select a Medicine!!");
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "")
            {
                if (MessageBox.Show("Are you sure want to issue Nil Qyantity??", "Issue Nil Quantity??", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    store st = new store();
                    st.storeTransferIndent(label23.Text);
                    loadIndent();
                    clearData();
                }
            }
            else
            {
                MessageBox.Show("Please Select a Medicine!!");
            }
        }
    }
}
