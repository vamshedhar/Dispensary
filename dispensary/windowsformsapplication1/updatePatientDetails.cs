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
    public partial class updatePatientDetails : Form
    {
        public string username;
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
            textBox1.Text = "";
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
        public void clearData()
        {
            errorProvider1.Clear();
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "dd";
            textBox5.Text = "mm";
            textBox6.Text = "yyyy";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }
        public void loadDOB(string ID)
        {
            opd opd = new opd();
            opd.patientAge(ID);
            textBox4.Text = opd.dob.Substring(8, 2);
            textBox5.Text = opd.dob.Substring(5, 2);
            textBox6.Text = opd.dob.Substring(0, 4);
        }
        public void updatePatient()
        {
            string gender = comboBox2.SelectedItem.ToString();
            string cat = comboBox1.SelectedItem.ToString();
            string date = textBox4.Text;
            string month = textBox5.Text;
            if (textBox4.Text.Length == 1)
            {
                date = "0" + date;
            }
            if (textBox5.Text.Length == 1)
            {
                month = "0" + month;
            }
            string dob = textBox6.Text + "-" + month + "-" + date;
            opd opd = new opd();
            opd.updatePatient(textBox1.Text,textBox2.Text,dob,gender,textBox3.Text,cat);
            MessageBox.Show(opd.message);
            if (opd.message == "Details UPDATED Sucessfully!!")
            {
                loadData();
                clearData();
            }
        }
        public updatePatientDetails(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool valid = false;
            if (textBox1.Text != "")
            {
                if (textBox2.Text != "")
                {
                    if (comboBox2.SelectedIndex != -1)
                    {
                        if (comboBox1.SelectedIndex != -1)
                        {
                            if (comboBox1.SelectedIndex == 0)
                            {
                                if (textBox3.Text != "")
                                {
                                    if (textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox4.Text == "dd" || textBox5.Text == "mm" || textBox6.Text == "yyyy")
                                    {
                                        errorProvider1.SetError(textBox6, "Please enter a valid Date.");
                                    }
                                    else
                                    {
                                        valid = true;
                                    }
                                }
                                else
                                {
                                    errorProvider1.SetError(textBox3, "Family Head ID required for general category.");
                                }
                            }
                            else
                            {
                                if (textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox4.Text == "dd" || textBox5.Text == "mm" || textBox6.Text == "yyyy")
                                {
                                    errorProvider1.SetError(textBox6, "Please enter a valid Date.");
                                }
                                else
                                {
                                    valid = true;
                                }
                            }
                        }
                        else
                        {
                            errorProvider1.SetError(comboBox1, "Please Select a category.");
                        }
                    }
                    else
                    {
                        errorProvider1.SetError(comboBox2, "Please Select Gender.");
                    }
                }
                else
                {
                    errorProvider1.SetError(textBox2, "Patient Name cannot be empty.");
                }
            }
            else
            {
                errorProvider1.SetError(textBox1, "Please Enter a valid Patient ID.");
            }


            if (valid)
            {
                updatePatient();
            }
            
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadData();
            clearData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox3.Enabled = true;
            }
            else
            {
                textBox3.Enabled = false;
                textBox3.Text = "";
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            loadData(textBox1.Text);
            clearData();
        }

        private void updatePatientDetails_Load(object sender, EventArgs e)
        {
            loadData();
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
                loadDOB(textBox1.Text);
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 90) || (e.KeyChar >= 97 && e.KeyChar <= 122) || e.KeyChar == 8)
            {
                e.Handled = false;
                if (e.KeyChar >= 97 && e.KeyChar <= 122)
                {
                    e.KeyChar = Char.ToUpper(e.KeyChar);
                }
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
