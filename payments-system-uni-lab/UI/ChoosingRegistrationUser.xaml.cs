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

namespace payments_system_uni_lab.UI
{
    /// <summary>
    /// Interaction logic for ChoosingRegistrationUser.xaml
    /// </summary>
    public partial class ChoosingRegistrationUser : Page
    {
        public ChoosingRegistrationUser()
        {
            InitializeComponent();
        }

        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            UserChosen(this, new UserChosenArgs()
            {
                RegistrationPageUri = new Uri("/UI/ClientRegistrationMenu.xaml", UriKind.RelativeOrAbsolute)
            });
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            throw new Exception();
        }

        public EventHandler<UserChosenArgs> UserChosen;
    }


    public class UserChosenArgs : EventArgs
    {
        public Uri RegistrationPageUri { get; set; }
    }
}
