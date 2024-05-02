﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace payments_system_ui.UI.Registration
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
                RegistrationPageUri = new Uri("/UI/Registration/ClientRegistrationMenu.xaml", UriKind.RelativeOrAbsolute)
            });
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            UserChosen(this, new UserChosenArgs()
            {
                RegistrationPageUri = new Uri("/UI/Registration/AdminRegistrationMenu.xaml", UriKind.RelativeOrAbsolute)
            });
        }

        public EventHandler<UserChosenArgs> UserChosen;
    }


    public class UserChosenArgs : EventArgs
    {
        public Uri RegistrationPageUri { get; set; }
    }
}
