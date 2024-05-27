using Microsoft.EntityFrameworkCore.Metadata.Internal;
using payments_system_lib.Classes.Users;
using payments_system_lib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using payments_system_lib.Classes.Cards;

namespace payments_system_lib.Classes.Transaction.Creators
{
    public class TransactionCreator : DbAgentCreator<Transaction>
    {
        public int Id { get; set; } = -1;
        public BaseCard Card { get; set; } = null;
        public float Amount { get; set; } = float.NaN;
        public string Info { get; set; } = null;
        public TransactionType Type { get; set; } = (TransactionType)(-1);


        public override Transaction TryGetFromDb()
        {
            if (Id == -1)
                throw new InvalidParamException(nameof(Id));

            using (var db = new ApplicationContext())
            {
                var transaction = db
                    .Transaction
                    .Include(t => t.Card)
                    .FirstOrDefault(t => t.Id == Id);
                return transaction;
            }
        }

        public override Transaction CreateNew()
        {
            if (Type == (TransactionType)(-1))
                throw new InvalidParamException(nameof(Type));
            if (Info == null)
                throw new InvalidParamException(nameof(Info));
            if (float.IsNaN(Amount))
                throw new InvalidParamException(nameof(Amount));
            if (Card == null)
                throw new InvalidParamException(nameof(Card));

            Transaction transaction;

            using (var db = new ApplicationContext())
            {
                var card = db.ClientCard.FirstOrDefault(c => c.Id == Card.Id);
                if(card == null)
                    throw new InvalidParamException(nameof(Card));

                transaction = new Transaction(Type, Info, Amount, DateTime.Now, card);

                db.Transaction.Add(transaction);
                db.SaveChanges();
            }

            return transaction;
        }

        // Use Card and Type
        public override List<T> GetAll<T>()
        {
            using (var db = new ApplicationContext())
            {
                return db
                    .Transaction
                    .Include(t=> t.Card)
                    .Where(t => (Card == null || t.Card == Card) && (Type == (TransactionType)(-1) || t.Type == Type))
                    .Select(t => t as T)
                    .ToList();
            }
        }

        public override void Save(Transaction toSave)
        {
            throw new NotImplementedException();
        }

        public override void Destroy(Transaction toDestroy)
        {
            throw new NotImplementedException();
        }
    }
}
