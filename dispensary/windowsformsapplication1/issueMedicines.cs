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
    public partial class issueMedicines : Form
    {
        string username;
        string[] medicinesID;
        string[] medicines;
        string[] dosage;
        string[] qty;
        string[] medicinesIDStore = new string[0];
        string[] medicinesStore = new string[0];
        string[] dosageStore = new string[0];
        string[] qtyStore = new string[0];
        string[] medicinesIDDisp = new string[0];
        string[] medicinesDisp = new string[0];
        string[] dosageDisp = new string[0];
        string[] qtyDisp = new string[0];
        bool issuedLP = false;
        public issueMedicines(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }
        public void loadPatients()
        {
            opd opd = new opd();
            opd.loadVisitedPatients();
            dataGridView1.DataSource = opd.table;
        }
        public void loadPatients(string opd)
        {
            opd opd2 = new opd();
            opd2.loadVisitedPatients(opd);
            dataGridView1.DataSource = opd2.table;
        }
        public void loadPatientsName(string name)
        {
            opd opd2 = new opd();
            opd2.loadVisitedPatientsName(name);
            dataGridView1.DataSource = opd2.table;
        }
        public void clear()
        {
            dataGridView2.Rows.Clear();
            textBox8.Text = "";
            textBox10.Text = "";
            textBox9.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox5.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox6.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
            textBox11.Text = "";
            dataGridView1.ClearSelection();
        }
        private void issueMedicines_Load(object sender, EventArgs e)
        {
            loadPatients();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.ClearSelection();
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int i = e.RowIndex;
            //MessageBox.Show(i.ToString());
            if (i >= 0)
            {
                dataGridView2.Rows.Clear();
                //string query = "SELECT opd.opd, opd.patient_id, patients.name, patients.dob, patients.gender, opd.doctor, patients.category, patients.family_head, opd.symptoms, opd.remarks, opd.medicine_id, opd.medicines, opd.dosage, opd.quantity, opd.storemedicine_id, opd.storequantity FROM opd INNER JOIN patients ON opd.patient_id=patients.patient_id WHERE opd.opd="+opd;
                dbconnect db = new dbconnect();
                opd opd = new opd();
                opd.loadOPDDetails(dataGridView1.Rows[i].Cells[0].Value.ToString(), db);
                if (opd.dr.Read())
                {
                    textBox8.Text = opd.dr[0].ToString();
                    textBox10.Text = opd.dr[1].ToString();
                    textBox9.Text = opd.dr[2].ToString();
                    int age = DateTime.Today.Year - Convert.ToInt32(opd.dr[3].ToString().Substring(0, 4));
                    if (Convert.ToInt32(opd.dr[3].ToString().Substring(5, 2)) > DateTime.Today.Month)
                    {
                        age--;
                    }
                    textBox1.Text = age.ToString();
                    textBox2.Text = opd.dr[4].ToString();
                    textBox5.Text = opd.dr[5].ToString();
                    textBox3.Text = opd.dr[6].ToString();
                    textBox4.Text = opd.dr[7].ToString();
                    textBox6.Text = opd.dr[8].ToString();
                    textBox13.Text = opd.dr[9].ToString();
                    medicinesID = new string[0];
                    medicines = new string[0];
                    dosage = new string[0];
                    qty = new string[0];
                    medicinesID = opd.dr[10].ToString().Split(',');
                    medicines = opd.dr[11].ToString().Split(',');
                    dosage = opd.dr[12].ToString().Split(',');
                    qty = opd.dr[13].ToString().Split(',');
                    if (opd.dr[14].ToString() != "")
                    {
                        medicinesIDStore = opd.dr[14].ToString().Split(',');
                        qtyStore = opd.dr[15].ToString().Split(',');
                    }
                    for (int q = 0; q < medicinesID.Length; q++)
                    {
                        dataGridView2.Rows.Add(medicinesID[q], medicines[q], qty[q], dosage[q]);
                    }
                    dataGridView2.ClearSelection();
                    issuedLP = medicinesID.Contains("LP");
                }
                db.dbclose();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadPatients();
            clear();
        }

        private void textBox11_KeyUp(object sender, KeyEventArgs e)
        {
            loadPatientsName(textBox11.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView2_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox8.Text != "")
            {
                if (MessageBox.Show("Are you sure you want to Print Duplicate Patient Slip??", "Duplicat Patient Slip??", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                     //   printPreviewDialog1.Document = printDocument1;
                     //   printPreviewDialog1.ShowDialog();
                        printDocument1.Print();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Select A Patient!!");
            }
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            int extra = 0;
            if (issuedLP)
            {
                extra += 21;
            }
            if (textBox6.Text != "")
            {
                extra += 35;
            }
            if (textBox13.Text != "")
            {
                extra += 35;
            }
            int height = Convert.ToInt32(245 + extra + (35 * (medicinesID.Length)));
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("token", 275, height);
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Pixel;
            Font messageFont = new Font("Times New Roman", 9, System.Drawing.GraphicsUnit.Point);
            g.DrawString("DISPENSARY, IIT ROORKEE", messageFont, Brushes.Black, 70, 0);
            g.DrawString("SAHARANPUR CAMPUS", messageFont, Brushes.Black, 90, 25);
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, 40);
            g.DrawString("PRESCRIPTION LIST(DUPLICATE)", messageFont, Brushes.Black, 50, 60);
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, 80);
            g.DrawString("Name : " + textBox9.Text, messageFont, Brushes.Black, 10, 100);
            g.DrawString("Age : " + textBox1.Text + " Years", messageFont, Brushes.Black, 10, 130);
            g.DrawString("Gender : " + textBox2.Text, messageFont, Brushes.Black, 280, 130);
            g.DrawString("Booklet No. : " + textBox10.Text, messageFont, Brushes.Black, 10, 160);
            g.DrawString("OPD No. : " + textBox8.Text, messageFont, Brushes.Black, 250, 160);
            g.DrawString(textBox5.Text, messageFont, Brushes.Black, 10, 190);
            g.DrawString("Date : " + DateTime.Now.ToString("MMM dd, yyyy"), messageFont, Brushes.Black, 220, 190);
            int height = 220;
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            if (textBox6.Text != "")
            {
                Rectangle rect = new Rectangle(new Point(10, height), new Size(350, 50));
                g.DrawString("Symptoms : " + textBox6.Text, messageFont, Brushes.Black, rect, format);
                height += 50;
            }
            if (textBox13.Text != "")
            {
                Rectangle rect2 = new Rectangle(new Point(10, height), new Size(350, 50));
                g.DrawString("Remarks : " + textBox13.Text, messageFont, Brushes.Black, rect2, format);
                height += 50;
            }
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height); //460
            g.DrawString("SNo.", messageFont, Brushes.Black, 0, height + 20);
            g.DrawString("Medicine Name", messageFont, Brushes.Black, 60, height + 20);
            g.DrawString("Qty", messageFont, Brushes.Black, 360, height + 20);
            g.DrawString("(Dosage)", messageFont, Brushes.Black, 80, height + 45);
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height + 60);
            height += 80;
            int j = 1;
            for (int i = 0; i < medicinesID.Length; i++)
            {
                if (medicinesID[i].Trim() != "LP")
                {
                    g.DrawString((j).ToString(), messageFont, Brushes.Black, 0, height);
                    Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                    g.DrawString(medicines[i].Trim() + " -- (" + dosage[i].Trim() + ")", messageFont, Brushes.Black, rect3, format);
                    g.DrawString(qty[i].Trim(), messageFont, Brushes.Black, 360, height);
                    height += 50;
                    j++;
                }
            }
            if (issuedLP)
            {
                g.DrawString("--------------------------(OWN LP)--------------------------", messageFont, Brushes.Black, 0, height);
                height += 30;
            }
            for (int i = 0; i < medicinesID.Length; i++)
            {
                if (medicinesID[i].Trim() == "LP")
                {
                    g.DrawString((j).ToString(), messageFont, Brushes.Black, 0, height);
                    Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                    g.DrawString(medicines[i].Trim() + " -- (" + dosage[i].Trim() + ")", messageFont, Brushes.Black, rect3, format);
                    g.DrawString(qty[i].Trim(), messageFont, Brushes.Black, 360, height);
                    height += 50;
                    j++;
                }
            }
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height);
            g.DrawString("IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 90, height + 20);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox8.Text != "")
            {
                medicinesIDDisp = new string[0];
                medicinesDisp = new string[0];
                dosageDisp = new string[0];
                qtyDisp = new string[0];
                for (int i = 0; i < medicinesID.Length; i++)
                {
                    medicinesID[i] = medicinesID[i].Trim();
                    medicines[i] = medicines[i].Trim();
                    dosage[i] = dosage[i].Trim();
                    qty[i] = qty[i].Trim();
                    if (medicinesID[i] != "LP")
                    {
                        if (medicinesIDStore.Contains(medicinesID[i]))
                        {
                            int issuedQty = Convert.ToInt32(qty[i]) - Convert.ToInt32(qtyStore[Array.IndexOf(medicinesIDStore, medicinesID[i])]);
                            if (issuedQty != 0)
                            {
                                Array.Resize<string>(ref medicinesIDDisp, (medicinesIDDisp.Length + 1));
                                medicinesIDDisp[medicinesIDDisp.Length - 1] = medicinesID[i].Trim();
                                Array.Resize<string>(ref medicinesDisp, (medicinesDisp.Length + 1));
                                medicinesDisp[medicinesDisp.Length - 1] = medicines[i].Trim();
                                Array.Resize<string>(ref qtyDisp, (qtyDisp.Length + 1));
                                qtyDisp[qtyDisp.Length - 1] = issuedQty.ToString();
                                Array.Resize<string>(ref dosageDisp, (dosageDisp.Length + 1));
                                dosageDisp[dosageDisp.Length - 1] = dosage[i].Trim();
                            }
                        }
                        else
                        {
                            Array.Resize<string>(ref medicinesIDDisp, (medicinesIDDisp.Length + 1));
                            medicinesIDDisp[medicinesIDDisp.Length - 1] = medicinesID[i].Trim();
                            Array.Resize<string>(ref medicinesDisp, (medicinesDisp.Length + 1));
                            medicinesDisp[medicinesDisp.Length - 1] = medicines[i].Trim();
                            Array.Resize<string>(ref qtyDisp, (qtyDisp.Length + 1));
                            qtyDisp[qtyDisp.Length - 1] = qty[i].Trim();
                            Array.Resize<string>(ref dosageDisp, (dosageDisp.Length + 1));
                            dosageDisp[dosageDisp.Length - 1] = dosage[i].Trim();
                        }
                    }
                }
               // MessageBox.Show(medicinesID.Length.ToString());
              //  MessageBox.Show(medicinesIDStore.Length.ToString());
               // MessageBox.Show(medicinesIDDisp.Length.ToString());
              //  MessageBox.Show(medicinesIDStore[1]);
              //  MessageBox.Show(Array.IndexOf(medicinesID, medicinesIDStore[1].Trim()).ToString());
                if (MessageBox.Show("Are you sure you want to Print Duplicate Patient Slip(Complete)??", "Duplicat Patient Slip??", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                     //  printPreviewDialog1.Document = printDocument2;
                     //   printPreviewDialog1.ShowDialog();
                        printDocument2.Print();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                    }
                }

            }
            else
            {
                MessageBox.Show("Plese Select A Patient!!");
            }
        }

        private void printDocument2_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            int extra = 0;
            int countLP = 0;
            if (medicinesIDStore.Length != 0)
            {
                extra += 21;
            }
            if (issuedLP)
            {
                extra += 21;
                for (int i = 0; i < medicinesID.Length; i++)
                {
                    if (medicinesID[i] == "LP")
                    {
                        countLP++;
                    }
                }
            }
            if (textBox6.Text != "")
            {
                extra += 35;
            }
            if (textBox13.Text != "")
            {
                extra += 35;
            }
            int height = Convert.ToInt32(245 + extra + (35 * (medicinesIDStore.Length + medicinesIDDisp.Length + countLP)));
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("token", 275, height);
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Pixel;
            Font messageFont = new Font("Times New Roman", 9, System.Drawing.GraphicsUnit.Point);
            g.DrawString("DISPENSARY, IIT ROORKEE", messageFont, Brushes.Black, 70, 0);
            g.DrawString("SAHARANPUR CAMPUS", messageFont, Brushes.Black, 90, 25);
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, 40);
            g.DrawString("PRESCRIPTION LIST(DUPLICATE)", messageFont, Brushes.Black, 50, 60);
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, 80);
            g.DrawString("Name : " + textBox9.Text, messageFont, Brushes.Black, 10, 100);
            g.DrawString("Age : " + textBox1.Text + " Years", messageFont, Brushes.Black, 10, 130);
            g.DrawString("Gender : " + textBox2.Text, messageFont, Brushes.Black, 280, 130);
            g.DrawString("Booklet No. : " + textBox10.Text, messageFont, Brushes.Black, 10, 160);
            g.DrawString("OPD No. : " + textBox8.Text, messageFont, Brushes.Black, 250, 160);
            g.DrawString(textBox5.Text, messageFont, Brushes.Black, 10, 190);
            g.DrawString("Date : " + DateTime.Now.ToString("MMM dd, yyyy"), messageFont, Brushes.Black, 220, 190);
            int height = 220;
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            if (textBox6.Text != "")
            {
                Rectangle rect = new Rectangle(new Point(10, height), new Size(350, 50));
                g.DrawString("Symptoms : " + textBox6.Text, messageFont, Brushes.Black, rect, format);
                height += 50;
            }
            if (textBox13.Text != "")
            {
                Rectangle rect2 = new Rectangle(new Point(10, height), new Size(350, 50));
                g.DrawString("Remarks : " + textBox13.Text, messageFont, Brushes.Black, rect2, format);
                height += 50;
            }
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height); //460
            g.DrawString("SNo.", messageFont, Brushes.Black, 0, height + 20);
            g.DrawString("Medicine Name", messageFont, Brushes.Black, 60, height + 20);
            g.DrawString("Qty", messageFont, Brushes.Black, 360, height + 20);
            g.DrawString("(Dosage)", messageFont, Brushes.Black, 80, height + 45);
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height + 60);
            height += 80;
            for (int i = 0; i < medicinesIDDisp.Length; i++)
            {
                g.DrawString((i + 1).ToString(), messageFont, Brushes.Black, 0, height);
                Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                g.DrawString(medicinesDisp[i] + " -- (" + dosageDisp[i] + ")", messageFont, Brushes.Black, rect3, format);
                g.DrawString(qtyDisp[i], messageFont, Brushes.Black, 360, height);
                height += 50;
            }
            if (medicinesIDStore.Length != 0)
            {
                g.DrawString("-------------------------(STORE)-----------------------", messageFont, Brushes.Black, 0, height);
                height += 30;
            }
            for (int i = 0; i < medicinesIDStore.Length; i++)
            {
                g.DrawString((i + 1).ToString(), messageFont, Brushes.Black, 0, height);
                Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                g.DrawString(medicines[Array.IndexOf(medicinesID, medicinesIDStore[i].Trim())] + " -- (" + dosage[Array.IndexOf(medicinesID, medicinesIDStore[i].Trim())] + ")", messageFont, Brushes.Black, rect3, format);
                g.DrawString(qtyStore[i], messageFont, Brushes.Black, 360, height);
                height += 50;
            }
            if (issuedLP)
            {
                g.DrawString("------------------------------(OWN LP)-----------------------", messageFont, Brushes.Black, 0, height);
                height += 50;
            }
            int j = 1;
            for (int i = 0; i < medicinesID.Length; i++)
            {
                if (medicinesID[i].Trim() == "LP")
                {
                    g.DrawString((j).ToString(), messageFont, Brushes.Black, 0, height);
                    Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                    g.DrawString(medicines[i].Trim() + " -- (" + dosage[i].Trim() + ")", messageFont, Brushes.Black, rect3, format);
                    g.DrawString(qty[i].Trim(), messageFont, Brushes.Black, 360, height);
                    height += 50;
                    j++;
                }
            }
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height);
            g.DrawString("IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 90, height + 20);
        }

        private void textBox12_KeyUp(object sender, KeyEventArgs e)
        {
            loadPatients(textBox12.Text);
        }
    }
}
