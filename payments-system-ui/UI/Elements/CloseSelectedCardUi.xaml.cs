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
using System.Windows.Navigation;
using System.Windows.Shapes;
using payments_system_lib.Classes.Cards;
using payments_system_lib.Classes.Users;

namespace payments_system_ui.UI.Elements
{
    /// <summary>
    /// Interaction logic for CloseSelectedCardUi.xaml
    /// </summary>
    public partial class CloseSelectedCardUi : Page
    {
        private readonly CreditCard _creditCard;
        public EventHandler CloseEvent;
        public EventHandler CloseCardEvent;

        public CloseSelectedCardUi(in CreditCard creditCard)
        {
            _creditCard = creditCard;
            InitializeComponent();

            BaseCardUiFrame.Content = new CreditCardUi(creditCard);
        }

        private void ClosePopupWindow(object sender, RoutedEventArgs e)
        {
            CloseEvent(this, e);
        }

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            if (_creditCard != null && CreditLimitTextBox.Text == _creditCard.Num)
            {
                CloseCardEvent(this, EventArgs.Empty);
            }
        }
    }
}
