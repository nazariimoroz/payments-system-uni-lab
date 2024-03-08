using System.ComponentModel;

namespace payments_system_uni_lab
{
    partial class UserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.testUserName = new System.Windows.Forms.Label();
            this.testPassword = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // testUserName
            // 
            this.testUserName.Location = new System.Drawing.Point(132, 97);
            this.testUserName.Name = "testUserName";
            this.testUserName.Size = new System.Drawing.Size(95, 36);
            this.testUserName.TabIndex = 0;
            this.testUserName.Text = "label1";
            // 
            // testPassword
            // 
            this.testPassword.Location = new System.Drawing.Point(130, 158);
            this.testPassword.Name = "testPassword";
            this.testPassword.Size = new System.Drawing.Size(96, 42);
            this.testPassword.TabIndex = 1;
            this.testPassword.Text = "label2";
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.testPassword);
            this.Controls.Add(this.testUserName);
            this.Name = "UserForm";
            this.Text = "UserForm";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label testUserName;
        private System.Windows.Forms.Label testPassword;

        #endregion
    }
}