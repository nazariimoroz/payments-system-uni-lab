using System;
using System.Windows;
using System.Windows.Controls;
using payments_system_lib.Classes.Cards;

namespace payments_system_ui.UI.Elements.Replenish
{
    /// <summary>
    /// Interaction logic for ReplenishUi.xaml
    /// </summary>
    public partial class ReplenishUi : Page
    {
        public EventHandler CloseEvent;
        public EventHandler<ReplenishInfo> ReplenishClicked;


        public ReplenishUi()
        {
            InitializeComponent();

            var replenishChoosingMenu = new ReplenishChoosingMenu();
            MainFrame.Content = replenishChoosingMenu;
            replenishChoosingMenu.OnClicked += ReplenishChoosingMenuOnClicked;
        }

        private void ClosePopupWindow(object sender, RoutedEventArgs e)
        {
            CloseEvent(this, EventArgs.Empty);
        }

        private void ReplenishChoosingMenuOnClicked(object sender, ReplenishSource e)
        {
            switch (e)
            {
                case ReplenishSource.Debug:
                {
                    var replenishDebug = new ReplenishDebugUi();
                    MainFrame.Content = replenishDebug;
                    replenishDebug.ReplenishClicked += ReplenishClicked;
                    break;
                }
                case ReplenishSource.ApplePay:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }
    }
}
