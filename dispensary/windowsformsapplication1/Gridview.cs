using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispensary;

namespace WindowsFormsApplication1
{
    public partial class Gridview : UserControl
    {
        public void loaddata(string search)
        {
            dbconnect db = new dbconnect();

            db.data_adapter(search, db.con);
            DataSet ds = new DataSet();
            db.da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            db.dbclose();
           foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        public string search
        {
            set
            {
                loaddata(value);
            }
        }
        public Gridview()
        {
            InitializeComponent();
            string query = "SELECT sno,patient_id,name,gender,family_head,category FROM patients";
            loaddata(query);
           
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Gridview_Load(object sender, EventArgs e)
        {
            
        }
    }
}
