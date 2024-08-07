﻿using System;
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
using payments_system_lib.Classes.Cards;

namespace payments_system_ui.UI.Elements
{
    /// <summary>
    /// Interaction logic for CreditCard.xaml
    /// </summary>
    public partial class CreditCardUi : Page
    {
        public CreditCardUi(string creditCardName, string cardNumber, string cardCvc, string cardExpiresEnd)
        {
            InitializeComponent();

            this.CreditCardName.Text = creditCardName;
            this.CreditCardNumber.Text = cardNumber;
            this.CreditCardCvc.Text = cardCvc;
            this.CreditCardExpiresEnd.Text = cardExpiresEnd;
        }

        public CreditCardUi(in CreditCard creditCard)
        {
            InitializeComponent();

            this.CreditCardName.Text = "CREDIT CARD";
            this.CreditCardNumber.Text = creditCard.Num;
            this.CreditCardCvc.Text = creditCard.Cvc;
            this.CreditCardExpiresEnd.Text = creditCard.ExpiresEnd.ToString("MM/yy");
        }
    }
}
