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
namespace ChadProgram
{
    public partial class Jobs : Form
    {
        SqlConnection conn;
        public Jobs()
        {
            InitializeComponent();
            this.conn = conn;
            //SQLDataLayer dl = new SQLDataLayer();
            //dataGridView1.DataSource = dl.GetJobs();
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }

        private void btnJobsList_Click_1(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            SQLDataLayer dl = new SQLDataLayer();
            dataGridView1.DataSource = dl.GetJobs();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void btnAppList_Click_1(object sender, EventArgs e)
        {

            dataGridView1.Columns.Clear();
            SQLDataLayer dl = new SQLDataLayer();
            dataGridView1.DataSource = dl.GetApps();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnPost_Click_1(object sender, EventArgs e)
        {
            JobPostForm jobPostForm = new JobPostForm();
            jobPostForm.Show();
        }

        private void btnApply_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 1 && dataGridView1.Columns.Count > 1) //dgv has extra row by default
            {
                JobApplication app = new JobApplication(dataGridView1.CurrentRow.Cells[0].Value.ToString(), dataGridView1.CurrentRow.Cells[4].Value.ToString());
                app.Show();
            }

        }
    }
}
