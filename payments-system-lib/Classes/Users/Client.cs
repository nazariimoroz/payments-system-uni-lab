using System;
using System.Collections.Generic;
using System.Linq;
using payments_system_lib.Classes.Cards;
using payments_system_lib.Interfaces;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Users
{
    public class Client : BaseUser, IDbAgent
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string EncryptedPassword { get; set; }
        public List<CreditCard> CreditCards { get; set; } = new List<CreditCard>();


        /*
         * For EF Core and Creator
         */
        public Client(string phoneNumber, string encryptedPassword)
        {
            PhoneNumber = phoneNumber;
            EncryptedPassword = encryptedPassword;

            UserMainUi = new Uri("/UI/Main/ClientMainUI.xaml", UriKind.RelativeOrAbsolute);
        }

        public bool SaveToDb()
        {
            using (var db = new ApplicationContext())
            {
                var client = db.Clients.FirstOrDefault(c => c.Id == Id);
                if (client != null)
                {
                    db.Entry(client).CurrentValues.SetValues(this);
                    db.Update(client);
                    db.SaveChanges();
                    return true;
                }
            }

            return false;
        }
    }
}
