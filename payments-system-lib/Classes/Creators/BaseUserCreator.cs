using payments_system_lib.Classes.Users;

namespace payments_system_lib.Classes.Creators
{
    public abstract class BaseUserCreator
    {
        public abstract BaseUser TryGetFromDb();
        public abstract BaseUser CreateNew();
        public abstract bool CanBeRegistered();
        public abstract bool IsValidArgs();
    }
}
