using ZonkGame.DB.Audit;
using ZonkGame.DB.GameRepository;

namespace ZonkGameCore.Observer
{
    /// <summary>
    /// Логгирование в консоль
    /// </summary>
    public class ConsoleLogger(IAuditWriter auditWriter, IGameRepository repository) : BaseObserver(auditWriter, repository)
    {
        public override void Error(Exception e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
        }

        protected override void Info(string message)
        {
            Console.WriteLine(message);
        }
    }
}
