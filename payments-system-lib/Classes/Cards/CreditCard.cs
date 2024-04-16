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
        public UInt16 Cvc { get; protected set; }
        public UInt64 ClientMoney { get; protected set; }
        public UInt64 CreditLimit { get; protected set; }
        
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
            UInt16 cvc = (UInt16)random.Next(0, 999);
            UInt64 clientMoney = 0;
            UInt64 creditLimit = 100000;

            CreditCard toRet;

            using (var db = new ApplicationContext())
            {
                CreditCard card;
                do
                {
                    card = db.CreditCards.FirstOrDefault(c => c.Num == num);
                } while (card != null);

                toRet = new CreditCard(num, cvc, clientMoney, creditLimit, client);

                db.CreditCards.Add(toRet);
                db.Clients.Update(toRet.Client);
                db.SaveChanges();
            }

            return toRet;
        }

        /*
         * For EC Core
         */
        protected CreditCard(string num, UInt16 cvc, UInt64 clientMoney, UInt64 creditLimit)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            CreditLimit = creditLimit;
        }

        protected CreditCard(string num, UInt16 cvc, UInt64 clientMoney, UInt64 creditLimit, Client client)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            CreditLimit = creditLimit;
            Client = client;
        }
    }
}
