﻿using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using payments_system_lib.Classes.Cards;
using payments_system_lib.Classes.Cards.Creators;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Users.Creators
{
    public class ClientCreator : BaseUserCreator
    {
        public string PhoneNumber { get; set; } = null;
        public string RealPassword { get; set; } = null;
        
        public override BaseUser TryGetFromDb()
        {
            var encryptedPassword = Utilities.Utilities.CreateMD5(RealPassword);

            using (var db = new ApplicationContext())
            {
                var client = db
                    .Clients
                    .Include(c => c.Cards)
                    .FirstOrDefault(c => c.PhoneNumber == PhoneNumber && c.EncryptedPassword == encryptedPassword);

                return client;
            }
        }

        public override BaseUser CreateNew()
        {
            var encryptedPassword = Utilities.Utilities.CreateMD5(RealPassword);

            var client = new Client(PhoneNumber, encryptedPassword);

            using (var db = new ApplicationContext())
            {
                var query = db.Clients
                    .Where(c => c.PhoneNumber == client.PhoneNumber)
                    .ToList();

                if (query.Count != 0)
                    return null;

                db.Clients.Add(client);
                db.ClientCards.UpdateRange(client.Cards);
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
                var query = db.Clients
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
                    .Clients
                    .Include(c => c.Cards)
                    .FirstOrDefault(c => c.Id == client.Id);

                if (foundClient == null)
                    return false;

                db.ClientCards.RemoveRange(foundClient.Cards);
                db.Clients.Remove(foundClient);
                db.SaveChanges();
            }

            return true;
        }
    }
}