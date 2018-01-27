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
    public partial class cancelToken : Form
    {
        public string username;
        public void loadOPD(string opd)
        {
            string query = "SELECT * FROM token WHERE opd=" + opd;
            dbconnect db = new dbconnect();
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {

                textBox2.Text = db.dr[2].ToString();
                textBox3.Text = db.dr[4].ToString();
                textBox4.Text = db.dr[3].ToString().Substring(0, 10);
                textBox5.Text = db.dr[6].ToString();
                textBox6.Text = db.dr[7].ToString();
                textBox7.Text = db.dr[1].ToString();
                textBox8.Text = db.dr[9].ToString();
            }
            else
            {
                MessageBox.Show("Enter Valid OPD.");
            }
            db.dbclose();
        }
        public void loadToken(string token)
        {
            string query = "SELECT * FROM token WHERE token_no=" + token + " AND date='" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            dbconnect db = new dbconnect();
            db.command_reader(query, db.con);
            if (db.dr.Read())
            {
                textBox1.Text = db.dr[0].ToString();
                textBox3.Text = db.dr[4].ToString();
                textBox4.Text = db.dr[3].ToString();
                textBox5.Text = db.dr[6].ToString();
                textBox6.Text = db.dr[7].ToString();
                textBox7.Text = db.dr[1].ToString();
                textBox8.Text = db.dr[9].ToString();
            }
            else
            {
                MessageBox.Show("Enter todays valid Token No.");
            }
            db.dbclose();
        }
        public void clearData()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
        }
        public cancelToken(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            clearData();
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                loadOPD(textBox1.Text);
            }
            else
            {
                MessageBox.Show("Please enter a OPD No.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                loadToken(textBox2.Text);
            }
            else
            {
                MessageBox.Show("Please enter a Token No.");
            }
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            clearData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool valid = false;
            if (!Convert.ToBoolean(textBox8.Text))
            {
                if (textBox4.Text == DateTime.Today.ToString("yyyy-MM-dd"))
                {
                    valid = true;
                }
                else
                {
                    MessageBox.Show("You can cancel todays tokens only.");
                }
            }
            else
            {
                MessageBox.Show("Patient already visited doctor. Token cannot be canceled.");
            }
           
            if (valid)
            {
                opd opd = new opd();
                opd.cancelToken(textBox1.Text);
                MessageBox.Show("Token canceled sucessfully!");
                clearData();
            }
        }
    }
}
