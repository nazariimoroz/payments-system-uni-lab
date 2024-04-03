namespace payments_system_uni_lab.Interfaces
{
    public interface IDbAgent
    {
        bool SaveToDb();
        bool ReadFromDb();
    }
}
