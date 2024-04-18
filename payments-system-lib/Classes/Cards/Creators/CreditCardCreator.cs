using System;
using System.Linq;
using System.Text;
using payments_system_lib.Classes.Users;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Cards.Creators
{
    public class CreditCardCreator : BaseCardCreator
    {
        public Client Client;

        public override BaseCard TryGetFromDb()
        {
            return null;
        }

        public override BaseCard CreateNew()
        {
            if (Client == null)
                return null;
            var random = new Random();

            var numBuilder = new StringBuilder();
            for (int i = 0; i < 16; ++i)
                numBuilder.Append(random.Next(0, 9));
            var num = numBuilder.ToString();

            numBuilder = new StringBuilder();
            for (int i = 0; i < 3; ++i)
                numBuilder.Append(random.Next(0, 9));
            var cvc = numBuilder.ToString();

            const float clientMoney = 0;
            const float creditLimit = 100000;
            var expiresEnd = new DateTime(DateTime.Now.Year + 5, DateTime.Now.Month, DateTime.Now.Day);

            CreditCard toRet;

            using (var db = new ApplicationContext())
            {
                BaseCard card;
                do
                {
                    card = db.ClientCards.FirstOrDefault(c => c.Num == num);

                    numBuilder = new StringBuilder();
                    for (int i = 0; i < 16; ++i)
                        numBuilder.Append(random.Next(0, 9));
                    num = numBuilder.ToString();
                } while (card != null);

                toRet = new CreditCard(num, cvc, clientMoney, creditLimit, expiresEnd, Client);

                db.ClientCards.Add(toRet);
                db.Clients.Update(toRet.Client);
                db.SaveChanges();
            }

            return toRet;
        }
    }
}