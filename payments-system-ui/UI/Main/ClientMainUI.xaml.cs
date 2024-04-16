using System.Text;
using System.Windows;
using payments_system_lib.Classes.Creators;
using payments_system_lib.Classes.Users;
using payments_system_lib.Utilities;

namespace payments_system_ui.UI.Main
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
                CreditCardCvc.Text = client.CreditCards[0].Cvc.ToString();

                var expiresEndText = new StringBuilder(5);
                if (client.CreditCards[0].ExpiresEnd.Month < 10)
                {
                    expiresEndText.Append(0);
                }
                expiresEndText.Append(client.CreditCards[0].ExpiresEnd.Month);
                expiresEndText.Append("/");
                if (client.CreditCards[0].ExpiresEnd.Year - 2000 < 10)
                {
                    expiresEndText.Append(0);
                }
                expiresEndText.Append(client.CreditCards[0].ExpiresEnd.Year - 2000);

                CreditCardExpiresEnd.Text = expiresEndText.ToString();
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

            var clientCreator = new ClientCreator()
            {
                PhoneNumber = NewPhoneNumberTextBox.Text
            };
            if(clientCreator.IsValidArgs())
            {
                client.PhoneNumber = NewPhoneNumberTextBox.Text;

                if (Utilities.SaveToDb(client))
                {
                    SettingsCanvas.Visibility = Visibility.Collapsed;
                    User = client;
                }
            }
        }
    }
}
