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
    /*
     * 
     *  Created By -- Vam$hedhar Reddy C
     *  All Rights Reserved
     *  Product of IMG Labs IIT Roorkee, Saharanpur Campus
     *  
     * */
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox1.Select();
            timer1.Enabled = false;
            label8.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            users user = new users(textBox1.Text,textBox2.Text);

            if (user.error == "error")
            {
                timer1.Enabled = true;
                label8.Visible = true;
                textBox2.Text = "";
                textBox2.Select();
            }
            else
            {
                if (user.post == "registration")
                {
                    Form registration = new Registration(textBox1.Text);
                    registration.ShowDialog();
                }
                else if (user.post == "store")
                {
                    Form store = new Store(textBox1.Text);
                    store.ShowDialog();
                }
                else if (user.post == "doctor")
                {
                    Form doctor = new doctorMain(textBox1.Text);
                    doctor.ShowDialog();
                }
                else if (user.post == "dispensary")
                {
                    Form disp = new dispensaryMain(textBox1.Text);
                    disp.ShowDialog();
                }
                textBox1.Text = "";
                textBox2.Text = "";
                timer1.Enabled = false;
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label8.Visible)
            {
                label8.Visible = false;
            }
            else
            {
                label8.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Select();
        }

       
    }
}
