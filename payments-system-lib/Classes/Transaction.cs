using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using payments_system_lib.Classes.Cards;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes
{
    public class Transaction
    {
        public int Id { get; protected set; }

        [ForeignKey("ClientCardReceiveId")]
        public BaseCard ClientCardReceiver { get; protected set; }

        protected int? ClientCardSenderId { get; set; }
        [ForeignKey("ClientCardSenderId")]
        public BaseCard ClientCardSender { get; protected set; }

        public string SourceInfo { get; protected set; }

        public float Money { get; protected set; }

        public Transaction(BaseCard cardReceiver, BaseCard cardSender, float money)
        {
            ClientCardReceiver = cardReceiver;
            ClientCardSender = cardSender;
            SourceInfo = "CreditCard";
            Money = money;
        }
        public Transaction(BaseCard cardReceiver, string sourceInfo, float money)
        {
            ClientCardReceiver = cardReceiver;
            ClientCardSender = null;
            SourceInfo = sourceInfo;
            Money = money;
        }
    }
}
