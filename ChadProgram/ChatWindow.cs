using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;

namespace ChadProgram
{
    public partial class ChatWindow : Form
    {
        List<string> chatMessages = new List<string>();
        List<string> chatUsers = new List<string>();
        List<string> directMessages = new List<string>();


        public static string? Username;
        //SqlConnection conn;
        public ChatWindow(string username)
        {
            InitializeComponent();
            //this.conn = conn;
            ChatWindow.Username = username;
            lblUsername.Text =$"Logged in as: {username}";

        }
       
        private void btnSend_Click(object sender, EventArgs e)
        {
            SQLDataLayer dl = new SQLDataLayer();
            dl.SendMessage(txtMessage.Text);
            //SendMessage(txtMessage.Text);
            txtMessage.Clear();
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            SQLDataLayer dl = new SQLDataLayer();
            List<string> messages = dl.GetChatMessages();
            if (messages.Count > chatMessages.Count)
            {
            lstMessages.DataSource = messages;
                chatMessages = messages;
                lstMessages.SelectedIndex = lstMessages.Items.Count - 1;
            }
            List<string> users = dl.GetChatUsers();
            if (users.Count > chatUsers.Count)
            {
                chatUsers = users;
                foreach (string user in chatUsers)
                {
                    dgvUsers.Rows.Add(user, dl.GetUnreadMessagesCount(user));
                    //userName.Add(user);
                    //lstUsers.Items.Add($"{user} ({dl.GetUnreadMessagesCount(user)})");

                    //lstUsers.SelectedValue = user;
                }

                //dgvUsers.DataSource = userName;
                //lstUsers.DataSource = users;
                //lstUsers.SelectedIndex = lstUsers.Items.Count - 1;
            }
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            directMessages = dl.GetAllDirectChatMessages();
        
        }


        private void btnNotifications_Click(object sender, EventArgs e)
        {

        }

        private void btnGroups_Click(object sender, EventArgs e)
        {
            //open groupchat form here
            GroupChat gp = new GroupChat();
            gp.Show();
        }

        private void lstUsers_MouseDown(object sender, MouseEventArgs e)
        { //right click
            if (e.Button == MouseButtons.Right)
            {
                FriendsForm f = new FriendsForm();
                f.Show();
            }
        }


        private void UpdateMessages()
        {
            SQLDataLayer dl = new SQLDataLayer();
            chatUsers = dl.GetChatUsers();
            dgvUsers.Rows.Clear();
            foreach (string user in chatUsers)
            {
                dgvUsers.Rows.Add(user, dl.GetUnreadMessagesCount(user));
                //userName.Add(user);
                //lstUsers.Items.Add($"{user} ({dl.GetUnreadMessagesCount(user)})");

                //lstUsers.SelectedValue = user;
            }
        }





        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DirectMessage dm = new DirectMessage(dgvUsers.CurrentRow.Cells[0].Value.ToString());
            //DirectMessage dm = new DirectMessage(dgvUsers.CurrentRow.Cells[0].Value.ToString());
            dm.ShowDialog();
            UpdateMessages();

        }

        private void btnJobs_Click(object sender, EventArgs e)
        {
            Jobs jobs = new Jobs();
            jobs.Show();
        }

        private void txtMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                SQLDataLayer dl = new SQLDataLayer();
                dl.SendMessage(txtMessage.Text);
                //SendMessage(txtMessage.Text);
                txtMessage.Clear();
            }
        }

        private void btnFriends_Click(object sender, EventArgs e)
        {
            FriendsForm f = new FriendsForm();
            f.Show();
        }

    }
}
