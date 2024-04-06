namespace payments_system_uni_lab.Interfaces
{
    public interface IDbAgent
    {
        int Id { get; }
        bool SaveToDb();
    }
}
