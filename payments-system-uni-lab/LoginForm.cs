using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace payments_system_uni_lab
{
    public enum LoginType
    {
        Client, Admin, LAST
    }
    
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        
        private void loginSwitcherButton_Click(object sender, EventArgs e)
        {
            _loginType = _loginType + 1;
            if (_loginType == LoginType.LAST) 
                _loginType = 0;

            var button = sender as Button;
            if (button == null)
                throw new InvalidCastException();
            button.Text = _loginType.ToString();
        }
        
        private void loginButton_Click(object sender, EventArgs e)
        {
            var username = loginUsernameTextBox.Text;
            var password = loginPasswordTextBox.Text;
            // password = Utilities.CreateMD5(password); todo after database connection

            bool userExists = false;
            
            switch (_loginType)
            {
                case LoginType.Client:
                    // todo here will be check in users database
                    if (username == "TestClient" && password == "TestPassword")
                        userExists = true;
                    break;
                
                case LoginType.Admin:
                    // todo here will be check in admin database
                    if (username == "TestAdmin" && password == "TestAdminPassword")
                        userExists = true;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Console.WriteLine(userExists);
        }

        private LoginType _loginType = LoginType.Client;
    }
}