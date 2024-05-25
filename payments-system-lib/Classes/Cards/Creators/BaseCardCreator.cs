using System;
using payments_system_lib.Classes.Users;
using payments_system_lib.Utilities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace payments_system_lib.Classes.Cards.Creators
{
    public abstract class BaseCardCreator : DbAgentCreator<BaseCard>
    {
        public abstract override BaseCard TryGetFromDb();
        public abstract override BaseCard CreateNew();

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
    }
}