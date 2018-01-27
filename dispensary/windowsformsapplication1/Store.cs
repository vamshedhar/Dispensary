using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Store : Form
    {
        string username;
        public Store(string user)
        {
            InitializeComponent();
            username = user;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form returnStock = new returnStock(username);
            returnStock.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form newMed = new newMedicine(username);
            newMed.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form newMed = new newMedicine(username);
            newMed.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void usertab1_Load(object sender, EventArgs e)
        {
            usertab1.user_name = username;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form transfer = new stockTransfer(username);
            transfer.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form receive = new receiveStock(username);
            receive.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form list = new medicineList(username);
            list.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form change = new changePassword(username);
            change.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form indentHistory = new dispIndentHistoryStore(username);
            indentHistory.ShowDialog();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Form dispIndent = new viewDispIndent(username);
            dispIndent.ShowDialog();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            monthCalendar1.TodayDate = DateTime.Today;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form indent = new generateStoreIndent(username);
            indent.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form history = new storeIndentHistory(username);
            history.ShowDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Form view = new viewStoreIndent(username);
            view.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form stockReports = new storeStockReport(username);
            stockReports.ShowDialog();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Form registrationTransfer = new stockTransfer(username);
            registrationTransfer.ShowDialog();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Form emergency = new doctorMain(username);
            emergency.ShowDialog();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Form patientHistory = new patientHistoryReports(username);
            patientHistory.ShowDialog();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Form expired = new storeExpiredMedicines(username);
            expired.ShowDialog();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Form expiredReports = new storeExpiredMedicinesReports(username);
            expiredReports.ShowDialog();
        }
    }
}
