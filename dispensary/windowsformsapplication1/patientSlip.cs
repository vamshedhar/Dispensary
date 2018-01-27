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
    public partial class patientSlip : UserControl
    {
        public string[] medicine_id = new string[0];
        public string[] medicines = new string[0];
        public string[] dosage = new string[0];
        public string[] quantity = new string[0];
        public bool LP = false;
        public string name;
        public string gender;
        public string date;
        public string symptoms;
        public string remarks;
        public string id;
        public string age;
        public string doctor;
        public patientSlip()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                opd opd = new opd();
                dbconnect db = new dbconnect();
                opd.duplicatePatientSlip(textBox4.Text, db);
                if (opd.dr.Read())
                {
                    //MessageBox.Show(opd.dr[9].ToString());
                    opd.patientAge(db.dr[0].ToString());
                    medicine_id = opd.dr[9].ToString().Split(',');
                    medicines = opd.dr[10].ToString().Split(',');
                    dosage = opd.dr[11].ToString().Split(',');
                    quantity = opd.dr[12].ToString().Split(',');
                    id = db.dr[0].ToString();
                    symptoms = db.dr[1].ToString();
                    remarks = db.dr[2].ToString();
                    doctor = db.dr[3].ToString();
                    date = db.dr[4].ToString().Substring(6, 2) + "-" + db.dr[4].ToString().Substring(4, 2) + "-" + db.dr[4].ToString().Substring(0, 4);
                    name = db.dr[5].ToString();
                    gender = db.dr[7].ToString();
                    age = opd.age.ToString();
                    for (int i = 0; i < medicine_id.Length; i++)
                    {
                        if (medicine_id[i] == "LP")
                        {
                            LP = true;
                            break;
                        }
                    }
                    if (MessageBox.Show("Are you sure you want to Print Duplicate Patient Slip??", "Duplicat Patient Slip??", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                       printDocument1.Print();
                      //  printPreviewDialog1.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid OPD!!");
                }
                db.dbclose();
            }
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            int extra = 0;
            if (LP)
            {
                extra += 21;
            }
            if (symptoms != "")
            {
                extra += 35;
            }
            if (remarks != "")
            {
                extra += 35;
            }
            int height = Convert.ToInt32(245 + extra + (35 * (medicine_id.Length)));
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
                g.DrawString("Name : " + name, messageFont, Brushes.Black, 10, 100);
                g.DrawString("Age : " + age + " Years", messageFont, Brushes.Black, 10, 130);
                g.DrawString("Gender : " + gender, messageFont, Brushes.Black, 280, 130);
                g.DrawString("Booklet No. : " + id, messageFont, Brushes.Black, 10, 160);
                g.DrawString("OPD No. : " + textBox4.Text, messageFont, Brushes.Black, 250, 160);
                g.DrawString(doctor, messageFont, Brushes.Black, 10, 190);
                g.DrawString("Date : " + date, messageFont, Brushes.Black, 220, 190);
                int height = 220;
                StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                if (symptoms != "")
                {
                    Rectangle rect = new Rectangle(new Point(10, height), new Size(350, 50));
                    g.DrawString("Symptoms : " + symptoms, messageFont, Brushes.Black, rect, format);
                    height += 50;
                }
                if (remarks != "")
                {
                    Rectangle rect2 = new Rectangle(new Point(10, height), new Size(350, 50));
                    g.DrawString("Remarks : " + remarks, messageFont, Brushes.Black, rect2, format);
                    height += 50;
                }
                g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height);
                g.DrawString("SNo.", messageFont, Brushes.Black, 0, height +20);
                g.DrawString("Medicine Name", messageFont, Brushes.Black, 60, height + 20);
                g.DrawString("Qty", messageFont, Brushes.Black, 360, height +30);
                g.DrawString("(Dosage)", messageFont, Brushes.Black, 80, height +45);
                g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 0, height + 60);
                height += 80;
                int j = 1;
                for (int i = 0; i < medicine_id.Length; i++)
                {
                    if (medicine_id[i].Trim() != "LP")
                    {
                        g.DrawString((j).ToString(), messageFont, Brushes.Black, 0, height);
                        Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                        g.DrawString(medicines[i].Trim() + " -- (" + dosage[i].Trim() + ")", messageFont, Brushes.Black, rect3, format);
                        g.DrawString(quantity[i].Trim(), messageFont, Brushes.Black, 360, height);
                        height += 50;
                        j++;
                    }
                }
                if (LP)
                {
                    g.DrawString("-----------------------------(OWN LP)-----------------------", messageFont, Brushes.Black, 0, height);
                    height += 30;
                }
                for (int i = 0; i < medicine_id.Length; i++)
                {
                    if (medicine_id[i].Trim() == "LP")
                    {
                        g.DrawString((j).ToString(), messageFont, Brushes.Black, 0, height);
                        Rectangle rect3 = new Rectangle(new Point(50, height), new Size(310, 50));
                        g.DrawString(medicines[i].Trim() + " -- (" + dosage[i].Trim() + ")", messageFont, Brushes.Black, rect3, format);
                        g.DrawString(quantity[i].Trim(), messageFont, Brushes.Black, 360, height);
                        height += 50;
                        j++;
                    }
                }
                g.DrawString("---------------------------------------------------------------", messageFont, Brushes.Black, 12, height);
                g.DrawString("IMG Labs, IIT Roorkee, SRE", messageFont, Brushes.Black, 100, height + 20);
        }
    }
}
