using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace ChadProgram
{
    public partial class JobPostForm : Form
    {
        SqlConnection conn;

        public JobPostForm()
        {
            InitializeComponent();
            this.conn = conn;

        }


        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            if (double.TryParse(txtWage.Text, out double wage) && double.TryParse(txtHours.Text, out double hours) && txtDescription.Text != "" && txtTitle.Text != "")
            {
                SQLDataLayer dl = new SQLDataLayer();
                bool w = dl.SubmitJob(txtTitle.Text, txtDescription.Text, wage, hours);
                txtTitle.Clear();
                txtDescription.Clear();
                txtHours.Clear();
                txtWage.Clear();
                if (w)
                    MessageBox.Show("Sucessfully posted");
                else
                    MessageBox.Show("Job posting failed (does it already exist?)");
            }
            else
                MessageBox.Show("Please enter valid values in all fields");


        }
    }
}
