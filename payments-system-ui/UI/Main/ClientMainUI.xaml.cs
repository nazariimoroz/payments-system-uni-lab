using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HexInnovation;
using payments_system_lib.Classes;
using payments_system_lib.Classes.Cards.Creators;
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

                _currentCardIndex = 0;

                InitCardsStackPanel();
                InitCardInfo();
            }
        }
        private Client _client;
        private int _currentCardIndex = 0;

        public ClientMainUI()
        {
            InitializeComponent();

            SettingsGrid.Visibility = Visibility.Collapsed;
            PopupWindowGrid.Visibility = Visibility.Collapsed;
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
                try
                {
                    new ClientCreator().Save(_client);
                    SettingsGrid.Visibility = Visibility.Collapsed;
                    User = _client;
                }
                catch (InvalidParamException) { }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(Window.GetWindow(this) is MainWindow mainWindow))
                throw new InvalidCastException("Window.GetWindow(this)");
            mainWindow.RestartFromRegistrationWindow();
        }

        private void DeleteCurrentAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var creator = new ClientCreator();
            creator.DestroyUser(User);
            if (!(Window.GetWindow(this) is MainWindow mainWindow))
                throw new InvalidCastException("Window.GetWindow(this)");
            mainWindow.RestartFromRegistrationWindow();
        }

        private void CreateNewCardButton_Click(object sender, RoutedEventArgs e)
        {
            PopupWindowGrid.Visibility = Visibility.Visible;

            var addNewCardUi = new AddNewCardUi();
            PopupWindowFrame.Content = addNewCardUi;

            addNewCardUi.CloseEvent = (o, args) => ClosePopupWindow();

            addNewCardUi.CreateCardEvent = CreateNewCard;
        }

        private void CloseSelectedCardButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void InitCardsStackPanel()
        {
            var cards = _client.Cards;
            CardsStackPanel.Children.Clear();
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

                frame.MouseDoubleClick += (sender, args) =>
                {
                    if (!(sender is Frame f))
                        return;
                    if (!(f.Content is BaseCardUi cardUi))
                        return;
                    foreach (var cardIndex in cards.Select((value, i) => new { i, value }))
                    {
                        var card = cardIndex.value;
                        var index = cardIndex.i;
                        if (card.Num == cardUi.CreditCardNumber.Text)
                        {
                            _currentCardIndex = index;
                            InitCardInfo();
                            return;
                        }
                    }
                };

                CardsStackPanel.Children.Add(frame);
            }
        }

        private void InitCardInfo()
        {
            if (_client?.Cards.ElementAtOrDefault(_currentCardIndex) == null)
                throw new ArgumentNullException();

            var card = _client.Cards[_currentCardIndex];
            CardBalanceTextBox.Text = $"Card Balance: {card.ClientMoney}";
            CardCreditLimitTextBox.Text = $"Credit Limit: {card.CreditLimit}";
        }

        private void ClosePopupWindow()
        {
            PopupWindowFrame.Content = null;
            PopupWindowGrid.Visibility = Visibility.Collapsed;
        }

        private void CreateNewCard(object sender, CreateCardInfo e)
        {
            var creditCardCreator = new CreditCardCreator()
            {
                Client = _client
            };
            var creditCard = creditCardCreator.CreateNew();
            creditCard.CreditLimit = e.CreditLimit;

            creditCardCreator.Save(creditCard);

            User = new ClientCreator { PhoneNumber = _client.PhoneNumber, EncryptedPassword = _client.EncryptedPassword }.TryGetFromDb();

            ClosePopupWindow();
        }
    }
}
