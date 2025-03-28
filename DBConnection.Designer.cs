﻿namespace BilderalbumCS
{
    partial class DBConnection
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtDone = new System.Windows.Forms.Button();
            this.ServerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DBName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.User = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSlideTime = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.btBrowserFont = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtSlideTime)).BeginInit();
            this.SuspendLayout();
            // 
            // BtDone
            // 
            this.BtDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtDone.Location = new System.Drawing.Point(205, 366);
            this.BtDone.Name = "BtDone";
            this.BtDone.Size = new System.Drawing.Size(75, 23);
            this.BtDone.TabIndex = 0;
            this.BtDone.Text = "OK";
            this.BtDone.UseVisualStyleBackColor = true;
            this.BtDone.Click += new System.EventHandler(this.button1_Click);
            // 
            // ServerName
            // 
            this.ServerName.Location = new System.Drawing.Point(12, 33);
            this.ServerName.Multiline = true;
            this.ServerName.Name = "ServerName";
            this.ServerName.Size = new System.Drawing.Size(268, 66);
            this.ServerName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server";
            // 
            // DBName
            // 
            this.DBName.Location = new System.Drawing.Point(12, 118);
            this.DBName.Name = "DBName";
            this.DBName.Size = new System.Drawing.Size(268, 20);
            this.DBName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Database";
            // 
            // User
            // 
            this.User.Location = new System.Drawing.Point(12, 157);
            this.User.Name = "User";
            this.User.Size = new System.Drawing.Size(268, 20);
            this.User.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "User";
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(12, 196);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(268, 20);
            this.Password.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Password";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(124, 366);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtSlideTime
            // 
            this.txtSlideTime.Location = new System.Drawing.Point(12, 294);
            this.txtSlideTime.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.txtSlideTime.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtSlideTime.Name = "txtSlideTime";
            this.txtSlideTime.Size = new System.Drawing.Size(74, 20);
            this.txtSlideTime.TabIndex = 3;
            this.txtSlideTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSlideTime.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 278);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "SlideShowTimer";
            // 
            // fontDialog1
            // 
            this.fontDialog1.FontMustExist = true;
            this.fontDialog1.Apply += new System.EventHandler(this.fontDialog1_Apply);
            // 
            // btBrowserFont
            // 
            this.btBrowserFont.Location = new System.Drawing.Point(11, 240);
            this.btBrowserFont.Name = "btBrowserFont";
            this.btBrowserFont.Size = new System.Drawing.Size(93, 23);
            this.btBrowserFont.TabIndex = 5;
            this.btBrowserFont.Text = "BrowserFont...";
            this.btBrowserFont.UseVisualStyleBackColor = true;
            this.btBrowserFont.Click += new System.EventHandler(this.btBrowserFont_Click);
            // 
            // DBConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 401);
            this.Controls.Add(this.btBrowserFont);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSlideTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.User);
            this.Controls.Add(this.DBName);
            this.Controls.Add(this.ServerName);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtDone);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DBConnection";
            this.Text = "Connection";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtSlideTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtDone;
        private System.Windows.Forms.TextBox ServerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DBName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox User;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown txtSlideTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button btBrowserFont;
    }
}

