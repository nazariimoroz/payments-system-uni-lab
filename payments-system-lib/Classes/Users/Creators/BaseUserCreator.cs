using System.Linq;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Users.Creators
{
    public abstract class BaseUserCreator
    {
        public abstract BaseUser TryGetFromDb();
        public abstract BaseUser CreateNew();
        public abstract bool CanBeRegistered();
        public abstract bool IsValidArgs();

        public abstract bool DestroyUser(BaseUser toDestroy);
    }
}
