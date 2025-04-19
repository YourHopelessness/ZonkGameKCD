namespace ZonkGameCore.Observer
{
    /// <summary>
    /// Логгирование в консоль
    /// </summary>
    public class ConsoleLogger : BaseObserver
    {
        public override void Error(string text)
        {
            Console.Error.WriteLine(text);
        }

        public override void Info(string text)
        {
            Console.WriteLine(text);
        }
    }
}
