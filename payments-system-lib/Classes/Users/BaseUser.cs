using System;
using System.ComponentModel.DataAnnotations.Schema;
using payments_system_lib.Interfaces;

namespace payments_system_lib.Classes.Users
{
    public abstract class BaseUser : IDbAgent
    {
        public int Id { get; set; }

        [NotMapped]
        public Uri UserMainUi { get; set; } = null;

        public abstract bool SaveToDb();
    }
}
