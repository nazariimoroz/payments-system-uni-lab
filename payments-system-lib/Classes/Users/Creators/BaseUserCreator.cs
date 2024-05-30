using System.Threading.Tasks;

namespace payments_system_lib.Classes.Users.Creators
{
    public abstract class BaseUserCreator 
        : DbAgentCreator<BaseUser>
    {
        public abstract override Task<BaseUser> TryGetFromDb();
        public abstract override Task<BaseUser> CreateNew();
        public abstract Task<bool> CanBeRegistered();
        public abstract Task<bool> IsValidArgs();
    }
}
