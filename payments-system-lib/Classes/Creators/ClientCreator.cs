using System.Linq;
using System.Text.RegularExpressions;
using payments_system_lib.Classes.Cards;
using payments_system_lib.Classes.Users;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Creators
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
                    .FirstOrDefault(c => c.PhoneNumber == PhoneNumber && c.EncryptedPassword == encryptedPassword);
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
                db.CreditCards.UpdateRange(client.CreditCards);
                db.SaveChanges();
            }

            CreditCard.CreateNew(client);

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
    }
}
