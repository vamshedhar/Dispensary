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
    public partial class viewStoreIndent : Form
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
        public void loadIndent(string indentNo)
        {
            store store = new store();
            store.loadStoreIndentStore(indentNo);
            dataGridView1.DataSource = store.table;
            dataGridView1.ClearSelection();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            medicine med = new medicine();
            med.getStoreIndentNo(label21.Text);
            label24.Text = med.count;
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
        public viewStoreIndent(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void viewStoreIndent_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMM, yyyy";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd MMM, yyyy";
            label21.Text = dateTimePicker2.Value.Date.ToString("yyyyMMdd");
            loadIndent(label21.Text);
            dateTimePicker2.MaxDate = DateTime.Today;
            dateTimePicker2.Text = DateTime.Today.ToString("dd MMM, yyyy");
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
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
                    if (textBox4.Text != "")
                    {
                        if (Convert.ToInt32(textBox13.Text) != 0)
                        {
                            int storeStock = Convert.ToInt32(textBox11.Text);
                            int dispStock = Convert.ToInt32(textBox12.Text);
                            int quantity = Convert.ToInt32(textBox13.Text);
                            string receiver = "store";
                            store st = new store();
                            st.storeReceiveIndent(storeStock, dispStock, quantity, textBox6.Text, textBox4.Text, username, receiver, label21.Text, label23.Text);
                            loadIndent(label21.Text);
                            textBox4.Text = "";
                            clearData();
                            // MessageBox.Show(st.error);
                        }
                        else
                        {
                            MessageBox.Show("Quantity Cannot be ZERO!! Please use Nill option if you dont want to issue medicines!!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Received Indent No.!!!");
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

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            label21.Text = dateTimePicker2.Value.Date.ToString("yyyyMMdd");
            loadIndent(label21.Text);
            clearData();
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
                    st.storeReceiveIndent(label23.Text);
                    loadIndent(label21.Text);
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
