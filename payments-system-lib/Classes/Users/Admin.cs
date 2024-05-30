using System;
using System.ComponentModel.DataAnnotations;

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
    }
}
