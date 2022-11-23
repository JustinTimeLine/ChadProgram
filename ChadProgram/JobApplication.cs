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
            if (txtEmail.Text != "" && txtFirstName.Text != "" && txtLastName.Text != "" && txtPhone.Text != "")
            {
                if (int.TryParse(txtPhone.Text, out int id))
                {
                    dl.SubmitApp(Title, Poster, txtFirstName.Text, txtLastName.Text, id, txtEmail.Text);
                    txtFirstName.Clear();
                    txtLastName.Clear();
                    txtPhone.Clear();
                    txtEmail.Clear();
                }
                else
                    MessageBox.Show("Enter only numeric characters in the phone number field");

            }
            else
                MessageBox.Show("Please fill all fields");
        }
    }
}
