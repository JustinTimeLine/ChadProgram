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
        //int friendsIndex = -1;
        public FriendsForm()
        {
            InitializeComponent();
            
            lstFriends.DataSource = dl.GetCurrentFriends();
            lstRequests.DataSource = dl.GetFriendsRequests();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
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
            //lstFriends.SelectedIndex = friendsIndex;
            btnAccept.Visible = false;
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            FriendRequestForm frm = new FriendRequestForm();
            frm.ShowDialog();

            lstFriends.DataSource = dl.GetCurrentFriends();
            lstRequests.DataSource = dl.GetFriendsRequests();
        }

        private void lstFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            //friendsIndex = lstFriends.SelectedIndex;
        }

        private void lstFriends_DoubleClick(object sender, EventArgs e)
        {
            DirectMessage dm = new DirectMessage(lstFriends.SelectedValue.ToString());
            //DirectMessage dm = new DirectMessage(dgvUsers.CurrentRow.Cells[0].Value.ToString());
            dm.ShowDialog();
            lstFriends.DataSource = dl.GetCurrentFriends();
            lstRequests.DataSource = dl.GetFriendsRequests();
        }
    }
}
