namespace payments_system_uni_lab
{
    public abstract class BaseUser
    {
        protected BaseUser(string name, string password)
        {
            Name = name;
            CachedPassword = password;
        }
        protected BaseUser() : this("none", "") { }

        public virtual string GetStatus()
        {
            return "test";
        }

        public string Name { get; set; }
        public string CachedPassword { get; set; }
    }
}