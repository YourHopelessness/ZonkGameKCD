namespace ZonkGameCore.Observer
{
    /// <summary>
    /// Базовый класс для логгирования
    /// </summary>
    public abstract class BaseObserver
    {
        /// <summary>
        /// Логгирование информации
        /// </summary>
        /// <param name="text">Сообщение</param>
        public abstract void Info(string text);

        /// <summary>
        /// Логгирование ошибки
        /// </summary>
        /// <param name="text">Ошибка</param>
        public abstract void Error(string text);
    }
}
