namespace ZonkGame.DB.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое при отсутствии сущности в базе данных
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public string Description { get; set; }

        public EntityNotFoundException(string entityName, Dictionary<string, string> parametrs) : base()
        {
            Description = $"Сущность {entityName} с параметр" 
                + (parametrs.Count < 2 ? "ом " : "ами ")
                + $"{string.Join(", ", parametrs.Select(x => $"{x.Value} = {x.Value}"))}";
        }
    }
}
