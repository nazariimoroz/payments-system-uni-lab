using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using payments_system_lib.Classes.Users;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Cards
{
    public class BaseCard
    {
        public int Id { get; set; }
        public string Num { get; protected set; }
        public string Cvc { get; protected set; }
        public float ClientMoney { get; protected set; }
        public float CreditLimit { get; protected set; }
        public DateTime ExpiresEnd { get; protected set; }

        [ForeignKey("ClientId")]
        public Client Client { get; protected set; }

        /*
         * For EC Core
         */
        protected BaseCard(string num, string cvc, float clientMoney, DateTime expiresEnd)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            ExpiresEnd = expiresEnd;

            CreditLimit = 0;
        }

        protected BaseCard(string num, string cvc, float clientMoney, DateTime expiresEnd, Client client)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            ExpiresEnd = expiresEnd;
            Client = client;

            CreditLimit = 0;
        }

        public bool SendMoneyToOtherCard(float amountOfMoney, BaseCard otherCard)
        {
            if (otherCard == null)
                return false;

            if (amountOfMoney > ClientMoney)
                return false;

            ClientMoney -= amountOfMoney;
            otherCard.ClientMoney += amountOfMoney;
            return true;
        }

        public bool ReplenishFromSource(float amountOfMoney, object source /*TODO*/)
        {
            if (source == null)
            {
                ClientMoney += amountOfMoney;
            }

            return false;
        }
    }
}