using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using payments_system_lib.Classes.Cards;
using payments_system_lib.Classes.Cards.Creators;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Users.Creators
{
    public class ClientCreator : BaseUserCreator
    {
        public string PhoneNumber { get; set; } = null;
        public string RealPassword { get; set; } = null;
        public string EncryptedPassword { get; set; } = null;

        private string GetEncryptedPassword() => RealPassword == null ? EncryptedPassword : Utilities.Utilities.CreateMD5(RealPassword);

        public override BaseUser TryGetFromDb()
        {
            var encryptedPassword = GetEncryptedPassword();
            if (PhoneNumber == null)
                throw new InvalidParamException(nameof(PhoneNumber));
            if (PhoneNumber == null)
                throw new InvalidParamException($"{nameof(RealPassword)} OR {nameof(EncryptedPassword)}");

            using (var db = new ApplicationContext())
            {
                var client = db
                    .Client
                    .Include(c => c.Cards)
                    .FirstOrDefault(c => c.PhoneNumber == PhoneNumber && c.EncryptedPassword == encryptedPassword);

                return client;
            }
        }

        public override BaseUser CreateNew()
        {
            var encryptedPassword = GetEncryptedPassword();

            var client = new Client(PhoneNumber, encryptedPassword, DateTime.Now);

            using (var db = new ApplicationContext())
            {
                var query = db.Client
                    .Where(c => c.PhoneNumber == client.PhoneNumber)
                    .ToList();

                if (query.Count != 0)
                    return null;

                db.Client.Add(client);
                db.CreditCard.UpdateRange(client.Cards);
                db.SaveChanges();
            }

            var ccCreator = new CreditCardCreator()
            {
                Client = client
            };
            ccCreator.CreateNew(); // Will be added automatically to array 

            return client;
        }

        public override bool CanBeRegistered()
        {
            if (!IsValidArgs())
                return false;

            using (var db = new ApplicationContext())
            {
                var query = db.Client
                    .FirstOrDefault(c => c.PhoneNumber == PhoneNumber);

                return query == null;
            }
        }

        public override bool IsValidArgs()
        {
            if (RealPassword != null && RealPassword.Length < 8)
                return false;

            if (PhoneNumber != null && !Regex.IsMatch(
                    PhoneNumber, 
                    "^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$"))
                return false;

            return true;
        }

        public override bool DestroyUser(BaseUser toDestroy)
        {
            if (!(toDestroy is Client client))
                return false;
            using (var db = new ApplicationContext())
            {
                var foundClient = db
                    .Client
                    .Include(c => c.Cards)
                    .FirstOrDefault(c => c.Id == client.Id);

                if (foundClient == null)
                    return false;

                db.CreditCard.RemoveRange(foundClient.Cards);
                db.Client.Remove(foundClient);
                db.SaveChanges();
            }

            return true;
        }
        public override List<T> GetAll<T>()
        {
            using (var db = new ApplicationContext())
            {
                return db
                    .Client
                    .Include(c => c.Cards)
                    .Select(c => c as T).ToList();
            }
        }

        public override void Save(BaseUser toSave)
        {
            if (!(toSave is Client clientToSave))
                throw new InvalidParamException(nameof(toSave));

            using (var db = new ApplicationContext())
            {
                var client = db.Client.FirstOrDefault(c => c.Id == clientToSave.Id);
                if (client == null)
                    throw new InvalidParamException(nameof(toSave));
                db.Entry(client).CurrentValues.SetValues(clientToSave);
                db.Update(client);
                db.SaveChanges();
            }
        }

        public override void Destroy(BaseUser toDestroy)
        {
            throw new NotImplementedException();
        }
    }
}
