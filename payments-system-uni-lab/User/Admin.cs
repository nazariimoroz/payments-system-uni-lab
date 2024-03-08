namespace payments_system_uni_lab
{
    public class Admin : BaseUser
    {
        public Admin(string name, string password) : base(name, password) { }

        public override string GetStatus()
        {
            return "Admin";
        }
    }
}