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
    public partial class newPatient : Form
    {
        public string username;
        public void clearData()
        {
            errorProvider1.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "dd";
            textBox5.Text = "mm";
            textBox6.Text = "yyyy";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }
        public void addPatient()
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
            opd.addPatient(textBox1.Text.Trim(), textBox2.Text.Trim(), dob, gender, textBox3.Text.Trim(), cat);
            MessageBox.Show(opd.message);
            if (opd.message == "Details added Sucessfully!")
            {
                clearData();
            }
        }
        public newPatient(string user)
        {
            InitializeComponent();
            username = user;
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
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clearData();
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            if(textBox4.Text == "dd")
            {
            textBox4.Text = "";
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.Text = "dd";
            }
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (textBox5.Text == "mm")
            {
                textBox5.Text = "";
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.Text = "mm";
            }
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            if (textBox6.Text == "yyyy")
            {
                textBox6.Text = "";
            }
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                textBox6.Text = "yyyy";
            }
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
                errorProvider1.SetError(textBox1, "Patient ID cannot be empty.");
            }
            if (valid)
            {
                addPatient();        
            }
        }
        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
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

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 65 && e.KeyChar <= 90) || (e.KeyChar >= 97 && e.KeyChar <= 122) || e.KeyChar == 8 || e.KeyChar == 32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
