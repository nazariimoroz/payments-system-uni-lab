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
using System.Windows.Navigation;
using System.Windows.Shapes;
using payments_system_uni_lab.Users;
using payments_system_uni_lab.Users.Creators;

namespace payments_system_uni_lab.UI
{
    /// <summary>
    /// Interaction logic for ClientRegistrationMenu.xaml
    /// </summary>
    public partial class ClientRegistrationMenu : UI.BaseRegistrationMenu
    {
        public ClientRegistrationMenu()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ClickBackButtonEvent(this, EventArgs.Empty);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var phoneNumber = ClientPhoneNumberTextBox.Text;
            var realPassword = ClientPasswordBox.Password;
            var password = Utilities.Utilities.CreateMD5(ClientPasswordBox.Password);

            var creator = new ClientCreator();

            BaseUserArgs args = new ClientArgs()
            {
                PhoneNumber = phoneNumber,
                RealPassword = realPassword
            };
            if (!creator.CanBeRegistered(args))
            {
                Console.WriteLine("User cannot be registered");
                return;
            }

            var client = creator.CreateNew(args);
            if (client != null)
            {
                UserLoggedEvent(this, client);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var phoneNumber = ClientPhoneNumberTextBox.Text;
            var realPassword = ClientPasswordBox.Password;

            var creator = new ClientCreator();

            BaseUserArgs args = new ClientArgs()
            {
                PhoneNumber = phoneNumber,
                RealPassword = realPassword
            };
            if (!creator.IsValidArgs(args))
            {
                Console.WriteLine("Invalid args");
                return;
            }

            var client = creator.TryGetFromDb(args);
            if (client != null)
            {
                UserLoggedEvent(this, client);
            }
        }
    }
}
