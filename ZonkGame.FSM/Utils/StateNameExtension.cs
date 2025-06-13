using System.Reflection;
using ZonkGameCore.FSM;
using ZonkGameCore.FSM.States;

namespace ZonkGameCore.Utils
{
    public static class StateNameExtension
    {
        public static string GetStateName<T>(this T state) where T : BaseGameState
        {
            if (state is BaseGameState)
            { 
                var statename = state.GetType().ToString();
                return statename;
            }

            return string.Empty;
        }

        /// <summary>
        /// Receives a condition by name
        /// </summary>
        public static BaseGameState GetStateByName(this ZonkStateMachine fsm, string stateName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            var state = (BaseGameState?)assembly
                .GetTypes()
                .Where(t => typeof(BaseGameState)
                .IsAssignableFrom(t) && !t.IsAbstract)
                .Where(x => x.FullName == stateName)
                .Select(x => Activator.CreateInstance(x, fsm.Observer, fsm))
                .FirstOrDefault();

            return state ?? throw new ArgumentException($"State named {statename} was not found");
        }
    }
}
