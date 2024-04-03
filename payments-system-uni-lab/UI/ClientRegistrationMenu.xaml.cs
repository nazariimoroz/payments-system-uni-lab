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
            var password = Utilities.CreateMD5(ClientPasswordBox.Password);

            var client = Client.TryGetFromDb(phoneNumber, password);
            if (client != null)
            {
                Console.WriteLine("User is already existed");
            }

            Client.CreateNew(phoneNumber, password);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
