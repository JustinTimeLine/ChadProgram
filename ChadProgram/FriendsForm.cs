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
    public partial class FriendsForm : Form
    {
        SQLDataLayer dl = new SQLDataLayer();
        public FriendsForm()
        {
            InitializeComponent();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lstFriends.DataSource = dl.GetCurrentFriends();
            lstRequests.DataSource = dl.GetFriendsRequests();
            if (lstRequests.SelectedIndex >= 0)
            {             
                btnAccept.Visible = true;
            }
            else
            {
                btnAccept.Visible = false;
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            dl.FriendRequestAccept(lstRequests.SelectedValue.ToString());
            lstFriends.DataSource = dl.GetCurrentFriends();
            lstRequests.DataSource = dl.GetFriendsRequests();
            btnAccept.Visible = false;
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            FriendRequestForm frm = new FriendRequestForm();
            frm.ShowDialog();
        }
    }
}
