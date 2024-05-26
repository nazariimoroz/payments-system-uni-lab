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

namespace payments_system_ui.UI.Elements.Replenish
{
    /// <summary>
    /// Interaction logic for ReplenishChoosingMenu.xaml
    /// </summary>
    public partial class ReplenishChoosingMenu : Page
    {
        public EventHandler<ReplenishSource> OnClicked;

        public ReplenishChoosingMenu()
        {
            InitializeComponent();
        }

        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            OnClicked(this, ReplenishSource.Debug);
        }


        private void ApplePay_Click(object sender, RoutedEventArgs e)
        {
            OnClicked(this, ReplenishSource.ApplePay);
        }
    }
}
