using System;
using payments_system_lib.Classes.Users;
using payments_system_lib.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace payments_system_lib.Classes.Cards.Creators
{
    public class BaseCardCreator : DbAgentCreator<BaseCard>
    {
        public string Num { get; set; } = null;

        public Client Client;

        public override BaseCard TryGetFromDb()
        {
            if (Num == null)
                throw new InvalidParamException(nameof(Num));
            using (var db = new ApplicationContext())
            {
                var baseCard = db
                    .ClientCard
                    .Include(c => c.Client)
                    .FirstOrDefault(c => c.Num == Num);

                return baseCard;
            }
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

            BaseCard toRet;

            using (var db = new ApplicationContext())
            {
                var client = db.Client.FirstOrDefault(c => c.Id == Client.Id);
                if (client == null)
                    throw new InvalidParamException(nameof(Client));

                BaseCard card;
                do
                {
                    card = db.ClientCard.FirstOrDefault(c => c.Num == num);

                    numBuilder = new StringBuilder();
                    for (int i = 0; i < 16; ++i)
                        numBuilder.Append(random.Next(0, 9));
                    num = numBuilder.ToString();
                } while (card != null);

                toRet = new BaseCard(num, cvc, clientMoney, creditLimit, expiresEnd, client);

                db.ClientCard.Add(toRet);
                db.Client.Update(toRet.Client);
                db.SaveChanges();
            }

            return toRet;
        }

        public override List<T> GetAll<T>()
        {
            using (var db = new ApplicationContext())
            {
                return db
                    .ClientCard
                    .Include(c => c.Client)
                    .Select(c => c as T).ToList();
            }
        }

        public override void Save(BaseCard toSave)
        {
            using (var db = new ApplicationContext())
            {
                var card = db.ClientCard.FirstOrDefault(c => c.Id == toSave.Id);
                if (card == null)
                    throw new InvalidParamException(nameof(toSave));
                db.Entry(card).CurrentValues.SetValues(toSave);
                db.Update(card);
                db.SaveChanges();
            }
        }

        public override void Destroy(BaseCard toDestroy)
        {
            using (var db = new ApplicationContext())
            {
                var card = db.ClientCard.FirstOrDefault(c => c.Id == toDestroy.Id);
                if (card == null)
                    throw new InvalidParamException(nameof(toDestroy));

                var client = db.Client.FirstOrDefault(c => c.Id == Client.Id);
                if (client == null)
                    throw new InvalidParamException(nameof(Client));

                db.ClientCard.Remove(card);
                db.Client.Update(client);
                db.SaveChanges();
            }
        }
    }
}