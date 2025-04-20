using System.Reflection;
using ZonkGameCore.FSM;
using ZonkGameCore.FSM.States;

namespace ZonkGameCore.Utils
{
    public static class StateNameExtension
    {
        public static string GetStateName<T>(this T state) where T : BaseGameState
        {
            if (state is IStateName)
            { 
                return state.GetType().GetProperty(nameof(IStateName.StateName))?.GetValue(state)?.ToString() ?? string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Получает состояние по имени
        /// </summary>
        public static BaseGameState GetStateByName(this ZonkStateMachine fsm, string stateName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Фильтруем типы, реализующие интерфейс IStateName
            var types = assembly.GetTypes()
                .Where(t => typeof(IStateName).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var type in types)
            {
                // Находим статическое свойство StateName
                PropertyInfo stateNameProperty = type.GetProperty("StateName", BindingFlags.Public | BindingFlags.Static) ??
                    throw new ArgumentNullException("Имя StateName не обнаружено");

                if (stateNameProperty != null && stateNameProperty.PropertyType == typeof(string))
                {
                    // Получаем значение статического свойства
                    if ((string?)stateNameProperty.GetValue(null) == stateName)
                    {
                        // Создаем экземпляр состояния
                        return (BaseGameState?)Activator.CreateInstance(type, fsm.Observer, fsm) ??
                            throw new InvalidCastException("Невозможно создать состояние");
                    }
                }
            }

            throw new ArgumentException($"Состояние с именем {stateName} не найдено");
        }
    }
}
