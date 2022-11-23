using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;

namespace ChadProgram
{

    public class SQLDataLayer
    {
        string connectionString;
        //this constructor takes a string
        //but also has a default value
        //which means we should be able to call the constructor without parameters
        //if no param it will read from the app.config
        public SQLDataLayer(string connString = "")
        {
            //if connstring is blank
            if (connString == "") //connectionstring from config
                connectionString = ConfigurationManager.ConnectionStrings["localconnection"].ConnectionString;
            else //otherwise whatever was passed in
                connectionString = connString;
        }

        //execute nonquery
        private bool ExecuteNonQuery(string qry)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            bool ret = true;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(qry, conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                //query didn't work
                ret = false;
            }
            finally
            {
                //close connection
                conn.Close();
            }
            return ret;
        }

        //execute scalar
        private object ExecuteScalar(string qry)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            object ret = null;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(qry, conn);
                ret = cmd.ExecuteScalar();
            }
            catch
            {
                //query didn't work
                ret = null;
            }
            finally
            {
                conn.Close();
            }
            return ret;
        }

        private object ExecuteDataReader(string qry)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(qry, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    object tmp = reader[0];
                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            //REMOVE THIS
            return null;
        }

        public List<string> GetChatMessages()
        {
            List<string> messages = new List<string>();

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from chat order by message_date", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //gets the current message from the db because we are reading line by line
                    string currentMessage = reader[0] + ": " + reader[2];
                    messages.Add(currentMessage);
                    //object tmp = reader[0];
                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            //ExecuteDataReader("select * from chat");

            return messages;
        }

        public List<string> GetChatUsers()
        {
            List<string> messages = new List<string>();

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select username from users", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //gets the current message from the db because we are reading line by line
                    string currentUser = reader[0].ToString();
                    messages.Add(currentUser);
                    //object tmp = reader[0];
                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            //ExecuteDataReader("select * from chat");

            return messages;
        }

        public bool RegisterUser(string username, string password)
        {
            return this.ExecuteNonQuery(@"insert into users(username, password, register_date) " +
                "values ('" + username + "','" + password + "',getdate())");
        }

        public bool FirstName(string username, string firstname)
        {
            return this.ExecuteNonQuery($"update users set first_name = '{firstname}' where username = '{username}'");
        }
        public bool LastName(string username, string lastname)
        {
            return this.ExecuteNonQuery($"update users set last_name = '{lastname}' where username = '{username}'");
        }

        public bool Login(string username, string password)
        {
            int count = (int)ExecuteScalar($"select count(*) from users where username = '{username}' and password = '{password}' ");

            //if count = 1 we know the user exists
            return count == 1;
        }

        public bool SendMessage(string message)
        {
            bool ret = true;

            ret = ExecuteNonQuery($"update users set last_message = getdate() where username = '{ChatWindow.Username}'");

            string qry = $"insert into chat values('{ChatWindow.Username}',getdate(),'{message}')";

            if (ret)
                ret = ExecuteNonQuery(qry);
            return ret;
        }

        public bool SendDirectMessage(string recipient, string message)
        {
            bool ret = true;

            string qry = $"insert into DirectMessage values ('{ChatWindow.Username}','{recipient}',getdate(),'{message}', '0')";
            ret = ExecuteNonQuery(qry);


            return ret;
        }

        //public void createDatabase()
        //{

        //    string Database = "create database chatdb;";
        //    ExecuteNonQuery(Database);
        //}

        public void SetUpDatabase()
        {

            string createTables = "USE [chatdb]\r\n\r\n/****** Object:  Table [dbo].[Chat]    Script Date: 11/22/2022 5:44:32 PM ******/\r\nSET ANSI_NULLS ON\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\nCREATE TABLE [dbo].[Chat](\r\n\t[username] [varchar](30) NOT NULL,\r\n\t[message_date] [datetime] NOT NULL,\r\n\t[message] [varchar](150) NULL,\r\nPRIMARY KEY CLUSTERED \r\n(\r\n\t[username] ASC,\r\n\t[message_date] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n\r\n/****** Object:  Table [dbo].[DirectMessage]    Script Date: 11/22/2022 5:44:32 PM ******/\r\nSET ANSI_NULLS ON\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\nCREATE TABLE [dbo].[DirectMessage](\r\n\t[Sender] [varchar](30) NOT NULL,\r\n\t[Receiver] [varchar](30) NOT NULL,\r\n\t[Message_Date] [datetime] NOT NULL,\r\n\t[Message_Content] [varchar](255) NULL,\r\n\t[Message_Read] [bit] NOT NULL,\r\n CONSTRAINT [PK__DirectMe__7315E33226B8FFAC] PRIMARY KEY CLUSTERED \r\n(\r\n\t[Sender] ASC,\r\n\t[Receiver] ASC,\r\n\t[Message_Date] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n\r\n/****** Object:  Table [dbo].[Friends]    Script Date: 11/22/2022 5:44:32 PM ******/\r\nSET ANSI_NULLS ON\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\nCREATE TABLE [dbo].[Friends](\r\n\t[user1] [varchar](30) NOT NULL,\r\n\t[user2] [varchar](30) NOT NULL,\r\n\t[request_accepted] [bit] NOT NULL,\r\n CONSTRAINT [PK_Friends] PRIMARY KEY CLUSTERED \r\n(\r\n\t[user1] ASC,\r\n\t[user2] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n\r\n/****** Object:  Table [dbo].[GroupChat]    Script Date: 11/22/2022 5:44:32 PM ******/\r\nSET ANSI_NULLS ON\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\nCREATE TABLE [dbo].[GroupChat](\r\n\t[group_name] [varchar](30) NOT NULL,\r\n\t[username] [varchar](30) NOT NULL,\r\n\t[message_date] [datetime] NOT NULL,\r\n\t[message] [varchar](150) NULL,\r\nPRIMARY KEY CLUSTERED \r\n(\r\n\t[username] ASC,\r\n\t[message_date] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n\r\n/****** Object:  Table [dbo].[Groups]    Script Date: 11/22/2022 5:44:32 PM ******/\r\nSET ANSI_NULLS ON\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\nCREATE TABLE [dbo].[Groups](\r\n\t[group_name] [varchar](30) NOT NULL,\r\n CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED \r\n(\r\n\t[group_name] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n\r\n/****** Object:  Table [dbo].[GroupUsers]    Script Date: 11/22/2022 5:44:32 PM ******/\r\nSET ANSI_NULLS ON\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\nCREATE TABLE [dbo].[GroupUsers](\r\n\t[username] [varchar](30) NOT NULL,\r\n\t[group_name] [varchar](30) NOT NULL\r\n) ON [PRIMARY]\r\n\r\n/****** Object:  Table [dbo].[Users]    Script Date: 11/22/2022 5:44:32 PM ******/\r\nSET ANSI_NULLS ON\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\nCREATE TABLE [dbo].[Users](\r\n\t[username] [varchar](30) NOT NULL,\r\n\t[password] [varchar](30) NOT NULL,\r\n\t[register_date] [datetime] NOT NULL,\r\n\t[first_name] [varchar](30) NULL,\r\n\t[last_name] [varchar](30) NULL,\r\n\t[last_message] [datetime] NULL,\r\n\t[last_updated] [datetime] NULL,\r\n CONSTRAINT [PK__Users__F3DBC573E2595F1D] PRIMARY KEY CLUSTERED \r\n(\r\n\t[username] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n\r\nALTER TABLE [dbo].[Chat]  WITH CHECK ADD  CONSTRAINT [FK__Chat__username__267ABA7A] FOREIGN KEY([username])\r\nREFERENCES [dbo].[Users] ([username])\r\n\r\nALTER TABLE [dbo].[Chat] CHECK CONSTRAINT [FK__Chat__username__267ABA7A]\r\n\r\nALTER TABLE [dbo].[DirectMessage]  WITH CHECK ADD  CONSTRAINT [FK__DirectMes__Recei__70DDC3D8] FOREIGN KEY([Receiver])\r\nREFERENCES [dbo].[Users] ([username])\r\n\r\nALTER TABLE [dbo].[DirectMessage] CHECK CONSTRAINT [FK__DirectMes__Recei__70DDC3D8]\r\n\r\nALTER TABLE [dbo].[DirectMessage]  WITH CHECK ADD  CONSTRAINT [FK__DirectMes__Sende__6FE99F9F] FOREIGN KEY([Sender])\r\nREFERENCES [dbo].[Users] ([username])\r\n\r\nALTER TABLE [dbo].[DirectMessage] CHECK CONSTRAINT [FK__DirectMes__Sende__6FE99F9F]\r\n\r\nALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Users] FOREIGN KEY([user1])\r\nREFERENCES [dbo].[Users] ([username])\r\n\r\nALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Users]\r\n\r\nALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Users1] FOREIGN KEY([user2])\r\nREFERENCES [dbo].[Users] ([username])\r\n\r\nALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Users1]\r\n\r\nALTER TABLE [dbo].[GroupChat]  WITH CHECK ADD  CONSTRAINT [FK_GroupChat_Groups] FOREIGN KEY([group_name])\r\nREFERENCES [dbo].[Groups] ([group_name])\r\n\r\nALTER TABLE [dbo].[GroupChat] CHECK CONSTRAINT [FK_GroupChat_Groups]\r\n\r\nALTER TABLE [dbo].[GroupUsers]  WITH CHECK ADD  CONSTRAINT [FK__GroupUser__usern__1BC821DD] FOREIGN KEY([username])\r\nREFERENCES [dbo].[Users] ([username])\r\n\r\nALTER TABLE [dbo].[GroupUsers] CHECK CONSTRAINT [FK__GroupUser__usern__1BC821DD]\r\n\r\nALTER TABLE [dbo].[GroupUsers]  WITH CHECK ADD  CONSTRAINT [FK_GroupUsers_Groups] FOREIGN KEY([group_name])\r\nREFERENCES [dbo].[Groups] ([group_name])\r\n\r\nALTER TABLE [dbo].[GroupUsers] CHECK CONSTRAINT [FK_GroupUsers_Groups]";
            
            //string createUser = "CREATE TABLE [dbo].[Users](\r\n    [username] [varchar](30) NOT NULL,\r\n    [password] [varchar](30) NOT NULL,\r\n    [register_date] [datetime] NOT NULL,\r\n    [first_name] [varchar](30) NULL,\r\n    [last_name] [varchar](30) NULL,\r\n    [last_message] [datetime] NULL,\r\n    [last_updated] [datetime] NULL,\r\n ) ";
           //string createChat = "create table Chat(username varchar(30), message_date datetime primary key(username,message_date), [message] varchar(150), foreign key (username) references users(username))";

//            string createDirectMessages = @"create table DirectMessage (
//Sender varchar(30),
//Receiver varchar(30),
//Message_Date datetime,
//Message_Content varchar(255)
//primary key(Sender, receiver,message_date),
//foreign key (sender) references Users(Username),
//foreign key (receiver) references Users(Username)
//)";
            //string createGroups = "create table Groups(GroupID int not null identity(1,1), GroupName varchar(20) not null, primary key(GroupID));";
            //string createGroupUsers = "create table GroupUsers(\r\nGroupID int not null, \r\nusername varchar(30) not null, \r\nprimary key(GroupID, username), \r\nforeign key (username) references users(username), \r\nforeign key (groupid) references groups(groupid))";
            //string createGroupChat = "create table GroupChat(\r\ngroup_name varchar(30) not null,\r\nusername varchar(30) not null, \r\nmessage_date datetime not null,\r\nmessage varchar(150),\r\nprimary key(username, message_date))";

            string createApp = "CREATE TABLE [dbo].[Applications](\n\t[FirstName] [varchar](50) NULL,\n\t[LastName] [varchar](50) NULL,\n\t[Phone] [nchar](10) NULL,\n\t[Email] [varchar](50) NULL)";
            string createJobApps = "CREATE TABLE [dbo].[JobApplications](\n\t[Job_Title] [varchar](50) NULL,\n\t[Poster] [varchar](50) NULL,\n\t[First_Name] [varchar](50) NULL,\n\t[Last_Name] [varchar](50) NULL,\n\t[Phone] [int] NULL,\n\t[E-Mail] [varchar](50) NULL\n)";
            string createJobs = "CREATE TABLE [dbo].[Jobs](\n\t[Job_Title] [varchar](50) NULL,\n\t[Description] [varchar](max) NULL,\n\t[Wage] [nchar](10) NULL,\n\t[Weekly_Hours] [nchar](10) NULL,\n\t[Poster] [varchar](50) NULL\n)";

            ExecuteNonQuery(createTables);

            //ExecuteNonQuery(createUser);
            //ExecuteNonQuery(createChat);
            //ExecuteNonQuery(createDirectMessages);
            //ExecuteNonQuery(createGroups);
            //ExecuteNonQuery(createGroupUsers);
            //ExecuteNonQuery(createGroupChat);
            ExecuteNonQuery(createJobApps);
            ExecuteNonQuery(createJobs);
            ExecuteNonQuery(createApp);
            // ExecuteNonQuery("CREATE TABLE Friends(\r\n\t[user1] [varchar](30) NOT NULL,\r\n\t[user2] [varchar](30) NOT NULL)")
        }

        public List<string> GetDirectChatMessages(string them)
        {
            List<string> messages = new List<string>();

            //            string qry = $@"select * from DirectMessage where (sender = '@username' and receiver = '@them') or (Sender = '@them' and receiver = '@username') 
            //order by Message_Date asc";


            //            SqlConnection conn = new SqlConnection(connectionString);
            //            try
            //            {
            //                conn.Open();
            //                SqlCommand cmd = new SqlCommand(qry, conn);
            //                cmd.Parameters.AddWithValue(@"them", them);
            //                cmd.Parameters.AddWithValue(@"username", ChatWindow.Username);
            //                SqlDataReader reader = cmd.ExecuteReader();
            //                while (reader.Read())
            //                {
            //                    //gets the current message from the db because we are reading line by line
            //                    string currentMessage = reader[0] + ":" + reader[3];
            //                    messages.Add(currentMessage);
            //                    //object tmp = reader[0];
            //                }
            //            }
            //            catch
            //            {

            //            }
            //            finally
            //            {
            //                conn.Close();
            //            }
            //            //ExecuteDataReader("select * from chat");

            //            return messages;

            string qry = $@"select * from DirectMessage
             Where (sender = '{ChatWindow.Username}'
             and receiver = '{them}')
             or
             (Sender = '{them}'
             and receiver = '{ChatWindow.Username}')
             Order by Message_Date asc";

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(qry, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //gets current message from the db, because we are reading line by line
                    string currentMessage = reader[0] + " : " + reader[3];
                    messages.Add(currentMessage);
                    //object tmp = reader[0]
                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            //
            return messages;


            ExecuteDataReader("Select * from chat");

            return messages;



        }


        // TO DO
        //GROUP STUFF
        public List<string> GetGroups(string user)
        {
            List<string> messages = new List<string>();

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select group_name from groupusers where username = '{user}", conn);
                //cmd.Parameters.AddWithValue("@user", user);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //gets the current message from the db because we are reading line by line
                    string currentUser = reader[0].ToString();
                    messages.Add(currentUser);
                    //object tmp = reader[0];
                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            //ExecuteDataReader("select * from chat");

            return messages;
        }

        public List<string> GetGroupChatMessages(string group)
        {
            List<string> messages = new List<string>();

            string qry = $@"select * from GroupChat where group_name = '{group}' order by Message_Date asc";


            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(qry, conn);
                //cmd.Parameters.AddWithValue("@group", group);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //gets the current message from the db because we are reading line by line
                    string currentMessage = reader[1] + ":" + reader[3];
                    messages.Add(currentMessage);
                    //object tmp = reader[0];
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return messages;
        }

        public bool SendGroupMessage(string group, string message)
        { //think this should work? no error checking here tho
            bool ret = true;

            string qry = $"use chatdb insert into GroupChat values ('{group}', '{ChatWindow.Username}',getdate(),'{message}')";
            ret = ExecuteNonQuery(qry);


            return ret;
        }

        public bool RegisterGroup(string name)
        {
            return this.ExecuteNonQuery($"use chatdb insert into groups values('{name}')");
        }

        public bool RegisterGroupUser(string groupName, string username)
        {
            return this.ExecuteNonQuery($"insert into groupusers values('{username}','{groupName}')");
        }

        public List<string> GetUserInfo(string username)
        {
            List<string> userInfo = new List<string>();
            //throw user info into a list
            string qry = $@"select first_name, last_name, last_message from users where username = '@username'";



            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@username", username);
                //string firstName = cmd.Parameters[0].ToString();
                //MessageBox.Show(cmd.Parameters[0].ToString());
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //gets the current message from the db because we are reading line by line
                    userInfo.Add(reader[0].ToString());
                    userInfo.Add(reader[1].ToString());
                    userInfo.Add(reader[2].ToString());

                    //object tmp = reader[0];
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return userInfo;
        }

        public bool FriendRequest(string user1, string user2)
        { //sending of request initially
            bool ret;
            SqlConnection conn = new SqlConnection(connectionString);

            int count = (int)ExecuteScalar($"select count(*) from friends where user1 = '{user1}' and user2 = '{user2}'");

            string qry = $"use chatdb insert into Friends values ('{user1}', '{user2}',0)"; //bit 0 is not accepted
            SqlCommand cmd = new SqlCommand(qry, conn);
            //cmd.Parameters.AddWithValue("@user1", user1);
            //cmd.Parameters.AddWithValue("@user2", user2);
            return ret = ExecuteNonQuery(qry);
        }

        public List<string> GetCurrentFriends()
        {
            List<string> friends = new List<string>();

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select user1 from friends where (user1 = '{ChatWindow.Username}' or user2 = '{ChatWindow.Username}') and request_accepted = 1", conn);
                SqlCommand cmd2 = new SqlCommand($"select user2 from friends where (user1 = '{ChatWindow.Username}' or user2 = '{ChatWindow.Username}') and request_accepted = 1", conn);
                //cmd.Parameters.AddWithValue("@user", user);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0].ToString() != ChatWindow.Username)
                        friends.Add(reader[0].ToString());
                }
                reader.Close();
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    if (reader2[0].ToString() != ChatWindow.Username)
                        friends.Add(reader2[0].ToString());
                }
                reader2.Close();
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            //ExecuteDataReader("select * from chat");

            return friends;
        }

        public List<string> GetFriendsRequests()
        {
            List<string> friends = new List<string>();

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"select user1 from friends where (user1 = '{ChatWindow.Username}' or user2 = '{ChatWindow.Username}') and request_accepted = 0", conn);
                SqlCommand cmd2 = new SqlCommand($"select user2 from friends where (user1 = '{ChatWindow.Username}' or user2 = '{ChatWindow.Username}') and request_accepted = 0", conn);
                //cmd.Parameters.AddWithValue("@user", user);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0].ToString() != ChatWindow.Username)
                        friends.Add(reader[0].ToString());
                }
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    if (reader2[0].ToString() != ChatWindow.Username)
                        friends.Add(reader2[0].ToString());
                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            //ExecuteDataReader("select * from chat");

            return friends;
        }

        public bool FriendRequestAccept(string requester)
        {
            //user1 is always making the request

            return ExecuteNonQuery($"update Friends set request_accepted = 1 where user1 = '{ChatWindow.Username}' and user2 = '{requester}' or user1 = '{requester}' and user2 = '{ChatWindow.Username}'");

        }

        public int GetUnreadMessagesCount(string userName)
        {
            int count = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select count (*) from DirectMessage where receiver = @myUser and message_read = 0 " +
                    "and sender = @otherUser", conn);
                cmd.Parameters.AddWithValue("@myUser", ChatWindow.Username);
                cmd.Parameters.AddWithValue("@otherUser", userName);
                count = (int)cmd.ExecuteScalar();
                //while (reader.Read())
                //{
                //    //gets current message from the db, because we are reading line by line
                //    string currentMessage = reader[0] + " : " + reader[2];
                //    //messages.Add(currentMessage);
                //    //object tmp = reader[0]
                //}
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            //
            return count;


            //ExecuteDataReader("Select * from chat");

            //return messages;
        }

        public List<string> GetAllDirectChatMessages()
        {
            List<string> messages = new List<string>();
            //int prevCount=0;
            string qry = $@"select * from DirectMessage
             Order by Message_Date asc";

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(qry, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //gets current message from the db, because we are reading line by line
                    string currentMessage = reader[0] + " : " + reader[3];
                    messages.Add(currentMessage);
                    //object tmp = reader[0]
                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            int Count = messages.Count;
            // if () 
            return messages;


            ExecuteDataReader("Select * from chat");

            return messages;
        }

        public bool UpdateDirectMessageRead(string otherUser)
        {
            bool ret = true;
            string qry = $"UPDATE DirectMessage\r\nSET Message_Read = '1'\r\nWHERE Sender = '{otherUser}' and receiver = '{ChatWindow.Username}'";
            ret = ExecuteNonQuery(qry);
            return ret;
        }

        public DataTable GetJobs()
        {


            SqlConnection conn = new(connectionString);
            DataTable jobs = new DataTable();
            try
            {
                conn.Open();
                SqlCommand cmd = new($"select * from jobs", conn);

                SqlDataAdapter data = new SqlDataAdapter(cmd);
                data.Fill(jobs);
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return jobs;

        }

        public DataTable GetApps()
        {


            SqlConnection conn = new(connectionString);
            DataTable apps = new DataTable();
            string userName = ChatWindow.Username;
            try
            {
                conn.Open();
                SqlCommand cmd = new($"select * from JobApplications where poster = '{userName}'", conn);

                SqlDataAdapter data = new SqlDataAdapter(cmd);
                data.Fill(apps);
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return apps;

        }

        public bool SubmitJob(string title, string description, double wage, double hours)
        {
            bool ret = true;
            string userName = ChatWindow.Username.ToString();
            string qry = $"insert into Jobs values ('{title}', '{description}', '{wage}','{hours}','{userName}')";
            ret = ExecuteNonQuery(qry);

            return ret;
        }

        public bool SubmitApp(string title, string poster, string firstName, string lastName, int phone, string email)
        {
            bool ret = true;
            string qry = $"insert into JobApplications values ('{title}', '{poster}', '{firstName}','{lastName}','{phone}', '{email}')";
            ret = ExecuteNonQuery(qry);

            return ret;
        }

    }
}
