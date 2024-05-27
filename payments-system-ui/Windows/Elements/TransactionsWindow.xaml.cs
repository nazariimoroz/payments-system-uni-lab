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
        public TransactionsWindow(in BaseCard baseCard)
        {
            InitializeComponent();
            
            InitCardNum(in baseCard);
            InitAmount(in baseCard);
            InitTransactions(in baseCard);
        }

        private void InitCardNum(in BaseCard baseCard)
        {
            CardNumTextBlock.Text = baseCard.Num;
        }

        private void InitAmount(in BaseCard baseCard)
        {
            AmountTextBlock.Text = baseCard.ClientMoney.ToString(CultureInfo.CurrentCulture) + "$";
        }

        private void InitTransactions(in BaseCard baseCard)
        {
            var transactions = new TransactionCreator() { Card = baseCard }.GetAll<Transaction>();
            transactions.Sort((t1, t2) => t2.CreationDate.CompareTo(t1.CreationDate));
            TransactionsDataGrid.ItemsSource = transactions;
        }

    }
}
