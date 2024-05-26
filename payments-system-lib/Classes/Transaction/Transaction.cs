using payments_system_lib.Classes.Cards;

namespace payments_system_lib.Classes.Transaction
{
    public class Transaction
    {
        public int Id { get; protected set; }

        public BaseCard Card  { get; protected set; }
        public TransactionType Type { get; protected set; }

        public string Info { get; protected set; }

        public float Amount { get; protected set; }

        public Transaction(TransactionType type, string info, float amount)
        {
            Type = type;
            Info = info;
            Amount = amount;
        }

        public Transaction(TransactionType type, string info, float amount, BaseCard card)
        {
            Type = type;
            Info = info;
            Amount = amount;
            Card = card;
        }
    }

    public enum TransactionType
    {
        Send, Receiver
    }
}
