using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChadProgram
{
    public partial class JobApplication : Form
    {
        string Title { get; }
        string Poster { get; }
        public JobApplication(string title, string poster)
        {
            InitializeComponent();
            lblTitle.Text = title;
            lblPoster.Text = $"Posted by {poster}";
            Title = title;
            Poster = poster;
        }

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {

            SQLDataLayer dl = new SQLDataLayer();
            dl.SubmitApp(Title, Poster, txtFirstName.Text, txtLastName.Text, int.Parse(txtPhone.Text), txtEmail.Text);
            txtFirstName.Clear();
            txtLastName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
        }
    }
}
