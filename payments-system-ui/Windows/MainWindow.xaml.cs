using System;
using System.Windows;
using System.Windows.Navigation;
using payments_system_lib.Classes.Users;
using payments_system_ui.UI.Main;

namespace payments_system_ui.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Activated += OnActivated;
            UserMainUi.LoadCompleted += UserMainUiOnLoadCompleted;
        }

        public void RestartFromRegistrationWindow()
        {
            UserMainUi.LoadCompleted -= UserMainUiOnLoadCompleted;
            _registrationWindow = null;
            _currentUser = null;
            UserMainUi.Content = null;
            UserMainUi.LoadCompleted += UserMainUiOnLoadCompleted;
            CreateRegistrationWindow();
        }

        private void OnActivated(object sender, EventArgs e)
        {
            if (_currentUser != null || _registrationWindow != null)
                return;

            CreateRegistrationWindow();
        }

        private void CreateRegistrationWindow()
        {
            _registrationWindow = new RegistrationWindow();
            _registrationWindow.Owner = this;
            if (_registrationWindow.ShowDialog() == true)
            {
                _currentUser = _registrationWindow.LoggedUser;
                UserMainUi.Source = _registrationWindow.LoggedUserUi;
            }
            else
            {
                Close();
            }
        }

        private void UserMainUiOnLoadCompleted(object sender, NavigationEventArgs e)
        {
            if (!(e.Content is UserMainUI ui))
            {
                return;
            }

            ui.User = _currentUser;
        }

        private BaseUser _currentUser = null;
        private RegistrationWindow _registrationWindow;
    }
}
