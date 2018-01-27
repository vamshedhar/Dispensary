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
    public partial class newMedicine : Form
    {
        public string username;
        public newMedicine(string user)
        {
            InitializeComponent();
            username = user;
        }
        public void id()
        {
            medicine med = new medicine();
            med.loadID();
            textBox2.Text = "M" + med.count;
        }
        public void clear()
        {
            id();
            textBox1.Text = "";
            textBox3.Text = "";
            comboBox1.SelectedIndex = -1;
        }
        public void loadMedicines()
        {
            medicine med = new medicine();
            med.listMedicine();
            listBox1.DataSource = med.table;
            listBox1.ValueMember = "medicine_id";
            listBox1.DisplayMember = "medicineDetail";
            listBox1.BindingContext = new BindingContext();
            listBox1.SelectedIndex = -1;
            med.loadID();
            textBox4.Text = (Convert.ToInt32(med.count) - 1).ToString();
         }
        public void loadMedicines(string like)
        {
            medicine med = new medicine();
            med.listMedicine(like);
            listBox1.DataSource = med.table;
            listBox1.ValueMember = "medicine_id";
            listBox1.DisplayMember = "medicineDetail";
            listBox1.BindingContext = new BindingContext();
            listBox1.SelectedIndex = -1;
        }
        public void resetEdit()
        {
            textBox6.Text = "";
            textBox7.Enabled = false;
            textBox7.Text = "";
            comboBox2.Enabled = false;
            comboBox2.SelectedIndex = -1;
            button7.Text = "Edit Medicine";
        }
        public void medicineDetail(string id)
        {
            medicine med = new medicine();
            dbconnect db = new dbconnect("medicines");
            med.medicineDetail(id,db);
            if (med.dr.Read())
            {
                textBox6.Text = med.dr[1].ToString();
                textBox7.Text = med.dr[2].ToString();
                comboBox2.SelectedItem = med.dr[3].ToString();
            }
            db.dbclose();
        }
        public void message(string msg)
        {
            MessageBox.Show(msg);
        }
        private void newMedicine_Load(object sender, EventArgs e)
        {
            id();
            loadMedicines();
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (comboBox1.SelectedIndex != -1)
                {
                    medicine med = new medicine();
                    med.addMedicine(textBox2.Text, textBox1.Text, comboBox1.Text, Convert.ToInt32(textBox3.Text), username);
                    //message(med.error);
                    message("Medicine added Sucessfully!!!");
                    clear();
                    loadMedicines();
                    id();
                }
                else { MessageBox.Show("Please Select a category for Medicine!!"); }
            }
            else { MessageBox.Show("Salt Name cannot be EMPTY!!"); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            loadMedicines();
            resetEdit();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                resetEdit();
                medicineDetail(listBox1.SelectedValue.ToString());
            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.Text == "Edit Medicine")
            {
                if (textBox6.Text != "")
                {
                    textBox7.Enabled = true;
                    comboBox2.Enabled = true;
                    button7.Text = "Done";
                }
                else
                {
                    MessageBox.Show("Please select the medicine which u want to EDIT!!");
                }
                
            }
            else
            {
                medicine med = new medicine();
                med.editMedicine(textBox6.Text,textBox7.Text,comboBox2.SelectedItem.ToString());
                resetEdit();
                loadMedicines();
                message("Medicine Updated Sucessfully!!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            resetEdit();
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

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            loadMedicines(textBox5.Text);
        }
    }
}
