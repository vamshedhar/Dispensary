using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class date_today : UserControl
    {
        public date_today()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void date_today_Load(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }
    }
}
