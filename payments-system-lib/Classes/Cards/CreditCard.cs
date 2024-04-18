using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using payments_system_lib.Classes.Users;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Cards
{
    public class CreditCard : BaseCard
    {
        /*
         * For EC Core
         */
        public CreditCard(string num, string cvc, float clientMoney, float creditLimit, DateTime expiresEnd)
            : base(num, cvc, clientMoney, expiresEnd)
        {
            ExpiresEnd = expiresEnd;
        }

        public CreditCard(string num, string cvc, float clientMoney, float creditLimit, DateTime expiresEnd, Client client)
            : base(num, cvc, clientMoney, expiresEnd, client)
        {
            ExpiresEnd = expiresEnd;
        }
    }
}
