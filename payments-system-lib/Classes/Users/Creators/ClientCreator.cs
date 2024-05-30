using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using payments_system_lib.Classes.Cards.Creators;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Users.Creators
{
    public class ClientCreator : BaseUserCreator
    {
        public string PhoneNumber { get; set; } = null;
        public string RealPassword { get; set; } = null;
        public string EncryptedPassword { get; set; } = null;
        public Func<Client, bool> WherePredicate { get; set; } = (Client c) => true;

        private string GetEncryptedPassword() => RealPassword == null ? EncryptedPassword : Utilities.Utilities.CreateMD5(RealPassword);

        /// <summary>
        /// + RealPassword OR EncryptedPassword <br/>
        /// + PhoneNumber
        /// </summary>
        public override async Task<BaseUser> TryGetFromDb()
        {
            var encryptedPassword = GetEncryptedPassword();
            if (PhoneNumber == null)
                throw new InvalidParamException(nameof(PhoneNumber));
            if (encryptedPassword == null)
                throw new InvalidParamException($"{nameof(RealPassword)} OR {nameof(EncryptedPassword)}");

            using (var db = new ApplicationContext())
            {
                var client = await db
                    .Client
                    .Include(c => c.Cards)
                    .FirstOrDefaultAsync(c => c.PhoneNumber == PhoneNumber && c.EncryptedPassword == encryptedPassword);

                return client;
            }
        }

        /// <summary>
        /// + RealPassword OR EncryptedPassword <br/>
        /// + PhoneNumber
        /// </summary>
        public override async Task<BaseUser> CreateNew()
        {
            var encryptedPassword = GetEncryptedPassword();
            if (PhoneNumber == null)
                throw new InvalidParamException(nameof(PhoneNumber));
            if (encryptedPassword == null)
                throw new InvalidParamException($"{nameof(RealPassword)} OR {nameof(EncryptedPassword)}");

            var client = new Client(PhoneNumber, encryptedPassword, DateTime.Now);

            using (var db = new ApplicationContext())
            {
                var query = await db
                    .Client
                    .Where(c => c.PhoneNumber == client.PhoneNumber)
                    .ToListAsync();

                if (query.Count != 0)
                    return null;

                await db.Client.AddAsync(client);
                db.CreditCard.UpdateRange(client.Cards);
                await db.SaveChangesAsync();
            }

            await new CreditCardCreator(){ Client = client }.CreateNew();

            using (var db = new ApplicationContext())
            {
                client = await db
                    .Client
                    .Include(c => c.Cards)
                    .FirstOrDefaultAsync(c => c.Id == client.Id);
            }

            return client;
        }

        /// <summary>
        /// + RealPassword(optional) <br/>
        /// + PhoneNumber
        /// </summary>
        public override async Task<bool> CanBeRegistered()
        {
            if (!await IsValidArgs())
                return false;

            using (var db = new ApplicationContext())
            {
                var query = await db.Client
                    .FirstOrDefaultAsync(c => c.PhoneNumber == PhoneNumber);

                return query == null;
            }
        }

        /// <summary>
        /// + RealPassword(optional) <br/>
        /// + PhoneNumber(optional)
        /// </summary>
        public override async Task<bool> IsValidArgs()
        {
            if (RealPassword != null && RealPassword.Length < 8)
                return false;

            if (PhoneNumber != null && !Regex.IsMatch(
                    PhoneNumber, 
                    "^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$"))
                return false;

            return true;
        }


        /// <summary>
        /// + WherePredicate(optional)
        /// </summary>
        public override async Task<List<T>> GetAll<T>()
        {
            using (var db = new ApplicationContext())
            {
                return db
                    .Client
                    .Include(c => c.Cards)
                    .Where(WherePredicate)
                    .Select(c => c as T)
                    .ToList();
            }
        }

        public override async Task Save(BaseUser toSave)
        {
            if (!(toSave is Client clientToSave))
                throw new InvalidParamException(nameof(toSave));

            using (var db = new ApplicationContext())
            {
                var client = await db.Client.FirstOrDefaultAsync(c => c.Id == clientToSave.Id);
                if (client == null)
                    throw new InvalidParamException(nameof(toSave));
                db.Entry(client).CurrentValues.SetValues(clientToSave);
                db.Update(client);
                await db.SaveChangesAsync();
            }
        }

        public override async Task Destroy(BaseUser toDestroy)
        {
            if (!(toDestroy is Client client))
                throw new InvalidParamException(nameof(toDestroy));

            using (var db = new ApplicationContext())
            {
                var foundClient = await db
                    .Client
                    .Include(c => c.Cards)
                    .FirstOrDefaultAsync(c => c.Id == client.Id);

                if (foundClient == null)
                    return;

                foreach (var foundClientCard in foundClient.Cards)
                {
                    db.Transaction.RemoveRange(
                        await db
                            .Transaction
                            .Include(t => t.Card)
                            .Where(t => t.Card.Id == foundClientCard.Id)
                            .ToListAsync()
                        );
                }
                db.CreditCard.RemoveRange(foundClient.Cards);
                db.Client.Remove(foundClient);
                await db.SaveChangesAsync();
            }
        }
    }
}
