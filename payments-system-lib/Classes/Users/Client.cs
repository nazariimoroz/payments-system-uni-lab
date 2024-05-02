using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using payments_system_lib.Classes.Cards;
using payments_system_lib.Interfaces;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Users
{
    public class Client : BaseUser
    {
        [Required]
        public string PhoneNumber { get; set ; }
        public string EncryptedPassword { get; set; }
        public List<BaseCard> Cards { get; set; } = new List<BaseCard>();

        /*
         * For EF Core and Creator
         */
        public Client(string phoneNumber, string encryptedPassword, DateTime registrationDate)
        {
            PhoneNumber = phoneNumber;
            EncryptedPassword = encryptedPassword;
            RegistrationDate = registrationDate;

            UserMainUi = new Uri("/UI/Main/ClientMainUI.xaml", UriKind.RelativeOrAbsolute);
        }

        public override bool SaveToDb()
        {
            using (var db = new ApplicationContext())
            {
                var client = db.Client.FirstOrDefault(c => c.Id == Id);
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
