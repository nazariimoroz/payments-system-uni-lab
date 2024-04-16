using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace payments_system_ui.Users
{
    public abstract class BaseUser
    {
        [NotMapped]
        public Uri UserMainUi { get; set; } = null;
    }
}
