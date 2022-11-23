using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ChadProgram
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            SQLDataLayer dl = new SQLDataLayer();
            bool loginWorked = dl.Login(txtUsername.Text, txtPassword.Text);
            if (loginWorked)
            {
                ChatWindow chat = new ChatWindow(txtUsername.Text);
                chat.Show();
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
            else
            {
                MessageBox.Show("Login didn't work nerd");
            }

        }
    }
}
