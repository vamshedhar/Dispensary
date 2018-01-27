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
    public partial class changePassword : Form
    {
        public string username;
        public changePassword(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            users user = new users(usertab1.user_name,textBox1.Text);
            if (user.error == "sucess")
            {
                if (textBox2.Text != "")
                {
                    if (textBox2.Text == textBox3.Text)
                    {
                        user.changePassword(username, textBox3.Text);
                        if (user.error == "Sucess")
                        {
                            MessageBox.Show("Password Changed Sucessfully.");
                        }
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                    }
                    else
                    {
                        errorProvider1.SetError(textBox3, "New Password and confirm Password donot match");
                    }
                }
                else
                {
                    errorProvider1.SetError(textBox2, "Password cannot empty.");
                }
            }
            else
            {
                errorProvider1.SetError(textBox1, "Old password entered was wrong.");
            }
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }
    }
}
