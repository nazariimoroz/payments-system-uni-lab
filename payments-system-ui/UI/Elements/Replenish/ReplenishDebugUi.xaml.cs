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
using payments_system_lib.Classes.Cards;

namespace payments_system_ui.UI.Elements.Replenish
{
    /// <summary>
    /// Interaction logic for ReplenishDebug.xaml
    /// </summary>
    public partial class ReplenishDebugUi : Page
    {
        public EventHandler<ReplenishInfo> ReplenishClicked;

        public ReplenishDebugUi()
        {
            InitializeComponent();
        }

        private void Replenish_Click(object sender, RoutedEventArgs e)
        {
            if (!float.TryParse(AmountOfMoneyTextBox.Text, NumberStyles.Any, new NumberFormatInfo(), out var amount))
                return;
            if (amount <= 0.0F)
                return;
            ReplenishClicked(this, new ReplenishInfo() { Amount = amount, ReplenishSource = ReplenishSource.Debug});
        }
    }

}
