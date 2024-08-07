﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using HexInnovation;
using payments_system_lib.Classes;
using payments_system_lib.Classes.Cards;
using payments_system_lib.Classes.Cards.Creators;
using payments_system_lib.Classes.Users;
using payments_system_lib.Classes.Users.Creators;
using payments_system_lib.Utilities;
using payments_system_ui.UI.Elements;
using payments_system_ui.UI.Elements.Replenish;
using payments_system_ui.UI.Elements.Send;
using payments_system_ui.Windows;
using payments_system_ui.Windows.Elements;

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

        private async void NewPhoneNumberButton_Click(object sender, RoutedEventArgs e)
        {
            var clientCreator = new ClientCreator()
            {
                PhoneNumber = NewPhoneNumberTextBox.Text
            };
            if(await clientCreator.CanBeRegistered())
            {
                _client.PhoneNumber = NewPhoneNumberTextBox.Text;
                try
                {
                    await new ClientCreator().Save(_client);
                    SettingsGrid.Visibility = Visibility.Collapsed;
                    User = _client;
                }
                catch (InvalidParamException) { }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            PopupWindowGrid.Visibility = Visibility.Visible;

            var ui = new SendToOtherCardUi();
            PopupWindowFrame.Content = ui;

            ui.CloseEvent = (o, args) => ClosePopupWindow();

            ui.SendClicked = async (o, info) =>
            {
                await SendFromSelectedCardToOtherCard(info);
                ClosePopupWindow();
            };
        }

        private void ReplenishButton_Click(object sender, RoutedEventArgs e)
        {
            PopupWindowGrid.Visibility = Visibility.Visible;

            var ui = new ReplenishUi();
            PopupWindowFrame.Content = ui;

            ui.CloseEvent = (o, args) => ClosePopupWindow();

            ui.ReplenishClicked = async (o, info) =>
            {
                await ReplenishSelectedCard(info);
                ClosePopupWindow();
            };
        }

        private void TransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCard = _client.Cards[_currentCardIndex];

            var transactionWindow = new TransactionsWindow(selectedCard);
            transactionWindow.Owner = Window.GetWindow(this);

            transactionWindow.ShowDialog();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(Window.GetWindow(this) is MainWindow mainWindow))
                throw new InvalidCastException("Window.GetWindow(this)");
            mainWindow.RestartFromRegistrationWindow();
        }

        private async void DeleteCurrentAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var creator = new ClientCreator();
            await creator.Destroy(User);
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

            addNewCardUi.CreateCardEvent = async (o, info) =>
            {
                await CreateNewCard(o, info);
                ClosePopupWindow();
            };
        }

        private void CloseSelectedCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCardIndex == 0) return; // TODO
            PopupWindowGrid.Visibility = Visibility.Visible;

            var ui = new CloseSelectedCardUi(_client.Cards[_currentCardIndex]);
            PopupWindowFrame.Content = ui;

            ui.CloseEvent = (o, args) => ClosePopupWindow();
            ui.CloseCardEvent = async (o, args) =>
            {
                await CloseSelectedCard();
                ClosePopupWindow();
            };
        }


        private void InitCardsStackPanel()
        {
            var cards = _client.Cards;
            CardsStackPanel.Children.Clear();
            foreach (var baseCard in cards)
            {
                var baseCardUi = new CreditCardUi(baseCard);
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
                    if (!(f.Content is CreditCardUi cardUi))
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

        private async Task CreateNewCard(object sender, CreateCardInfo e)
        {
            var creditCardCreator = new CreditCardCreator()
            {
                Client = _client
            };
            var creditCard = await creditCardCreator.CreateNew();
            creditCard.CreditLimit = e.CreditLimit;

            await creditCardCreator.Save(creditCard);

            User = await new ClientCreator { PhoneNumber = _client.PhoneNumber, EncryptedPassword = _client.EncryptedPassword }.TryGetFromDb();
        }


        private async Task CloseSelectedCard()
        {
            var selectedCard = _client.Cards[_currentCardIndex];
            await new CreditCardCreator(){Client = _client}.Destroy(selectedCard);

            User = await new ClientCreator { PhoneNumber = _client.PhoneNumber, EncryptedPassword = _client.EncryptedPassword }.TryGetFromDb();
        }

        private async Task SendFromSelectedCardToOtherCard(SendInfo info)
        {
            var selectedCard = _client.Cards[_currentCardIndex];
            var (isSuccess, receiver) = await selectedCard.SendMoneyToOtherCard(info);
            if (!isSuccess) return;
            await new CreditCardCreator().Save(selectedCard);
            await new CreditCardCreator().Save(receiver);
            User = await new ClientCreator { PhoneNumber = _client.PhoneNumber, EncryptedPassword = _client.EncryptedPassword }.TryGetFromDb();
        }

        private async Task ReplenishSelectedCard(ReplenishInfo info)
        {
            var selectedCard = _client.Cards[_currentCardIndex];
            await selectedCard.ReplenishFromSource(info);

            await new CreditCardCreator().Save(selectedCard);
            User = await new ClientCreator { PhoneNumber = _client.PhoneNumber, EncryptedPassword = _client.EncryptedPassword }.TryGetFromDb();
        }

    }
}
