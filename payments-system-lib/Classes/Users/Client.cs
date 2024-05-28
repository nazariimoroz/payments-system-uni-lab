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
        public List<CreditCard> Cards { get; set; } = new List<CreditCard>();

        /*
         * For EF Core and Creator
         */
        public Client(string phoneNumber, string encryptedPassword, DateTime registrationDate)
        {
            PhoneNumber = phoneNumber;
            EncryptedPassword = encryptedPassword;
            RegistrationDate = registrationDate;
        }
    }
}
