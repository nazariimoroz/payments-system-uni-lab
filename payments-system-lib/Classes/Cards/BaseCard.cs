using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Tls;
using payments_system_lib.Classes.Cards.Creators;
using payments_system_lib.Classes.Transaction;
using payments_system_lib.Classes.Transaction.Creators;
using payments_system_lib.Classes.Users;
using payments_system_lib.Interfaces;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Cards
{
    public class BaseCard
    {
        public int Id { get; set; }
        public string Num { get; protected set; }
        public string Cvc { get; protected set; }
        public float ClientMoney { get; protected set; }
        public float CreditLimit { get; set; }
        public DateTime ExpiresEnd { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; protected set; }

        /*
         * For EC Core
         */
        protected BaseCard(string num, string cvc, float clientMoney, DateTime expiresEnd)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            ExpiresEnd = expiresEnd;

            CreditLimit = 0;
        }

        protected BaseCard(string num, string cvc, float clientMoney, DateTime expiresEnd, Client client)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            ExpiresEnd = expiresEnd;
            Client = client;

            CreditLimit = 0;
        }

        public virtual bool SendMoneyToOtherCard(SendInfo info, out BaseCard receiver)
        {
            receiver = new BaseCardCreator { Num = info.NumOfReceiver }.TryGetFromDb();
            var amount = info.Amount;
            if (receiver == null)
                return false;

            if (amount <= 0.0F || amount > ClientMoney)
                return false;

            ClientMoney -= amount;
            receiver.ClientMoney += amount;

            var transactionInfo = $"Receive {info.Amount}$ from '{Num}'";
            new TransactionCreator { Type = TransactionType.Receiver, Amount = info.Amount, Card = receiver, Info = transactionInfo }.CreateNew();

            transactionInfo = $"Send {info.Amount}$ to '{receiver.Num}'";
            new TransactionCreator { Type = TransactionType.Send, Amount = info.Amount, Card = this, Info = transactionInfo }.CreateNew();

            return true;
        }

        public virtual bool ReplenishFromSource(ReplenishInfo info)
        {
            if (info.Amount <= 0.0F)
                return false;
            ClientMoney += info.Amount;

            var transactionInfo = $"Receive {info.Amount}$ from '{info.ReplenishSource.ToString()}' source";
            new TransactionCreator{ Type = TransactionType.Receiver, Amount = info.Amount, Card = this, Info = transactionInfo }.CreateNew();
            return true;
        }
    }

    public struct ReplenishInfo
    {
        public float Amount { get; set; }
        public ReplenishSource ReplenishSource { get; set; }
    }

    public enum ReplenishSource
    {
        Debug, ApplePay
    }

    public struct SendInfo
    {
        public float Amount { get; set; }
        public string NumOfReceiver { get; set; }
    }
}