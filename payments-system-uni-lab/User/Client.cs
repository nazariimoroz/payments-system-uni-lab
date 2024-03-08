namespace payments_system_uni_lab
{
    public class Client : BaseUser
    {
        public Client(string name, string password) : base(name, password) { }

        public override string GetStatus()
        {
            return "Client";
        }
    }
}