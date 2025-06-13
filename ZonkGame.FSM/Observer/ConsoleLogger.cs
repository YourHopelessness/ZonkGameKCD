using ZonkGame.DB.GameRepository.Interfaces;
using ZonkGame.DB.Repositories.Interfaces;

namespace ZonkGameCore.Observer
{
    /// <summary>
    /// Logging to the console
    /// </summary>
    public class ConsoleLogger(IAuditWriter auditWriter, IGameRepository repository) : BaseObserver(auditWriter, repository)
    {
        public override void Error(Exception e)
        {
            Console.WriteLine($"Error: {E.Message}");
        }

        protected override void Info(string message)
        {
            Console.WriteLine(message);
        }
    }
}
