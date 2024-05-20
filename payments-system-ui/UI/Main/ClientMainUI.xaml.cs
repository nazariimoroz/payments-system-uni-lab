using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HexInnovation;
using payments_system_lib.Classes.Users;
using payments_system_lib.Classes.Users.Creators;
using payments_system_lib.Utilities;
using payments_system_ui.UI.Elements;
using payments_system_ui.Windows;

namespace payments_system_ui.UI.Main
{
    /// <summary>
    /// Interaction logic for ClientMainUI.xaml
    /// </summary>
    public partial class ClientMainUI : UserMainUI
    {
        public override BaseUser User
        {
            get => _client;
            set
            {
                if(!(value is Client client))
                {
                    throw new BadUserException("User should be derived from client");
                }
                _client = client;

                ClientPhone.Content = client.PhoneNumber;

                InitCardsStackPanel();
            }
        }
        private Client _client;

        public ClientMainUI()
        {
            InitializeComponent();

            SettingsGrid.Visibility = Visibility.Collapsed;
        }


        private void ClientPhone_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SettingsGrid.Visibility = SettingsGrid.Visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void NewPhoneNumberButton_Click(object sender, RoutedEventArgs e)
        {
            var clientCreator = new ClientCreator()
            {
                PhoneNumber = NewPhoneNumberTextBox.Text
            };
            if(clientCreator.CanBeRegistered())
            {
                _client.PhoneNumber = NewPhoneNumberTextBox.Text;

                if (Utilities.SaveToDb(_client))
                {
                    SettingsGrid.Visibility = Visibility.Collapsed;
                    User = _client;
                }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DeleteCurrentAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var creator = new ClientCreator();
            creator.DestroyUser(User);
            if (!(Window.GetWindow(this) is MainWindow mainWindow))
                throw new InvalidCastException("Window.GetWindow(this)");
            mainWindow.RestartFromRegistrationWindow();
        }
        private void InitCardsStackPanel()
        {
            var cards = _client.Cards;
            foreach (var baseCard in cards)
            {
                var baseCardUi = new BaseCardUi("TODO",
                    baseCard.Num,
                    baseCard.Cvc,
                    baseCard.ExpiresEnd.ToString("mm/yy"));
                var frame = new Frame
                {
                    Content = baseCardUi
                };

                frame.SetBinding(FrameworkElement.HeightProperty,
                    new Binding("ActualHeight")
                    {
                        Source = CardsScrollViewer,
                        Converter = new MathConverter(),
                        ConverterParameter = "x-20"
                    });

                frame.SetBinding(FrameworkElement.WidthProperty,
                    new Binding("ActualHeight")
                    {
                        Source = CardsScrollViewer,
                        Converter = new MathConverter(),
                        ConverterParameter = "(x-20)*1.61"
                    });

                CardsStackPanel.Children.Add(frame);
            }
        }
    }
}
