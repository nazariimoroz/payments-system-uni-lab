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
                UserMainUi.Source = _currentUser.UserMainUi;
            }
            else
            {
                CreateRegistrationWindow();
            }
        }

        private void UserMainUiOnLoadCompleted(object sender, NavigationEventArgs e)
        {
            if (!(e.Content is UserMainUI ui))
            {
                throw new UserUiException("Ui should be derived from UserMainUI");
            }

            ui.User = _currentUser;
        }

        private BaseUser _currentUser = null;
        private RegistrationWindow _registrationWindow;
    }

    class UserUiException : Exception
    {
        public UserUiException(string message) : base(message)
        {

        }
    }
}
