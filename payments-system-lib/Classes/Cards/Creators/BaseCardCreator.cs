using payments_system_lib.Classes.Users;

namespace payments_system_lib.Classes.Cards.Creators
{
    public abstract class BaseCardCreator : DbAgentCreator<BaseCard>
    {
        public abstract override BaseCard TryGetFromDb();
        public abstract override BaseCard CreateNew();
    }
}