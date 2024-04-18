using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using payments_system_lib.Classes.Users;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Cards
{
    public class CreditCard
    {
        public int Id { get; set; }
        public string Num { get; protected set; }
        public string Cvc { get; protected set; }
        public UInt64 ClientMoney { get; protected set; }
        public UInt64 CreditLimit { get; protected set; }
        public DateTime ExpiresEnd { get; protected set; }

        [ForeignKey("ClientId")]
        public Client Client { get; protected set; }

        public static CreditCard TryGetFromDb()
        {
            return null;
        }

        public static CreditCard CreateNew(Client client)
        {
            var random = new Random();


            var numBuilder = new StringBuilder();
            for (int i = 0; i < 16; ++i)
                numBuilder.Append(random.Next(0, 9));
            var num = numBuilder.ToString();

            numBuilder = new StringBuilder();
            for (int i = 0; i < 3; ++i)
                numBuilder.Append(random.Next(0, 9));
            var cvc = numBuilder.ToString();

            UInt64 clientMoney = 0;
            UInt64 creditLimit = 100000;
            var expiresEnd = new DateTime(DateTime.Now.Year + 5, DateTime.Now.Month, DateTime.Now.Day);

            CreditCard toRet;

            using (var db = new ApplicationContext())
            {
                CreditCard card;
                do
                {
                    card = db.CreditCards.FirstOrDefault(c => c.Num == num);
                } while (card != null);

                toRet = new CreditCard(num, cvc, clientMoney, creditLimit, expiresEnd, client);

                db.CreditCards.Add(toRet);
                db.Clients.Update(toRet.Client);
                db.SaveChanges();
            }

            return toRet;
        }

        /*
         * For EC Core
         */
        protected CreditCard(string num, string cvc, UInt64 clientMoney, UInt64 creditLimit, DateTime expiresEnd)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            CreditLimit = creditLimit;
            ExpiresEnd = expiresEnd;
        }

        protected CreditCard(string num, string cvc, UInt64 clientMoney, UInt64 creditLimit, DateTime expiresEnd, Client client)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            CreditLimit = creditLimit;
            ExpiresEnd = expiresEnd;
            Client = client;
        }
    }
}
