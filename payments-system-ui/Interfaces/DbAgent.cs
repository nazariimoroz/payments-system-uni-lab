namespace payments_system_ui.Interfaces
{
    public interface IDbAgent
    {
        int Id { get; }
        bool SaveToDb();
    }
}
