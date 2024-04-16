using System;
using System.Windows.Controls;
using payments_system_ui.Users;

namespace payments_system_ui.UI.Main
{
    public abstract class UserMainUI : Page
    {
        public abstract BaseUser User { get; set; }
    }

    public class BadUserException : Exception
    {
        public BadUserException(string message) : base(message)
        {}
    }
}
