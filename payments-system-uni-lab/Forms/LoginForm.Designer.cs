namespace payments_system_uni_lab
{
    partial class LoginForm
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
            this.loginSwitcherButton = new System.Windows.Forms.Button();
            this.loginUsernameTextBox = new System.Windows.Forms.TextBox();
            this.loginPasswordTextBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loginSwitcherButton
            // 
            this.loginSwitcherButton.Location = new System.Drawing.Point(15, 18);
            this.loginSwitcherButton.Name = "loginSwitcherButton";
            this.loginSwitcherButton.Size = new System.Drawing.Size(107, 33);
            this.loginSwitcherButton.TabIndex = 0;
            this.loginSwitcherButton.Text = "Client";
            this.loginSwitcherButton.UseVisualStyleBackColor = true;
            this.loginSwitcherButton.Click += new System.EventHandler(this.loginSwitcherButton_Click);
            // 
            // loginUsernameTextBox
            // 
            this.loginUsernameTextBox.Location = new System.Drawing.Point(304, 173);
            this.loginUsernameTextBox.Name = "loginUsernameTextBox";
            this.loginUsernameTextBox.Size = new System.Drawing.Size(144, 22);
            this.loginUsernameTextBox.TabIndex = 1;
            // 
            // loginPasswordTextBox
            // 
            this.loginPasswordTextBox.Location = new System.Drawing.Point(304, 201);
            this.loginPasswordTextBox.Name = "loginPasswordTextBox";
            this.loginPasswordTextBox.PasswordChar = '*';
            this.loginPasswordTextBox.Size = new System.Drawing.Size(144, 22);
            this.loginPasswordTextBox.TabIndex = 2;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(327, 253);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(98, 35);
            this.loginButton.TabIndex = 3;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.loginPasswordTextBox);
            this.Controls.Add(this.loginUsernameTextBox);
            this.Controls.Add(this.loginSwitcherButton);
            this.Name = "LoginForm";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button loginButton;

        private System.Windows.Forms.TextBox loginUsernameTextBox;
        private System.Windows.Forms.Button loginSwitcherButton;
        private System.Windows.Forms.TextBox loginPasswordTextBox;

        private System.Windows.Forms.TabPage userLoginPage;
        private System.Windows.Forms.TabPage adminLoginPage;

        #endregion
    }
}