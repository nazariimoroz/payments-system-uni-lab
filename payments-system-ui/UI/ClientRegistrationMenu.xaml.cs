using System;
using System.Windows;
using payments_system_ui.Users.Creators;

namespace payments_system_ui.UI
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
