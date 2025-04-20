using ZonkGameCore.FSM;
using ZonkGameCore.InputHandler;
using ZonkGameCore.Observer;

namespace ZonkGameConsole
{
    public static class ConsoleZonk
    {
        public async static Task Main(params string[] args)
        {
            ZonkStateMachine game = new(new ConsoleLogger());
            game.InitStartGame(2000, [new("First", new ConsoleInputHandler()), new("Second", new ConsoleInputHandler())]);
            while (!game.IsGameOver)
            {
                try
                {
                    await game.Handle();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}