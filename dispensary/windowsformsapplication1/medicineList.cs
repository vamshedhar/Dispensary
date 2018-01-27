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
    public partial class medicineList : Form
    {
        string username;
        public medicineList(string user)
        {
            InitializeComponent();
            username = user;
        }
        private void fullMedicineList()
        {
            medicine med = new medicine();
            med.fullMedicineList();
            dataGridView1.DataSource = med.table;
        }
        private void medicineList_Load(object sender, EventArgs e)
        {
            fullMedicineList();
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
