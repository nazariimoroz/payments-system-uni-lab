using System;
using System.ComponentModel.DataAnnotations;

namespace payments_system_lib.Classes.Users
{
    public abstract class BaseUser
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime RegistrationDate { get; protected set;  }
    }
}
