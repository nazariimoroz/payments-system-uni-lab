using System;
using System.Collections.Generic;
using System.Windows.Documents;
using payments_system_uni_lab.Objects;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using payments_system_uni_lab.Interfaces;
using payments_system_uni_lab.Utilities;

namespace payments_system_uni_lab.Users
{
    public class Client : BaseUser, IDbAgent
    {
        public int Id { get; set; }
        public string PhoneNumber { get; protected set; }
        public string EncryptedPassword { get; protected set; }
        public List<CreditCard> CreditCards { get; protected set; } = new List<CreditCard>();


        /*
         * For EF Core and Creator
         */
        public Client(string phoneNumber, string encryptedPassword)
        {
            PhoneNumber = phoneNumber;
            EncryptedPassword = encryptedPassword;
        }

        public bool SaveToDb()
        {
            using (var db = new ApplicationContext())
            {
                var client = db.Clients.FirstOrDefault(c => c == this);
                if (client != null)
                {
                    db.Update(client);
                    db.SaveChanges();
                    return true;
                }
            }

            return false;
        }
    }
}
