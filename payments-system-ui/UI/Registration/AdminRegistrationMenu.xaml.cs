using payments_system_lib.Classes.Users.Creators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace payments_system_ui.UI.Registration
{
    /// <summary>
    /// Interaction logic for AdminRegistrationMenu.xaml
    /// </summary>
    public partial class AdminRegistrationMenu : BaseRegistrationMenu
    {
        public AdminRegistrationMenu()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ClickBackButtonEvent(this, EventArgs.Empty);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var key = AdminKeyTextBox.Text;
            var realPassword = AdminPasswordBox.Password;

            var creator = new AdminCreator()
            {
                Key = key,
                RealPassword = realPassword
            };

            if (!creator.IsValidArgs())
            {
                Console.WriteLine("Invalid args");
                return;
            }

            var admin = creator.TryGetFromDb();
            if (admin != null)
            {
                UserLoggedEvent(this, admin);
            }
        }
    }
}
