using payments_system_uni_lab.Objects;
using payments_system_uni_lab.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var encryptedPassword = cargs.EncryptedPassword;

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

        public override BaseUser CreateNew(BaseUserArgs args)
        {
            if (!(args is ClientArgs cargs))
            {
                return null;
            }
            var phoneNumber = cargs.PhoneNumber;
            var encryptedPassword = cargs.EncryptedPassword;

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

        public override bool IsValidArgs(BaseUserArgs args)
        {
            if (!(args is ClientArgs cargs))
            {
                return false;
            }

            if (cargs.RealPassword.Length < 8)
                return false;

            using (var db = new ApplicationContext())
            {
                var client = db
                    .Clients
                    .FirstOrDefault(c => c.PhoneNumber == cargs.PhoneNumber);

                return client == null;
            }
        }
    }

    public class ClientArgs : BaseUserArgs
    {
        public string PhoneNumber { get; set; }
        public string RealPassword { get; set; }
        public string EncryptedPassword { get; set; }
    }
}
