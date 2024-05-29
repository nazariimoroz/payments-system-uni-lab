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
            InitCreditCardsDataGrid();
        }

        void InitCreditCardsDataGrid()
        {
            CreditCardsDataGrid.ItemsSource = _client.Cards;
        }

        private void CardTransactionsButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ChangeCardCreditLimitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BlockCardButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
