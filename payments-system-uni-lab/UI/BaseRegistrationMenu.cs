using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using payments_system_uni_lab.Users;


namespace payments_system_uni_lab.UI
{
    /// <summary>
    /// Interaction logic for RegistrationMenu.xaml
    /// </summary>
    public class BaseRegistrationMenu : Page
    {
        public delegate void UserLoggedEventHandler(BaseRegistrationMenu menu, BaseUser user);

        public BaseRegistrationMenu()
        {
        }

        public UserLoggedEventHandler UserLoggedEvent;
        public EventHandler ClickBackButtonEvent;
    }
}

