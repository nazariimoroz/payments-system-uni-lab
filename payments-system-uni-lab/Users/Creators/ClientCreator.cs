using payments_system_uni_lab.Objects;
using payments_system_uni_lab.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace payments_system_uni_lab.Users.Creators
{
    public class ClientCreator : BaseUserCreator
    {
        public override BaseUser TryGetFromDb(BaseUserArgs args)
        {
            if (!(args is ClientArgs cargs))
            {
                return null;
            }
            var phoneNumber = cargs.PhoneNumber;
            var encryptedPassword = Utilities.Utilities.CreateMD5(cargs.RealPassword);

            using (var db = new ApplicationContext())
            {
                var client = db
                    .Clients
                    .FirstOrDefault(c => c.PhoneNumber == phoneNumber && c.EncryptedPassword == encryptedPassword);
                if (client == null)
                {
                    return null;
                }

                client.CreditCards = db.CreditCards
                    .Where(card => card.Client == client)
                    .ToList();

                return client;
            }
        }

        public override BaseUser CreateNew(BaseUserArgs args)
        {
            if (!(args is ClientArgs cargs))
            {
                return null;
            }
            var phoneNumber = cargs.PhoneNumber;
            var encryptedPassword = Utilities.Utilities.CreateMD5(cargs.RealPassword);

            var client = new Client(phoneNumber, encryptedPassword);

            using (var db = new ApplicationContext())
            {
                var query = db.Clients
                    .Where(c => c.PhoneNumber == client.PhoneNumber)
                    .ToList();

                if (query.Count != 0)
                    return null;

                db.Clients.Add(client);
                db.CreditCards.UpdateRange(client.CreditCards);
                db.SaveChanges();
            }

            CreditCard.CreateNew(client);

            return client;
        }

        public override bool CanBeRegistered(BaseUserArgs args)
        {
            if (!IsValidArgs(args))
                return false;

            if (!(args is ClientArgs cargs))
            {
                return false;
            }
            var phoneNumber = cargs.PhoneNumber;

            using (var db = new ApplicationContext())
            {
                var query = db.Clients
                    .FirstOrDefault(c => c.PhoneNumber == phoneNumber);

                return query == null;
            }
        }

        public override bool IsValidArgs(BaseUserArgs args)
        {
            if (!(args is ClientArgs cargs))
            {
                return false;
            }

            if (cargs.RealPassword.Length < 8)
                return false;

            if (!Regex.IsMatch(
                    cargs.PhoneNumber, 
                    "^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$"))
                return false;

            return true;
        }
    }

    public class ClientArgs : BaseUserArgs
    {
        public string PhoneNumber { get; set; }
        public string RealPassword { get; set; }
    }
}
