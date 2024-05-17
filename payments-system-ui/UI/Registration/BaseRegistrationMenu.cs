using System;
using System.Windows.Controls;
using payments_system_lib.Classes.Users;

namespace payments_system_ui.UI.Registration
{
    /// <summary>
    /// Interaction logic for RegistrationMenu.xaml
    /// </summary>
    public abstract class BaseRegistrationMenu : Page
    {
        public Uri LoggedUserUi { get; protected set; } = null;

        public delegate void UserLoggedEventHandler(BaseRegistrationMenu menu, BaseUser user);

        public BaseRegistrationMenu()
        {
        }

        public UserLoggedEventHandler UserLoggedEvent;
        public EventHandler ClickBackButtonEvent;
    }
}

