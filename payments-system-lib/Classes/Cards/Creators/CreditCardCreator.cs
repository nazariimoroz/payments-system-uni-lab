using System;
using payments_system_lib.Classes.Users;
using payments_system_lib.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace payments_system_lib.Classes.Cards.Creators
{
    public class CreditCardCreator : DbAgentCreator<CreditCard>
    {
        public string Num { get; set; } = null;
        public Client Client { get; set; } = null;

        /// <summary>
        /// + Num
        /// </summary>
        public override async Task<CreditCard> TryGetFromDb()
        {
            if (Num == null)
                throw new InvalidParamException(nameof(Num));

            using (var db = new ApplicationContext())
            {
                var baseCard = await db
                    .CreditCard
                    .Include(c => c.Client)
                    .FirstOrDefaultAsync(c => c.Num == Num);

                return baseCard;
            }
        }

        /// <summary>
        /// + Client
        /// </summary>
        public override async Task<CreditCard> CreateNew()
        {
            if (Client == null)
                throw new InvalidParamException(nameof(Num));

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
                var client = await db.Client.FirstOrDefaultAsync(c => c.Id == Client.Id);
                if (client == null)
                    throw new InvalidParamException(nameof(Client));

                CreditCard card;
                do
                {
                    card = await db.CreditCard.FirstOrDefaultAsync(c => c.Num == num);

                    numBuilder = new StringBuilder();
                    for (int i = 0; i < 16; ++i)
                        numBuilder.Append(random.Next(0, 9));
                    num = numBuilder.ToString();
                } while (card != null);

                toRet = new CreditCard(num, cvc, clientMoney, creditLimit, expiresEnd, client);

                await db.CreditCard.AddAsync(toRet);
                db.Client.Update(toRet.Client);
                await db.SaveChangesAsync();
            }

            return toRet;
        }

        public override async Task<List<T>> GetAll<T>()
        {
            using (var db = new ApplicationContext())
            {
                return await db
                    .CreditCard
                    .Include(c => c.Client)
                    .Select(c => c as T)
                    .ToListAsync();
            }
        }

        public override async Task Save(CreditCard toSave)
        {
            using (var db = new ApplicationContext())
            {
                var card = await db.CreditCard.FirstOrDefaultAsync(c => c.Id == toSave.Id);
                if (card == null)
                    throw new InvalidParamException(nameof(toSave));
                db.Entry(card).CurrentValues.SetValues(toSave);
                db.Update(card);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// + Client
        /// </summary>
        public override async Task Destroy(CreditCard toDestroy)
        {
            if (Client == null)
                throw new InvalidParamException(nameof(Client));
            
            using (var db = new ApplicationContext())
            {
                var card = await db
                    .CreditCard
                    .Include(c => Client)
                    .Include(c => Client)
                    .FirstOrDefaultAsync(c => c.Id == toDestroy.Id);
                if (card == null)
                    throw new InvalidParamException(nameof(toDestroy));

                var client = await db.Client.FirstOrDefaultAsync(c => c.Id == Client.Id);
                if (client == null)
                    throw new InvalidParamException(nameof(Client));

                var transactions = await db
                    .Transaction
                    .Include(t => client)
                    .Where(t => t.Card.Id == card.Id)
                    .ToListAsync();
                if (client == null)
                    throw new InvalidParamException(nameof(Client));

                db.CreditCard.Remove(card);
                db.Client.Update(client);
                db.Transaction.UpdateRange(transactions);
                await db.SaveChangesAsync();
            }
        }
    }
}