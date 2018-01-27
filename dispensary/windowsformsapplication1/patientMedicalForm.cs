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
    public partial class patientMedicalForm : Form
    {
        string username;
        string fname;
        string lname;
        string opd;
        string[] issued = new string[0];
        string[] issuedLP = new string[0];
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
        string[] medicinesLP = new string[0];
        string[] dosageLP = new string[0];
        string[] qtyLP = new string[0];
        string[] type_1 = new string[] { "Injection", "Syringe", "Syrup", "Lotion", "Cream", "Ointment", "Suspension", "Inhalar", "Respirator", "Solution", "Mouth Wash", "Nesal Drop", "Ear Drop", "Eye Drop" };
        string[] type_40 = new string[] { "Tablet", "Capsule", "Rotocap" };
        public int opdCount = 0;
        public int medicineCount = 0;
        public string[] medicine_idHistory = new string[0];
        public string[] medicinesHistory = new string[0];
        public string[] dosageHistory = new string[0];
        public string[] quantityHistory = new string[0];
        public string patient_name;
        public string gender;
        public string patient_id;
        public string age;
        public string familyHead = "";
        public void medicalFormLoad()
        {
            opd opd2 = new opd();
            dbconnect db = new dbconnect();
            opd2.getPatientDetails(opd,db);
            if (opd2.dr.Read())
            {
                textBox1.Text = opd2.dr[0].ToString();
                textBox2.Text = opd2.dr[1].ToString();
                textBox5.Text = opd2.dr[2].ToString();
                textBox3.Text = opd;
                int age = DateTime.Today.Year - Convert.ToInt32(opd2.dr[3].ToString().Substring(0, 4));
                if (Convert.ToInt32(opd2.dr[3].ToString().Substring(5, 2)) > DateTime.Today.Month)
                {
                    age--;
                }
                textBox4.Text = age.ToString();
            }
            else
            {
                //MessageBox.Show(opd2.message);
            }
            db.dbclose();
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
            textBox7.Text = (Convert.ToInt32(med.count) - 1).ToString();
            clearData();
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
        public void clearData()
        {
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            comboBox1.SelectedIndex = -1;
            textBox14.Text = "";
            comboBox2.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            panel5.Enabled = false;
            groupBox3.Enabled = true;
            comboBox7.SelectedIndex = -1;
            comboBox6.SelectedIndex = -1;
            textBox18.Text = "";
            textBox17.Text = "";
        }
        public void medicineDetail(string id)
        {
            medicine med = new medicine();
            dbconnect db = new dbconnect("medicines");
            med.medicineDetail(id,db);
            if (med.dr.Read())
            {
                textBox9.Text = med.dr[2].ToString();
                textBox10.Text = med.dr[3].ToString();
                textBox11.Text = med.dr[4].ToString();
                textBox12.Text = med.dr[5].ToString();
                textBox13.Text = "";
                comboBox1.SelectedIndex = -1;    
            }
            db.dbclose();
        }
        public patientMedicalForm(string user,string OPD)
        {
            InitializeComponent();
            username = user;
            opd = OPD;
            users userObj = new users(user);
            fname = userObj.fname;
            lname = userObj.lname;
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void patientMedicalForm_Load(object sender, EventArgs e)
        {
            medicalFormLoad();
            loadMedicines();
            dataGridView1.ClearSelection();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            printPreviewDialog1.PrintPreviewControl.Zoom = 1.0;
            ((Form)printPreviewDialog1).StartPosition = FormStartPosition.CenterScreen;
            ((Form)printPreviewDialog1).Width = 880;
            ((Form)printPreviewDialog1).Height = 680;
        }

        private void textBox8_KeyUp(object sender, KeyEventArgs e)
        {
            loadMedicines(textBox8.Text);
            textBox8.Focus();
            if (listBox1.Items.Count == 0)
            {
                clearData();
                dataGridView1.ClearSelection();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadMedicines();
            textBox8.Text = "";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                clearData();
                medicineDetail(listBox1.SelectedValue.ToString());
                dataGridView1.ClearSelection();
                textBox13.Focus();
                
            }
            else if (listBox1.SelectedIndex == -1)
            {
                clearData();
                dataGridView1.ClearSelection();
                textBox13.Focus();
            }
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
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

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
                if (textBox9.Text != "")
                {
                    if (issued.Contains(listBox1.SelectedValue.ToString()))
                    {
                        MessageBox.Show("Medicine Already issued.\n If you want any changes to be made please use UPDATE option!!");
                    }
                    else
                    {
                        if (comboBox1.SelectedIndex >= 0 && comboBox4.SelectedIndex>=0)
                        {
                            if (textBox13.Text != "")
                            {
                                bool valid = true;
                                if (type_1.Contains(textBox10.Text) && Convert.ToInt32(textBox13.Text) > 1)
                                {
                                    if (MessageBox.Show("Are you sure that you want to issue quantity more than 1?", "Confirm issue", MessageBoxButtons.YesNo) == DialogResult.No)
                                    {
                                        valid = false;
                                    }
                                }
                                if (type_40.Contains(textBox10.Text) && Convert.ToInt32(textBox13.Text) > 40)
                                {
                                    if (MessageBox.Show("Are you sure that you want to issue quantity more than 40?", "Confirm issue", MessageBoxButtons.YesNo) == DialogResult.No)
                                    {
                                        valid = false;
                                    }
                                }
                                if (valid)
                                {
                                    int storeQty = Convert.ToInt32(textBox11.Text);
                                    int dispQty = Convert.ToInt32(textBox12.Text);
                                    int issuedQty = Convert.ToInt32(textBox13.Text);
                                    int total = Convert.ToInt32(label18.Text);
                                    if (storeQty == 0 && dispQty == 0)
                                    {
                                        MessageBox.Show("Medicine is neither available in Store nor in Dispensary!!!");
                                    }
                                    else
                                    {

                                        if (storeQty + dispQty - issuedQty < 0)
                                        {
                                            dataGridView1.Rows.Insert(total, listBox1.SelectedValue.ToString(), textBox9.Text, (storeQty + dispQty).ToString(), comboBox4.Text + " " + comboBox1.Text);
                                            MessageBox.Show("Issued quantity was unavailabe in either in dispensary or in store. So only available amount was issued!!");
                                        }
                                        else
                                        {
                                            dataGridView1.Rows.Insert(total, listBox1.SelectedValue.ToString(), textBox9.Text, (issuedQty).ToString(), comboBox4.Text + " " + comboBox1.Text);
                                        }
                                        label18.Text = (total + 1).ToString();
                                        Array.Resize<string>(ref issued, (issued.Length + 1));
                                        issued[issued.Length - 1] = listBox1.SelectedValue.ToString();
                                        loadMedicines();
                                        textBox8.Text = "";
                                        dataGridView1.ClearSelection();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please Enter Quantity!!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Select Dosage!!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Select a Medicine!!");
                }
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                medicineDetail(dataGridView1.Rows[i].Cells[0].Value.ToString());
                textBox14.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
                string dosageSel = dataGridView1.Rows[i].Cells[3].Value.ToString();
                comboBox5.SelectedItem = dosageSel.Substring(0,dosageSel.IndexOf(" "));
                comboBox2.SelectedItem = dosageSel.Substring(dosageSel.IndexOf(" ") + 1);
                panel5.Enabled = true;
                groupBox3.Enabled = false;
                listBox1.ClearSelected();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex >= 0 && comboBox5.SelectedIndex >= 0)
            {
                if (textBox14.Text != "")
                {
                    bool valid = true;
                    if (type_1.Contains(textBox10.Text) && Convert.ToInt32(textBox14.Text) > 1)
                    {
                        if (MessageBox.Show("Are you sure that you want to issue quantity more than 1?", "Confirm issue", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            valid = false;
                        }
                    }
                    if (type_40.Contains(textBox10.Text) && Convert.ToInt32(textBox14.Text) > 40)
                    {
                        if (MessageBox.Show("Are you sure that you want to issue quantity more than 40?", "Confirm issue", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            valid = false;
                        }
                    }
                    if (valid)
                    {
                        int storeQty = Convert.ToInt32(textBox11.Text);
                        int dispQty = Convert.ToInt32(textBox12.Text);
                        int issuedQty = Convert.ToInt32(textBox14.Text);
                        if (dispQty + storeQty < issuedQty)
                        {

                            dataGridView1.SelectedRows[0].Cells[2].Value = (dispQty + storeQty).ToString();
                            dataGridView1.SelectedRows[0].Cells[3].Value = comboBox5.Text + " " + comboBox2.Text;
                            loadMedicines();
                            dataGridView1.ClearSelection();
                            MessageBox.Show("Issued quantity was unavailabe in either in dispensary or in store. So only available amount was issued!!");
                        }
                        else
                        {
                            dataGridView1.SelectedRows[0].Cells[2].Value = (issuedQty).ToString();
                            dataGridView1.SelectedRows[0].Cells[3].Value = comboBox5.Text + " " + comboBox2.Text;
                            loadMedicines();
                            dataGridView1.ClearSelection();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter Quantity!!");
                    textBox14.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please Select DOSAGE!!!");
                comboBox2.Focus();
            }
        }


        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete it from list??","Delete medicines??",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (dataGridView1.SelectedRows[0].Cells[0].Value.ToString() != "LP")
                {
                    var list = new List<string>(issued);
                    list.Remove(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    issued = list.ToArray();
                    label18.Text = (Convert.ToInt32(label18.Text) - 1).ToString();
                }
                else
                {
                    var list = new List<string>(issuedLP);
                    list.Remove(dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
                    issuedLP = list.ToArray();
                }
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                dataGridView1.ClearSelection();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to ALL MEDICINES from list??", "Delete ALL medicines??", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i += 0)
                {
                    dataGridView1.Rows.RemoveAt(0);
                }
                label18.Text = "00";
                issued = new string[0];
                issuedLP = new string[0];
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                int total = Convert.ToInt32(label18.Text);
                if (dataGridView1.Rows.Count > 0)
                {
                    if (MessageBox.Show("Are you sure you want to Issue Medicines??", "Issue medicines??", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        button6.Enabled = false;
                        medicinesID = new string[dataGridView1.Rows.Count];
                        medicines = new string[dataGridView1.Rows.Count];
                        dosage = new string[dataGridView1.Rows.Count];
                        qty = new string[dataGridView1.Rows.Count];
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (i < total)
                            {
                                medicineDetail(dataGridView1.Rows[i].Cells[0].Value.ToString());
                                int storeQty = Convert.ToInt32(textBox11.Text);
                                int dispQty = Convert.ToInt32(textBox12.Text);
                                int issuedQty = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
                                if (issuedQty > dispQty)
                                {
                                    if (dispQty != 0)
                                    {
                                        Array.Resize<string>(ref medicinesIDDisp, (medicinesIDDisp.Length + 1));
                                        medicinesIDDisp[medicinesIDDisp.Length - 1] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                        Array.Resize<string>(ref medicinesDisp, (medicinesDisp.Length + 1));
                                        medicinesDisp[medicinesDisp.Length - 1] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                        Array.Resize<string>(ref qtyDisp, (qtyDisp.Length + 1));
                                        qtyDisp[qtyDisp.Length - 1] = dispQty.ToString();
                                        Array.Resize<string>(ref dosageDisp, (dosageDisp.Length + 1));
                                        dosageDisp[dosageDisp.Length - 1] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                    }
                                    Array.Resize<string>(ref medicinesIDStore, (medicinesIDStore.Length + 1));
                                    medicinesIDStore[medicinesIDStore.Length - 1] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                    Array.Resize<string>(ref medicinesStore, (medicinesStore.Length + 1));
                                    medicinesStore[medicinesStore.Length - 1] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                    Array.Resize<string>(ref qtyStore, (qtyStore.Length + 1));
                                    qtyStore[qtyStore.Length - 1] = (issuedQty - dispQty).ToString();
                                    Array.Resize<string>(ref dosageStore, (dosageStore.Length + 1));
                                    dosageStore[dosageStore.Length - 1] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                }
                                else
                                {
                                    Array.Resize<string>(ref medicinesIDDisp, (medicinesIDDisp.Length + 1));
                                    medicinesIDDisp[medicinesIDDisp.Length - 1] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                    Array.Resize<string>(ref medicinesDisp, (medicinesDisp.Length + 1));
                                    medicinesDisp[medicinesDisp.Length - 1] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                    Array.Resize<string>(ref qtyDisp, (qtyDisp.Length + 1));
                                    qtyDisp[qtyDisp.Length - 1] = issuedQty.ToString();
                                    Array.Resize<string>(ref dosageDisp, (dosageDisp.Length + 1));
                                    dosageDisp[dosageDisp.Length - 1] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                }
                            }
                            else
                            {
                                Array.Resize<string>(ref medicinesLP, (medicinesLP.Length + 1));
                                medicinesLP[medicinesLP.Length - 1] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                Array.Resize<string>(ref qtyLP, (qtyLP.Length + 1));
                                qtyLP[qtyLP.Length - 1] = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                Array.Resize<string>(ref dosageLP, (dosageLP.Length + 1));
                                dosageLP[dosageLP.Length - 1] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                            }
                            medicinesID[i] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            medicines[i] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                            qty[i] = dataGridView1.Rows[i].Cells[2].Value.ToString();
                            dosage[i] = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        }
                        // MessageBox.Show(String.Join(", ", medicinesID));
                        // MessageBox.Show(String.Join(", ", medicinesIDStore));
                        // MessageBox.Show(String.Join(", ", medicinesIDDisp));
                        // MessageBox.Show(String.Join(", ", medicinesLP));
                        for (int i = 0; i < medicinesIDDisp.Length; i++)
                        {
                            opd opdObj = new opd();
                            opdObj.updateIssuedMedicine(medicinesIDDisp[i], qtyDisp[i], textBox3.Text, textBox1.Text, textBox2.Text, fname + " " + lname);
                        }
                        for (int i = 0; i < medicinesIDStore.Length; i++)
                        {
                            opd opdObj = new opd();
                            opdObj.updateIssuedMedicineStore(medicinesIDStore[i], qtyStore[i], textBox3.Text, textBox1.Text, textBox2.Text, fname + " " + lname);
                        }
                        opd opdObj2 = new opd();
                        opdObj2.updateOPD(textBox3.Text, textBox1.Text, String.Join(", ", medicinesID), String.Join(", ", medicines), String.Join(", ", qty), String.Join(", ", medicinesIDStore), String.Join(", ", qtyStore), String.Join(", ", dosage), textBox15.Text, textBox16.Text, fname + " " + lname);
                        //   printPreviewDialog1.Document = printDocument1;
                        //   printPreviewDialog1.ShowDialog();

                        //   printPreviewDialog1.Document = printDocument2;
                        //   printPreviewDialog1.ShowDialog();
                        try
                        {
                            printDocument1.Print();
                            //printDocument2.Print();    
                        }
                        catch (Exception Ex)
                        {
                            MessageBox.Show(Ex.Message + "\n\nPrinter Not Connected Properly!! Please Check that and then Print!!");
                        }
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Medicines to issue!!");
                }
            }
            else
            {
                if (textBox19.Text != "")
                {
                    if (MessageBox.Show("Are you sure you want Refer Patient and Not issue Medicines??", "Refer??", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        opd opdObj2 = new opd();
                        opdObj2.updateOPD(textBox3.Text, textBox1.Text, "", "", "", "", "", "", textBox15.Text, textBox16.Text + textBox19.Text, fname + " " + lname);
                        printDocument1.Print();
                        //printPreviewDialog1.Document = printDocument1;
                        //printPreviewDialog1.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter Whom do you want to refer the patient to!!");
                    textBox19.Focus();
                }
            }
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            int extra = 0;
            if (medicinesIDStore.Length != 0)
            {
                extra += 21;
            }
            if (medicinesLP.Length != 0)
            {
                extra += 21;
            }
            if (textBox15.Text != "")
            {
                extra += 35;
            }
            if (textBox16.Text != "" || textBox19.Text != "")
            {
                extra += 35;
            }
            int height = Convert.ToInt32(245 + extra + (35 * (medicinesIDStore.Length + medicinesIDDisp.Length + medicinesLP.Length)));
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
            g.DrawString("PRESCRIPTION LIST", messageFont, Brushes.Black, 50, 60);
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, 80);
            g.DrawString("Name : "+textBox2.Text, messageFont, Brushes.Black, 10, 100);
            g.DrawString("Age : "+textBox4.Text+" Years", messageFont, Brushes.Black, 10, 130);
            g.DrawString("Gender : " + textBox5.Text, messageFont, Brushes.Black, 280, 130);
            g.DrawString("Booklet No. : " + textBox1.Text, messageFont, Brushes.Black, 10, 160);
            g.DrawString("OPD No. : " + textBox3.Text, messageFont, Brushes.Black, 250, 160);
            g.DrawString(fname + " " + lname, messageFont, Brushes.Black, 10, 190);
            g.DrawString("Date : " + DateTime.Now.ToString("MMM dd, yyyy"), messageFont, Brushes.Black, 220, 190);
            int height = 220;
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            if (textBox15.Text != "")
            {
                Rectangle rect = new Rectangle(new Point(10, height), new Size(350, 50));
                g.DrawString("Symptoms : " + textBox15.Text, messageFont, Brushes.Black, rect, format);
                height += 50;
            }
            if (textBox16.Text != "" || textBox19.Text != "")
            {
                Rectangle rect2 = new Rectangle(new Point(10, height), new Size(350, 50));
                g.DrawString("Remarks : " + textBox16.Text + textBox19.Text, messageFont, Brushes.Black, rect2, format);
                height += 50;
            }
            if (radioButton2.Checked)
            {
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
                    g.DrawString("----------------------------(STORE)-----------------------", messageFont, Brushes.Black, 0, height);
                    height += 30;
                }
                for (int i = 0; i < medicinesIDStore.Length; i++)
                {
                    g.DrawString((i + 1).ToString(), messageFont, Brushes.Black, 0, height);
                    Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                    g.DrawString(medicinesStore[i] + " -- (" + dosageStore[i] + ")", messageFont, Brushes.Black, rect3, format);
                    g.DrawString(qtyStore[i], messageFont, Brushes.Black, 360, height);
                    height += 50;
                }
                if (medicinesLP.Length != 0)
                {
                    g.DrawString("-----------------------------(OWN LP)-----------------------", messageFont, Brushes.Black, 0, height);
                    height += 30;
                }
                for (int i = 0; i < medicinesLP.Length; i++)
                {
                    g.DrawString((i + 1).ToString(), messageFont, Brushes.Black, 0, height);
                    Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                    g.DrawString(medicinesLP[i] + " -- (" + dosageLP[i] + ")", messageFont, Brushes.Black, rect3, format);
                    g.DrawString(qtyLP[i], messageFont, Brushes.Black, 360, height);
                    height += 50;
                }
            }
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height);
            g.DrawString("IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 90, height +20);
        }

        private void panel3_VisibleChanged(object sender, EventArgs e)
        {
            if (panel3.Visible)
            { 
                opd opd2 = new opd();
                dbconnect db = new dbconnect();
                opd2.getPatientOPDHistory(textBox1.Text,db);
                while (opd2.dr.Read())
                {
                    comboBox3.Items.Add(opd2.dr.GetValue(0).ToString() + " - (" + opd2.dr.GetValue(1).ToString().Substring(0, 4) + "-" + opd2.dr.GetValue(1).ToString().Substring(4, 2) + "-" + opd2.dr.GetValue(1).ToString().Substring(6, 2) + ")");
                }
                db.dbclose();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (panel3.Visible)
            {
                panel3.Visible = false;
                comboBox3.Items.Clear();
                for (int i = 0; i < dataGridView2.Rows.Count; i += 0)
                {
                    dataGridView2.Rows.RemoveAt(0);
                }
            }
            else
            {
                panel3.Visible = true;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i += 0)
            {
                dataGridView2.Rows.RemoveAt(0);
            }         
            string opd2 = comboBox3.Text.Substring(0, comboBox3.Text.IndexOf(" - ("));
            opd opdObj = new opd();
            dbconnect db = new dbconnect();
            opdObj.getPatientMedicineHistory(opd2,db);
            string[] medicine_id2 = new string[0];
            string[] medicines2 = new string[0];
            string[] dosage2 = new string[0];
            string[] quantity2 = new string[0];
            if(opdObj.dr.Read())
            {
                medicine_id2 = opdObj.dr[0].ToString().Split(',');
                medicines2 = opdObj.dr[1].ToString().Split(',');
                dosage2 = opdObj.dr[2].ToString().Split(',');
                quantity2 = opdObj.dr[3].ToString().Split(',');
            }
            db.dbclose();
            for (int i = 0; i < medicine_id2.Length; i++)
            {
                dataGridView2.Rows.Insert(i, medicine_id2[i].Trim(), medicines2[i].Trim(), quantity2[i].Trim(), dosage2[i].Trim());
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            comboBox3.Items.Clear();
            for (int i = 0; i < dataGridView2.Rows.Count; i += 0)
            {
                dataGridView2.Rows.RemoveAt(0);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (issued.Contains(dataGridView2.SelectedRows[0].Cells[0].Value.ToString()))
            {
                MessageBox.Show("Medicine " + dataGridView2.SelectedRows[0].Cells[1].Value.ToString() + " Already issued.\n If you want any changes to be made please use UPDATE option!!");
            }
            else
            {
                if (dataGridView2.SelectedRows[0].Cells[0].Value.ToString() == "LP")
                {
                    if (issuedLP.Contains(dataGridView2.SelectedRows[0].Cells[1].Value.ToString()))
                    {
                        MessageBox.Show("Medicine " + dataGridView2.SelectedRows[0].Cells[1].Value.ToString() + " Already issued.\n If you want any changes to be made please use UPDATE option!!");
                    }
                    else
                    {
                        dataGridView1.Rows.Insert(dataGridView1.Rows.Count, dataGridView2.SelectedRows[0].Cells[0].Value.ToString(), dataGridView2.SelectedRows[0].Cells[1].Value.ToString(), dataGridView2.SelectedRows[0].Cells[2].Value.ToString(), dataGridView2.SelectedRows[0].Cells[3].Value.ToString());
                        Array.Resize<string>(ref issuedLP, (issuedLP.Length + 1));
                        issuedLP[issuedLP.Length - 1] = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                    }
                }
                else
                {
                    medicineDetail(dataGridView2.SelectedRows[0].Cells[0].Value.ToString());
                    int storeQty = Convert.ToInt32(textBox11.Text);
                    int dispQty = Convert.ToInt32(textBox12.Text);
                    int issuedQty = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[2].Value.ToString());
                    int total = Convert.ToInt32(label18.Text);
                    if (storeQty == 0 && dispQty == 0)
                    {
                        MessageBox.Show("Medicine is neither available in Store nor in Dispensary!!!");
                    }
                    else
                    {
                        if (storeQty + dispQty - issuedQty < 0)
                        {
                            dataGridView1.Rows.Insert(total, dataGridView2.SelectedRows[0].Cells[0].Value.ToString(), dataGridView2.SelectedRows[0].Cells[1].Value.ToString(), (storeQty + dispQty).ToString(), dataGridView2.SelectedRows[0].Cells[3].Value.ToString());
                            MessageBox.Show("Issued quantity was unavailabe in either in dispensary or in store. So only available amount was issued!!");
                        }
                        else
                        {
                            dataGridView1.Rows.Insert(total, dataGridView2.SelectedRows[0].Cells[0].Value.ToString(), dataGridView2.SelectedRows[0].Cells[1].Value.ToString(), issuedQty.ToString(), dataGridView2.SelectedRows[0].Cells[3].Value.ToString());
                        }
                        label18.Text = (total + 1).ToString();
                        Array.Resize<string>(ref issued, (issued.Length + 1));
                        issued[issued.Length - 1] = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                        loadMedicines();
                        dataGridView1.ClearSelection();
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(label18.Text);
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (issued.Contains(dataGridView2.Rows[i].Cells[0].Value.ToString()))
                {
                    MessageBox.Show("Medicine " + dataGridView2.Rows[i].Cells[1].Value.ToString() + " Already issued.\n If you want any changes to be made please use UPDATE option!!");
                }
                else
                {
                    if (dataGridView2.Rows[i].Cells[0].Value.ToString() == "LP")
                    {
                        if (issuedLP.Contains(dataGridView2.Rows[i].Cells[1].Value.ToString()))
                        {
                            MessageBox.Show("Medicine " + dataGridView2.Rows[i].Cells[1].Value.ToString() + " Already issued.\n If you want any changes to be made please use UPDATE option!!");
                        }
                        else
                        {
                            dataGridView1.Rows.Insert(dataGridView1.Rows.Count, dataGridView2.Rows[i].Cells[0].Value.ToString(), dataGridView2.Rows[i].Cells[1].Value.ToString(), dataGridView2.Rows[i].Cells[2].Value.ToString(), dataGridView2.Rows[i].Cells[3].Value.ToString());
                            Array.Resize<string>(ref issuedLP, (issuedLP.Length + 1));
                            issuedLP[issuedLP.Length - 1] = dataGridView2.Rows[i].Cells[1].Value.ToString();
                        }
                    }
                    else
                    {
                        medicineDetail(dataGridView2.Rows[i].Cells[0].Value.ToString());
                        int storeQty = Convert.ToInt32(textBox11.Text);
                        int dispQty = Convert.ToInt32(textBox12.Text);
                        int issuedQty = Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value.ToString());
                        int total = Convert.ToInt32(label18.Text);
                        if (storeQty == 0 && dispQty == 0)
                        {
                            MessageBox.Show("Medicine is neither available in Store nor in Dispensary!!!");
                        }
                        else
                        {

                            if (storeQty + dispQty - issuedQty < 0)
                            {
                                dataGridView1.Rows.Insert(total, dataGridView2.Rows[i].Cells[0].Value.ToString(), dataGridView2.Rows[i].Cells[1].Value.ToString(), (storeQty + dispQty).ToString(), dataGridView2.Rows[i].Cells[3].Value.ToString());
                                MessageBox.Show("Issued quantity was unavailabe in either in dispensary or in store. So only available amount was issued!!");
                            }
                            else
                            {
                                dataGridView1.Rows.Insert(total, dataGridView2.Rows[i].Cells[0].Value.ToString(), dataGridView2.Rows[i].Cells[1].Value.ToString(), issuedQty.ToString(), dataGridView2.Rows[i].Cells[3].Value.ToString());
                            }
                            label18.Text = (total + 1).ToString();
                            Array.Resize<string>(ref issued, (issued.Length + 1));
                            issued[issued.Length - 1] = dataGridView2.Rows[i].Cells[0].Value.ToString();
                            loadMedicines();
                            dataGridView1.ClearSelection();
                        }
                    }
                }
            }
            dataGridView1.ClearSelection();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox17.Text != "")
            {
                if (textBox18.Text != "")
                {
                    if (comboBox7.SelectedIndex >= 0 && comboBox6.SelectedIndex >= 0)
                    {
                        if (issuedLP.Contains(textBox17.Text))
                        {
                            MessageBox.Show("Medicine " + textBox17.Text + " Already issued.\n If you want any changes to be made please use UPDATE option!!");
                        }
                        else
                        {
                            dataGridView1.Rows.Insert(dataGridView1.Rows.Count, "LP", textBox17.Text, textBox18.Text, comboBox7.Text + " " + comboBox6.Text);
                            Array.Resize<string>(ref issuedLP, (issuedLP.Length + 1));
                            issuedLP[issuedLP.Length - 1] = textBox17.Text;
                            clearData();
                            dataGridView1.ClearSelection();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Dosage!!");
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter Qunatity!!");
                }
            }
            else
            {
                MessageBox.Show("Please Enter Medicine Name!!");
            }
        }

        private void printDocument2_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            int extra = 0;
            if (medicinesLP.Length != 0)
            {
                extra += 21;
            }
            if (textBox15.Text != "")
            {
                extra += 35;
            }
            if (textBox16.Text != "")
            {
                extra += 35;
            }
            int height = Convert.ToInt32(245 + extra + (35 * (medicinesID.Length)));
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
            g.DrawString("PRESCRIPTION LIST", messageFont, Brushes.Black, 50, 60);
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, 80);
            g.DrawString("Name : " + textBox2.Text, messageFont, Brushes.Black, 10, 100);
            g.DrawString("Age : " + textBox4.Text + " Years", messageFont, Brushes.Black, 10, 130);
            g.DrawString("Gender : " + textBox5.Text, messageFont, Brushes.Black, 280, 130);
            g.DrawString("Booklet No. : " + textBox1.Text, messageFont, Brushes.Black, 10, 160);
            g.DrawString("OPD No. : " + textBox3.Text, messageFont, Brushes.Black, 250, 160);
            g.DrawString(fname + " " + lname, messageFont, Brushes.Black, 10, 190);
            g.DrawString("Date : " + DateTime.Now.ToString("MMM dd, yyyy"), messageFont, Brushes.Black, 220, 190);
            int height = 220;
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            if (textBox15.Text != "")
            {
                Rectangle rect = new Rectangle(new Point(10, height), new Size(350, 50));
                g.DrawString("Symptoms : " + textBox15.Text, messageFont, Brushes.Black, rect, format);
                height += 50;
            }
            if (textBox16.Text != "")
            {
                Rectangle rect2 = new Rectangle(new Point(10, height), new Size(350, 50));
                g.DrawString("Remarks : " + textBox16.Text, messageFont, Brushes.Black, rect2, format);
                height += 50;
            }
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height); //460
            g.DrawString("SNo.", messageFont, Brushes.Black, 0, height + 20);
            g.DrawString("Medicine Name", messageFont, Brushes.Black, 60, height + 20);
            g.DrawString("Qty", messageFont, Brushes.Black, 360, height + 20);
            g.DrawString("(Dosage)", messageFont, Brushes.Black, 80, height + 45);
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height + 60);
            height += 80;
            for (int i = 0; i < medicinesID.Length; i++)
            {
                if (medicinesID[i] != "LP")
                {
                    g.DrawString((i + 1).ToString(), messageFont, Brushes.Black, 0, height);
                    Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                    g.DrawString(medicines[i] + " -- (" + dosage[i] + ")", messageFont, Brushes.Black, rect3, format);
                    g.DrawString(qty[i], messageFont, Brushes.Black, 360, height);
                    height += 50;
                 // opd opdObj = new opd();
                 // opdObj.updateIssuedMedicine(medicinesIDDisp[i], qtyDisp[i], textBox3.Text, textBox1.Text, textBox2.Text, fname +" "+ lname);
                }
            }
            if (medicinesLP.Length != 0)
            {
                g.DrawString("-----------------------------(OWN LP)-----------------------", messageFont, Brushes.Black, 0, height);
                height += 30;
            }
            for (int i = 0; i < medicinesLP.Length; i++)
            {
                g.DrawString((i + 1).ToString(), messageFont, Brushes.Black, 2, height);
                Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                g.DrawString(medicinesLP[i] + " -- (" + dosageLP[i] + ")", messageFont, Brushes.Black, rect3, format);
                g.DrawString(qtyLP[i], messageFont, Brushes.Black, 360, height);
                height += 50;
            }
            g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height);
            g.DrawString("IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 90, height + 20);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.getPatientDetailsHistory(textBox1.Text, db);
            if (opd.dr.Read())
            {
                patient_id = textBox1.Text;
                patient_name = opd.dr[0].ToString();
                gender = opd.dr[1].ToString();
                familyHead = opd.dr[2].ToString();
            }
            db.reader_close();
            opdCount = 0;
            medicineCount = 0;
            opd.patientAge(patient_id);
            age = opd.age.ToString();
            opd.getPatientOPDHistory(textBox1.Text, db);
            while (opd.dr.Read())
            {
                medicine_idHistory = opd.dr[2].ToString().Split(',');
                medicineCount += medicine_idHistory.Length;
                opdCount += 1;
            }
            db.dbclose();
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument3_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            e.PageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4 (210 x 297 mm)", 827, Convert.ToInt32(Math.Round((56 + (20 + 5) * opdCount + medicineCount * 6 + 8) * 3.9,1,MidpointRounding.AwayFromZero)));
            e.PageSettings.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
        }

        private void printDocument3_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            Font messageFont = new Font("Times New Roman", 14, System.Drawing.GraphicsUnit.Point);
            g.DrawString("INSTITUTE DISPENSARY", messageFont, Brushes.Black, 75, 9);
            g.DrawString("INDIAN INSTITUTE OF TECHNOLOGY ROORKEE, SAHARANPUR CAMPUS", messageFont, Brushes.Black, 22, 15);
            messageFont = new Font("Times New Roman", 20, System.Drawing.GraphicsUnit.Point);
            g.DrawString("PATIENT MEDICAL HISTORY", messageFont, Brushes.Black, 60, 25);
            Pen blackPen = new Pen(Color.Black, 1);
            g.DrawLine(blackPen, 5, 38, 205, 38);
            messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
            g.DrawString("Patient Name: " + patient_name, messageFont, Brushes.Black, 10, 42);
            g.DrawString("Patient ID: " + patient_id, messageFont, Brushes.Black, 10, 49);
            g.DrawString("Age: " + age + " Years", messageFont, Brushes.Black, 120, 42);
            g.DrawString("Gender: " + gender, messageFont, Brushes.Black, 160, 42);
            if (familyHead != "")
            {
                g.DrawString("Family Head ID: " + familyHead, messageFont, Brushes.Black, 120, 49);
            }
            g.DrawString("Dated: " + DateTime.Today.ToString("dd MMM, yyyy"), messageFont, Brushes.Black, 160, 30);
            g.DrawLine(blackPen, 5, 56, 205, 56);
            int total = 56;
            opd opd = new opd();
            dbconnect db = new dbconnect();
            opd.getPatientOPDHistory(textBox1.Text, db);
            int sno = 1;
            while (opd.dr.Read())
            {
                messageFont = new Font("Times New Roman", 12, System.Drawing.GraphicsUnit.Point);
                medicine_idHistory = new string[0];
                medicinesHistory = new string[0];
                dosageHistory = new string[0];
                quantityHistory = new string[0];
                g.DrawString(sno + ")", messageFont, Brushes.Black, 10, total + 2);
                g.DrawString("OPD: " + opd.dr[0].ToString(), messageFont, Brushes.Black, 20, total + 2);
                g.DrawString("Doctor: " + opd.dr[8].ToString(), messageFont, Brushes.Black, 55, total + 2);
                g.DrawString("Symptoms: " + opd.dr[6].ToString(), messageFont, Brushes.Black, 105, total + 2);
                g.DrawString("Visit Date: " + opd.dr[1].ToString().Substring(0, 4) + "-" + opd.dr[1].ToString().Substring(4, 2) + "-" + opd.dr[1].ToString().Substring(6, 2), messageFont, Brushes.Black, 20, total + 10);
                g.DrawString("Remarks: " + opd.dr[7].ToString(), messageFont, Brushes.Black, 70, total + 10);
                medicine_idHistory = opd.dr[2].ToString().Split(',');
                medicinesHistory = opd.dr[3].ToString().Split(',');
                dosageHistory = opd.dr[4].ToString().Split(',');
                quantityHistory = opd.dr[5].ToString().Split(',');
                total += 20;
                for (int i = 0; i < medicine_idHistory.Length; i++)
                {
                    g.DrawString((i + 1) + ".", messageFont, Brushes.Black, 20, total);
                    g.DrawString(medicinesHistory[i] + " -- ( " + quantityHistory[i] + " ) -- ( " + dosageHistory[i] + " )", messageFont, Brushes.Black, 27, total);
                    total += 6;
                }
                if (sno < opdCount)
                {
                    messageFont = new Font("Times New Roman", 8, FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                    g.DrawString("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ", messageFont, Brushes.Black, 10, total - 1);
                    total += 3;
                }
                sno++;
            }
            db.dbclose();
            messageFont = new Font("Times New Roman", 15, System.Drawing.GraphicsUnit.Point);
            g.DrawString("----------------------------------------------------------------------------------------------------------", messageFont, Brushes.Black, 7, total);
            messageFont = new Font("Times New Roman", 8, System.Drawing.GraphicsUnit.Point);
            g.DrawString("© IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 87, total + 5);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox19.Visible = true;
            }
            else
            {
                textBox19.Visible = false;
            }
        }  
    }
}
