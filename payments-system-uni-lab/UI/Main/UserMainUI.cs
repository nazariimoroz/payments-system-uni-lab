using System;
using System.Windows.Controls;
using payments_system_uni_lab.Users;

namespace payments_system_uni_lab.UI.Main
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
