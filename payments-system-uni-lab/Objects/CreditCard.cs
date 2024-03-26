using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using payments_system_uni_lab.Persons;

namespace payments_system_uni_lab.Objects
{
    class CreditCard
    {
        public UInt64 Num { get; private set; }
        public UInt16 Cvc { get; private set; }
        public UInt64 ClientMoney { get; private set; }
        public UInt64 CreditLimit { get; private set; }
        public Client Client { get; private set; }

        public CreditCard(Client cardClient)
        {
            // take from db
            Num = 1234123412341234;
            Cvc = 123;
            ClientMoney = 333;
            CreditLimit = 444;
            Client = cardClient;
        }
    }
}
