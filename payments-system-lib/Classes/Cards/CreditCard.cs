using System;
using System.ComponentModel.DataAnnotations.Schema;
using payments_system_lib.Classes.Cards.Creators;
using payments_system_lib.Classes.Transaction;
using payments_system_lib.Classes.Transaction.Creators;
using payments_system_lib.Classes.Users;

namespace payments_system_lib.Classes.Cards
{
    public class CreditCard
    {
        public int Id { get; set; }
        public string Num { get; protected set; }
        public string Cvc { get; protected set; }
        public float ClientMoney { get; protected set; }
        public float CreditLimit { get; set; }
        public DateTime ExpiresEnd { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; protected set; }

        [NotMapped]
        public float AllMoney
        {
            get => ClientMoney + CreditLimit;
            set
            {
                if (value < 0)
                    throw new WithdrawFromCardError(AllMoney, value - AllMoney);
                ClientMoney = value - CreditLimit;
            }
        }

        /*
         * For EC Core
         */
        public CreditCard(string num, string cvc, float clientMoney, float creditLimit, DateTime expiresEnd)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            CreditLimit = creditLimit;
            ExpiresEnd = expiresEnd;
        }

        public CreditCard(string num, string cvc, float clientMoney, float creditLimit, DateTime expiresEnd, Client client)
        {
            Num = num;
            Cvc = cvc;
            ClientMoney = clientMoney;
            CreditLimit = creditLimit;
            ExpiresEnd = expiresEnd;
            Client = client;
        }

        public virtual bool SendMoneyToOtherCard(SendInfo info, out CreditCard receiver)
        {
            receiver = new CreditCardCreator { Num = info.NumOfReceiver }.TryGetFromDb();
            var amount = info.Amount;
            if (receiver == null)
                return false;
            if (amount <= 0.0F)
                return false;
            
            try
            {
                AllMoney -= amount;
            } catch (WithdrawFromCardError e)
            {
                Console.WriteLine(e);
                return false;
            }

            receiver.AllMoney += amount;

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
            AllMoney += info.Amount;

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

    class WithdrawFromCardError : Exception
    {
        public WithdrawFromCardError(float currentMoney, float tryToWithdraw)
            : base($"{currentMoney} < {-tryToWithdraw}")
        {

        }
    }
}