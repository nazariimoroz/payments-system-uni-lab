using payments_system_lib.Classes.Cards;
using payments_system_lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace payments_system_lib.Classes.Users
{
    public class Admin : BaseUser
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string EncryptedPassword { get; set; }

        public Admin(string key, string encryptedPassword, DateTime registrationDate)
        {
            Key = key;
            EncryptedPassword = encryptedPassword;
            RegistrationDate = registrationDate;
        }

        public override bool SaveToDb()
        {
            using (var db = new ApplicationContext())
            {
                var admin = db.Admin.FirstOrDefault(a => a.Id == Id);
                if (admin != null)
                {
                    db.Entry(admin).CurrentValues.SetValues(this);
                    db.Update(admin);
                    db.SaveChanges();
                    return true;
                }
            }

            return false;
        }
    }
}
