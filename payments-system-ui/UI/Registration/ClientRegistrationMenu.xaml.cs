using System;
using System.Windows;
using payments_system_lib.Classes.Users.Creators;
using payments_system_lib.Utilities;

namespace payments_system_ui.UI.Registration
{
    /// <summary>
    /// Interaction logic for ClientRegistrationMenu.xaml
    /// </summary>
    public partial class ClientRegistrationMenu : BaseRegistrationMenu
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
            var password = Utilities.CreateMD5(ClientPasswordBox.Password);

            var creator = new ClientCreator()
            {
                PhoneNumber = phoneNumber,
                RealPassword = realPassword
            };

            if (!creator.CanBeRegistered())
            {
                Console.WriteLine("User cannot be registered");
                return;
            }

            var client = creator.CreateNew();
            if (client != null)
            {
                UserLoggedEvent(this, client);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var phoneNumber = ClientPhoneNumberTextBox.Text;
            var realPassword = ClientPasswordBox.Password;

            var creator = new ClientCreator()
            {
                PhoneNumber = phoneNumber,
                RealPassword = realPassword
            };

            if (!creator.IsValidArgs())
            {
                Console.WriteLine("Invalid args");
                return;
            }

            var client = creator.TryGetFromDb();
            if (client != null)
            {
                UserLoggedEvent(this, client);
            }
        }
    }
}
