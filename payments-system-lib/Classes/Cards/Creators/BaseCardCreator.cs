using payments_system_lib.Classes.Users;

namespace payments_system_lib.Classes.Cards.Creators
{
    public abstract class BaseCardCreator
    {
        public abstract BaseCard TryGetFromDb();

        public abstract BaseCard CreateNew();
    }
}