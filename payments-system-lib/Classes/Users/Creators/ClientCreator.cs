using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    .Client
                    .Include(c => c.Cards)
                    .FirstOrDefault(c => c.PhoneNumber == PhoneNumber && c.EncryptedPassword == encryptedPassword);

                return client;
            }
        }

        public override BaseUser CreateNew()
        {
            var encryptedPassword = Utilities.Utilities.CreateMD5(RealPassword);

            var client = new Client(PhoneNumber, encryptedPassword, DateTime.Now);

            using (var db = new ApplicationContext())
            {
                var query = db.Client
                    .Where(c => c.PhoneNumber == client.PhoneNumber)
                    .ToList();

                if (query.Count != 0)
                    return null;

                db.Client.Add(client);
                db.ClientCard.UpdateRange(client.Cards);
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

                db.ClientCard.RemoveRange(foundClient.Cards);
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

        public IEnumerable GetAllWIthRegex(string phoneRegex)
        {
            using (var db = new ApplicationContext())
            {
                return
                    db.Client
                        .Where(c => Regex.IsMatch(c.PhoneNumber, phoneRegex))
                        .Join(db.ClientCard,
                            c => c.Id,
                            cc => cc.Client.Id,
                            (c, cc) => new { Client = c, ClientCard = cc })
                        .AsEnumerable()
                        .GroupBy(arg => arg.Client.PhoneNumber)
                        .Select(arg => new
                        {
                            PhoneNumber = arg.FirstOrDefault().Client.PhoneNumber,
                            RegistrationDate = arg.FirstOrDefault().Client.RegistrationDate,
                            Money = arg.Sum(sarg => sarg.ClientCard.ClientMoney)
                        }).ToList();
            }
        }
    }
}
