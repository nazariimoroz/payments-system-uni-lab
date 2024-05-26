using payments_system_lib.Classes.Cards;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace payments_system_ui.UI.Elements.Send
{
    /// <summary>
    /// Interaction logic for SendToOtherCardUi.xaml
    /// </summary>
    public partial class SendToOtherCardUi : Page
    {
        public EventHandler CloseEvent;
        public EventHandler<SendInfo> SendClicked;
        public SendToOtherCardUi()
        {
            InitializeComponent();
        }

        private void ClosePopupWindow(object sender, RoutedEventArgs e)
        {
            CloseEvent(this, EventArgs.Empty);
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            var numOfReceiver = CardNumberTextBox.Text;
            if (numOfReceiver.Length != 16)
                return;
            if (!float.TryParse(AmountOfMoneyTextBox.Text, NumberStyles.Any, new NumberFormatInfo(), out var amount) 
                || amount <= 0.0F)
                return;

            SendClicked(this, new SendInfo { Amount = amount, NumOfReceiver = numOfReceiver });
        }
    }
}
