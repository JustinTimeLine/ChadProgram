namespace ChadProgram
{
    partial class Jobs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAppList = new System.Windows.Forms.Button();
            this.btnJobsList = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnPost = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAppList
            // 
            this.btnAppList.Location = new System.Drawing.Point(190, 11);
            this.btnAppList.Name = "btnAppList";
            this.btnAppList.Size = new System.Drawing.Size(167, 23);
            this.btnAppList.TabIndex = 9;
            this.btnAppList.Text = "Your Applicants";
            this.btnAppList.UseVisualStyleBackColor = true;
            this.btnAppList.Click += new System.EventHandler(this.btnAppList_Click_1);
            // 
            // btnJobsList
            // 
            this.btnJobsList.Location = new System.Drawing.Point(12, 11);
            this.btnJobsList.Name = "btnJobsList";
            this.btnJobsList.Size = new System.Drawing.Size(172, 23);
            this.btnJobsList.TabIndex = 8;
            this.btnJobsList.Text = "Jobs List";
            this.btnJobsList.UseVisualStyleBackColor = true;
            this.btnJobsList.Click += new System.EventHandler(this.btnJobsList_Click_1);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(185, 473);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(167, 23);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Apply for Job";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click_1);
            // 
            // btnPost
            // 
            this.btnPost.Location = new System.Drawing.Point(12, 473);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(167, 23);
            this.btnPost.TabIndex = 6;
            this.btnPost.Text = "Post Job";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click_1);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 40);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(722, 427);
            this.dataGridView1.TabIndex = 5;
            // 
            // Jobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 519);
            this.Controls.Add(this.btnAppList);
            this.Controls.Add(this.btnJobsList);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Jobs";
            this.Text = "Jobs";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnAppList;
        private Button btnJobsList;
        private Button btnApply;
        private Button btnPost;
        private DataGridView dataGridView1;
    }
}