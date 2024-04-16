namespace payments_system_ui.Users.Creators
{
    public abstract class BaseUserCreator
    {
        public abstract BaseUser TryGetFromDb(BaseUserArgs args);

        public abstract BaseUser CreateNew(BaseUserArgs args);

        public abstract bool CanBeRegistered(BaseUserArgs args);

        public abstract bool IsValidArgs(BaseUserArgs args);
    }

    public abstract class BaseUserArgs
    {
    }
}
