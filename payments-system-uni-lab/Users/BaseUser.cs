using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using payments_system_uni_lab.Interfaces;

namespace payments_system_uni_lab.Users
{
    public abstract class BaseUser
    {
        [NotMapped]
        public Uri UserMainUi { get; set; } = null;
    }
}
