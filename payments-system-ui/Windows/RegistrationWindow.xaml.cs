using System.Windows;
using System.Windows.Navigation;
using payments_system_lib.Classes.Users;
using payments_system_ui.UI;
using payments_system_ui.UI.Registration;

namespace payments_system_ui.Windows
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public BaseUser LoggedUser { get; private set; } = null;

        public RegistrationWindow()
        {
            InitializeComponent();

            ChoosingRegistrationUserFrame.LoadCompleted += ChoosingRegistrationUserFrameLoaded;
            RegistrationMenuFrame.LoadCompleted += RegistrationMenuFrameLoaded;
        }

        private void ChoosingRegistrationUserFrameLoaded(object sender, NavigationEventArgs e)
        {
            _choosingRegistrationUser = e.Content as ChoosingRegistrationUser;
            if (_choosingRegistrationUser != null)
            {
                _choosingRegistrationUser.UserChosen = UserChosen;
            }
        }

        private void UserChosen(object sender, UserChosenArgs e)
        {
            RegistrationMenuFrame.Source = e.RegistrationPageUri;
        }

        private void RegistrationMenuFrameLoaded(object frame, NavigationEventArgs frameArgs)
        {
            _registrationMenu = frameArgs.Content as BaseRegistrationMenu;
            if (_registrationMenu != null)
            {
                Tabs.SelectedIndex = 1;

                _registrationMenu.ClickBackButtonEvent += (o, args) =>
                {
                    _registrationMenu = null;
                    Tabs.SelectedIndex = 0;
                };

                _registrationMenu.UserLoggedEvent += (menu, user) =>
                {
                    if (user == null)
                    {
                        DialogResult = false;
                        return;
                    }

                    DialogResult = true;
                    LoggedUser = user;
                };
            }
        }

        private ChoosingRegistrationUser _choosingRegistrationUser;
        private BaseRegistrationMenu _registrationMenu;
    }
}
