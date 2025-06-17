namespace ZonkGame.DB.Exceptions
{
    /// <summary>
    /// The exception thrown out in the absence of an essence in the database
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public string Description { get; set; }

        public EntityNotFoundException(string entityName, Dictionary<string, string?> parametrs) : base()
        {
            Description = $"Entity {entityName} with parameter" 
                + (parametrs.Count < 2 ? " " : "s ")
                + $"{string.Join(", ", parametrs.Select(x => $"{x.Key} = {x.Value}"))}"
                + " not found";
        }
    }
}
