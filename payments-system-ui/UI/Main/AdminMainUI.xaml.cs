using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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
using payments_system_lib.Classes.Cards.Creators;
using payments_system_lib.Classes.Users;
using payments_system_lib.Classes.Users.Creators;

namespace payments_system_ui.UI.Main
{
    /// <summary>
    /// Interaction logic for AdminMainUI.xaml
    /// </summary>
    public partial class AdminMainUI : UserMainUI
    {
        public override BaseUser User { get; set; }

        public AdminMainUI()
        {
            InitializeComponent();

            updateUsersDataGrid(PhoneRegexTextBox.Text);
        }

        private void PhoneRegex_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                updateUsersDataGrid(textBox.Text);
            }

        }

        private void updateUsersDataGrid(string phoneRegex)
        {
            var clientCreator = new ClientCreator();
            var cardCreator = new BaseCardCreator();
            try
            {
                UsersDataGrid.ItemsSource =
                    from client in clientCreator.GetAll<Client>()
                    where Regex.IsMatch(client.PhoneNumber, phoneRegex)
                    select new
                    {
                        PhoneNumber=client.PhoneNumber, 
                        RegistrationDate=client.RegistrationDate, 
                        Money=client.Cards.Sum(c => c.ClientMoney)
                    };
            }
            catch (Exception e)
            {
                UsersDataGrid.ItemsSource = null;
            }
        }

    }
}
