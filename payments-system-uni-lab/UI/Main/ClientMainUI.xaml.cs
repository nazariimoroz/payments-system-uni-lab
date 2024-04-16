using System.Windows;
using payments_system_uni_lab.Users;
using payments_system_uni_lab.Users.Creators;

namespace payments_system_uni_lab.UI.Main
{
    /// <summary>
    /// Interaction logic for ClientMainUI.xaml
    /// </summary>
    public partial class ClientMainUI : UserMainUI
    {
        public override BaseUser User
        {
            get => _user;
            set
            {
                if(!(value is Client client))
                {
                    throw new BadUserException("User should be derived from client");
                }
                _user = value;

                ClientPhone.Content = client.PhoneNumber;
                CreditCardNumber.Text = client.CreditCards[0].Num;
            }
        }
        
        public ClientMainUI()
        {
            InitializeComponent();

            SettingsCanvas.Visibility = Visibility.Collapsed;
        }

        private BaseUser _user;

        private void ClientPhone_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingsCanvas.Visibility = SettingsCanvas.Visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void NewPhoneNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(User is Client client)) return;

            var clientCreator = new ClientCreator();

            var clientArgs = new ClientArgs
            {
                PhoneNumber = NewPhoneNumberTextBox.Text
            };
            if(clientCreator.IsValidArgs(clientArgs))
            {
                client.PhoneNumber = NewPhoneNumberTextBox.Text;

                if (Utilities.Utilities.SaveToDb(client))
                {
                    SettingsCanvas.Visibility = Visibility.Collapsed;
                    User = client;
                }
            }
        }
    }
}
