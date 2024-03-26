using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using payments_system_uni_lab.Objects;

namespace payments_system_uni_lab.Persons
{
    class Client
    {
        public CreditCard CreditCard { get; private set; }

        public Client()
        {
            CreditCard = new CreditCard(this);
        }
    }
}
