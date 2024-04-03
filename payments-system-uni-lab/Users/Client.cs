using System;
using System.Collections.Generic;
using System.Windows.Documents;
using payments_system_uni_lab.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using payments_system_uni_lab.Interfaces;

namespace payments_system_uni_lab.Users
{
    public class Client
    {
        public int Id { get; set; }
        public string PhoneNumber { get; protected set; }
        public string EncryptedPassword { get; protected set; }
        public List<CreditCard> CreditCards { get; protected set; } = new List<CreditCard>();

        public static Client TryGetFromDb(string phoneNumber, string encryptedPassword)
        {
            using (var db = new ApplicationContext())
            {
                var client = db
                    .Clients
                    .FirstOrDefault(c => c.PhoneNumber == phoneNumber && c.EncryptedPassword == encryptedPassword);
                if (client != null)
                {
                    return client;
                }
            }

            return null;
        }

        public static Client CreateNew(string phoneNumber, string encryptedPassword)
        {
            var client = new Client(phoneNumber, encryptedPassword);

            using (var db = new ApplicationContext())
            {
                var query = db.Clients
                    .Where(c => c.PhoneNumber == client.PhoneNumber)
                    .ToList();

                if (query.Count != 0)
                    throw new DbException("This PhoneNumber has been already used");

                db.Clients.Add(client);
                db.CreditCards.UpdateRange(client.CreditCards);
                db.SaveChanges();
            }

            CreditCard.CreateNew(client);

            return client;
        }

        protected Client(string phoneNumber, string encryptedPassword)
        {
            PhoneNumber = phoneNumber;
            EncryptedPassword = encryptedPassword;
        }
    }
}
