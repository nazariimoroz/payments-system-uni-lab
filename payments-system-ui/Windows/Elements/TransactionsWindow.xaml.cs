using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
using payments_system_lib.Classes.Transaction;
using payments_system_lib.Classes.Transaction.Creators;

namespace payments_system_ui.Windows.Elements
{
    /// <summary>
    /// Interaction logic for TransactionsWindow.xaml
    /// </summary>
    public partial class TransactionsWindow : Window
    {
        public TransactionsWindow(in CreditCard creditCard)
        {
            InitializeComponent();
            
            InitCardNum(in creditCard);
            InitAmount(in creditCard);
            InitTransactions(creditCard);
        }

        private void InitCardNum(in CreditCard creditCard)
        {
            CardNumTextBlock.Text = creditCard.Num;
        }

        private void InitAmount(in CreditCard creditCard)
        {
            AmountTextBlock.Text = creditCard.ClientMoney.ToString(CultureInfo.CurrentCulture) + "$";
        }

        private void InitTransactions(CreditCard creditCard)
        {
            var transactions = new TransactionCreator{ Card = creditCard }.GetAll<Transaction>().Result;
            transactions.Sort((t1, t2) => t2.CreationDate.CompareTo(t1.CreationDate));
            TransactionsDataGrid.ItemsSource = transactions;
        }

    }
}
