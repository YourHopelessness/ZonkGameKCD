namespace ZonkGameApi.Interfaces
{
    public interface ISessionHandler
    {
        Task CreateSession();

        Task CloseSession();
    }
}
