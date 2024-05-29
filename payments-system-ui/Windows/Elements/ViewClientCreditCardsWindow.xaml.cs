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
using System.Windows.Shapes;
using payments_system_lib.Classes.Cards;
using payments_system_lib.Classes.Cards.Creators;
using payments_system_lib.Classes.Users;

namespace payments_system_ui.Windows.Elements
{
    public partial class ViewClientCreditCardsWindow : Window
    {
        private readonly Client _client;

        public ViewClientCreditCardsWindow(in Client client)
        {
            InitializeComponent();

            _client = client;
            UpdateCreditCardsDataGrid();
        }

        private void CardTransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (CreditCardsDataGrid == null || CreditCardsDataGrid.SelectedCells.Count == 0)
                return;
            var transactionsWindow = new TransactionsWindow(CreditCardsDataGrid.SelectedCells[0].Item as CreditCard);
            transactionsWindow.Owner = Window.GetWindow(this);
            transactionsWindow.ShowDialog();
        }

        private void ChangeCardCreditLimitButton_Click(object sender, RoutedEventArgs e)
        {
            if (CreditCardsDataGrid == null || CreditCardsDataGrid.SelectedCells.Count == 0)
                return;
            if (!(CreditCardsDataGrid.SelectedCells[0].Item is CreditCard creditCard))
                return;

            var changeCardCreditLimitWindow = new ChangeCardCreditLimitWindow(creditCard);
            changeCardCreditLimitWindow.Owner = this;
            if (changeCardCreditLimitWindow.ShowDialog().GetValueOrDefault(false))
            {
                Close();
            }
        }

        private void BlockCardButton_Click(object sender, RoutedEventArgs e)
        {
            if (CreditCardsDataGrid == null || CreditCardsDataGrid.SelectedCells.Count == 0)
                return;
            if (!(CreditCardsDataGrid.SelectedCells[0].Item is CreditCard creditCard))
                return;
            if (_client.Cards.Count <= 1 || _client.Cards[0] == creditCard)
                return;

            new CreditCardCreator{Client = _client}.Destroy(creditCard);
            DialogResult = true;
            Close();
        }

        void UpdateCreditCardsDataGrid()
        {
            CreditCardsDataGrid.ItemsSource = _client.Cards;
        }
    }
}
