using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZonkGameApi.Utils
{
    /// <summary>
    /// Тип игрока
    /// </summary>
    public enum PlayerTypeEnum
    {
        /// <summary> Игрок - человек</summary>
        RealPlayer,
        /// <summary> Игрок - ИИ</summary>
        AIAgent
    }
}
