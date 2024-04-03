using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using payments_system_uni_lab.Users;

namespace payments_system_uni_lab.Objects
{
    public class CreditCard
    {
        public int Id { get; set; }
        public UInt64 Num { get; protected set; }
        public UInt16 Cvc { get; protected set; }
        public UInt64 ClientMoney { get; protected set; }
        public UInt64 CreditLimit { get; protected set; }
        public int ClientId { get; protected set; }
        public Client Client { get; protected set; }

        public static CreditCard TryGetFromDb()
        {
            return null;
        }

        public static CreditCard CreateNew(Client client)
        {
            var random = new Random();
                
            UInt64 num = random.NextUInt64();
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
        protected CreditCard(UInt64 num, UInt16 cvc, UInt64 clientMoney, UInt64 creditLimit)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            CreditLimit = creditLimit;
        }

        protected CreditCard(UInt64 num, UInt16 cvc, UInt64 clientMoney, UInt64 creditLimit, Client client)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            CreditLimit = creditLimit;
            Client = client;
        }
    }
}
