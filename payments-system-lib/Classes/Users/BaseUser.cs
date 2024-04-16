using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace payments_system_lib.Classes.Users
{
    public abstract class BaseUser
    {
        [NotMapped]
        public Uri UserMainUi { get; set; } = null;
    }
}
