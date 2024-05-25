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

namespace payments_system_ui.UI.Elements
{
    /// <summary>
    /// Interaction logic for AddNewCardUi.xaml
    /// </summary>
    public partial class AddNewCardUi : Page
    {
        public EventHandler CloseEvent;
        public EventHandler<CreateCardInfo> CreateCardEvent;

        public AddNewCardUi()
        {
            InitializeComponent();
        }

        private void ClosePopupWindow(object sender, RoutedEventArgs e)
        {
            CloseEvent(this, e);
        }

        private void CreateCard(object sender, RoutedEventArgs e)
        {
            if (!float.TryParse(CreditLimitTextBox.Text, NumberStyles.Any, new NumberFormatInfo(), out var creditLimit))
                return;
            CreateCardEvent(this, new CreateCardInfo
            {
                CreditLimit = creditLimit
            });
        }
    }

    public struct CreateCardInfo
    {
        public float CreditLimit { get; set; }
    }
}
