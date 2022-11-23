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
    public partial class FriendsForm : Form
    {
        SQLDataLayer dl = new SQLDataLayer();
        public FriendsForm()
        {
            InitializeComponent();
            lstFriends.DataSource = dl.GetCurrentFriends();
        }
    }
}
