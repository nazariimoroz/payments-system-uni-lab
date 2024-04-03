using System;
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
