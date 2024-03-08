using System;
using System.Windows.Forms;

namespace payments_system_uni_lab
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();

            var loginForm = new LoginForm();
            loginForm.Closed += LoginFormOnClosed;
            loginForm.ShowDialog(this);
        }

        public void InitFrom(BaseUser user)
        {
            _user = user ?? throw new NullReferenceException();
            
            testUserName.Text = _user.Name;
            testPassword.Text = _user.CachedPassword;
        }

        private void LoginFormOnClosed(object sender, EventArgs e)
        {
            if(_user == null)
                this.Close();
        }
        
        private BaseUser _user = null;
    }
}