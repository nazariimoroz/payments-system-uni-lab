using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
using MaterialDesignThemes.Wpf;
using payments_system_uni_lab.UI.Main;
using payments_system_uni_lab.Users;
using payments_system_uni_lab.Windows;

namespace payments_system_uni_lab.Windows
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
            _registrationWindow = new Windows.RegistrationWindow();
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
