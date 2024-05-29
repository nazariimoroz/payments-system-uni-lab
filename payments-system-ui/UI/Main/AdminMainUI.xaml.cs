using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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
using payments_system_lib.Classes.Cards;
using payments_system_lib.Classes.Cards.Creators;
using payments_system_lib.Classes.Users;
using payments_system_lib.Classes.Users.Creators;
using payments_system_ui.Windows.Elements;

namespace payments_system_ui.UI.Main
{
    /// <summary>
    /// Interaction logic for AdminMainUI.xaml
    /// </summary>
    public partial class AdminMainUI : UserMainUI
    {
        public override BaseUser User { get; set; }
        private Client _selectedClient = null;
        private List<Client> _clients;

        public AdminMainUI()
        {
            InitializeComponent();

            ClientMenuPanel.Visibility = Visibility.Collapsed;

            ClientDataGrid.SelectedCellsChanged += OnClientDataGridOnSelectedCellsChanged;

            UpdateUsersDataGrid(PhoneRegexTextBox.Text);
        }

        private void PhoneRegex_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                UpdateUsersDataGrid(textBox.Text);
            }
        }

        private void ViewClientCreditCardsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClient == null)
                return;

            var viewWinCreditCard = new ViewClientCreditCardsWindow(_selectedClient);
            viewWinCreditCard.Owner = Window.GetWindow(this);
            if (viewWinCreditCard.ShowDialog().GetValueOrDefault(false))
            {
                UpdateUsersDataGrid(PhoneRegexTextBox.Text);
            }
        }

        private void BlockClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedClient == null)
                return;

            new ClientCreator().Destroy(_selectedClient);
            UpdateUsersDataGrid(PhoneRegexTextBox.Text);
        }

        private void OnClientDataGridOnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs args)
        {
            if (args == null || args.AddedCells.Count == 0)
            {
                SelectClient(null);
                return;
            }
            
            dynamic clientInfo = args.AddedCells[0].Item;
            var phoneNumber = clientInfo.PhoneNumber as string;
            var client = _clients.Find(c => c.PhoneNumber == phoneNumber);
            if (client == null) throw new Exception("Bad client number");
            SelectClient(client);
        }

        private void UpdateUsersDataGrid(string phoneRegex)
        {
            try
            {
                ClientDataGrid.UnselectAllCells();

                _clients = new ClientCreator
                {
                    WherePredicate =
                        client => Regex.IsMatch(client.PhoneNumber,
                            phoneRegex)
                }.GetAll<Client>();

                ClientDataGrid.ItemsSource =
                    from client in _clients
                    select new
                    {
                        PhoneNumber=client.PhoneNumber, 
                        RegistrationDate=client.RegistrationDate, 
                        Money=client.Cards.Sum(c => c.ClientMoney)
                    };
            }
            catch (Exception e)
            {
                ClientDataGrid.ItemsSource = null;
            }
        }

        private void SelectClient(Client client)
        {
            _selectedClient = client;
            if (_selectedClient == null)
            {
                ClientMenuPanel.Visibility = Visibility.Collapsed;
                return;
            }
            
            ClientMenuPanel.Visibility = Visibility.Visible;
            ClientPhoneNumberTextBox.Text = _selectedClient.PhoneNumber;
        }

    }
}
