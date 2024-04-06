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
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
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
        }

        private void OnActivated(object sender, EventArgs e)
        {
            if (_registrationCompleted || _registrationWindow != null)
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
            }
            else
            {
                CreateRegistrationWindow();
            }
        }

        private BaseUser _currentUser = null;
        private bool _registrationCompleted = false;
        private RegistrationWindow _registrationWindow;
    }
}
