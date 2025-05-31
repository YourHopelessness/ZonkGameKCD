namespace ZonkGame.Auth.Models
{
    /// <summary>
    /// Модель для регистрации нового пользователя
    /// </summary>
    public class RegisterRequestModel
    {
        /// <summary> Имя пользователя для регистрации </summary>
        public string Username { get; set; } = null!;

        /// <summary> Электронная почта пользователя для регистрации </summary>
        public string Email { get; set; } = null!;

        /// <summary> Пароль пользователя для регистрации </summary>
        public string Password { get; set; } = null!;
    }
}
