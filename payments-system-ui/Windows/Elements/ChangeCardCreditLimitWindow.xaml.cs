using System;
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
using payments_system_lib.Classes.Cards.Creators;

namespace payments_system_ui.Windows.Elements
{
    /// <summary>
    /// Interaction logic for ChangeCardCreditLimitWindow.xaml
    /// </summary>
    public partial class ChangeCardCreditLimitWindow : Window
    {
        private CreditCard _creditCard;

        public ChangeCardCreditLimitWindow(CreditCard creditCard)
        {
            InitializeComponent();

            _creditCard = creditCard;
            CreditCardNumTextBlock.Text = _creditCard.Num;
            OldCreditLimitTextBlock.Text = $"Old Credit Limit: {_creditCard.CreditLimit}";
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (_creditCard == null || !float.TryParse(NewCreditLimitTextBox.Text, out var newCreditLimit) || newCreditLimit < 0.0F)
                return;
            _creditCard.CreditLimit = newCreditLimit;
            new CreditCardCreator().Save(_creditCard);
            DialogResult = true;
            Close();
        }
    }
}
