using System.Collections.Generic;
using System.Linq;
using payments_system_lib.Utilities;

namespace payments_system_lib.Classes.Users.Creators
{
    public abstract class BaseUserCreator 
        : DbAgentCreator<BaseUser>
    {
        public abstract override BaseUser TryGetFromDb();
        public abstract override BaseUser CreateNew();
        public abstract bool CanBeRegistered();
        public abstract bool IsValidArgs();

        public abstract bool DestroyUser(BaseUser toDestroy);
    }
}
