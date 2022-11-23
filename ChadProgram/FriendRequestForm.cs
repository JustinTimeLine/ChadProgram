﻿using System;
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
    public partial class FriendRequestForm : Form
    {
        SQLDataLayer dl = new SQLDataLayer();
        public FriendRequestForm()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            bool worked;
            worked = dl.FriendRequest(ChatWindow.Username, txtName.Text); //user1 is always making the request
            if (worked)
            {
                MessageBox.Show("Success!");
                this.DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("Failed to add friend");
        }
    }
}
