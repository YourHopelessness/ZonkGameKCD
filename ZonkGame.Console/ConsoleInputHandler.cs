using ZonkGameCore.InputParams;

namespace ZonkGameCore.InputHandler
{
    public class ConsoleInputHandler : IInputAsyncHandler
    {
        public Task<IEnumerable<int>> HandleSelectDiceInputAsync(IEnumerable<int> roll)
        {
            return Task.FromResult(GetHandleSelectDiceInput(roll));
        }

        public Task<bool> HandleShouldContinueGameInputAsync()
        {
            return Task.FromResult(GetHandleShouldContinueGameInput());
        }

        private bool GetHandleShouldContinueGameInput()
        {
            while (true)
            {
                Console.WriteLine("Хотите продолжить игру? (y/n)");
                var input = Console.ReadLine();
                if (input == null)
                {
                    Console.WriteLine("Ваш ответ не должен быть пустым");
                }
                else if (input == "y")
                {
                    return true;
                }
                else if (input == "n")
                {
                    return false;
                }
            }
        }

        private IEnumerable<int> GetHandleSelectDiceInput(IEnumerable<int> roll)
        {
            var selectedDices = new List<int>();
            while (true)
            {
                Console.WriteLine("Выберите кости, которые хотите отложить (через запятую):");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Вы должны отложить хотя бы одну кость");
                }
                else
                {
                    selectedDices = [.. input.Split(',', ' ', '.').Select(d => int.Parse(d.Trim()))];
                    if (selectedDices.Count == 0)
                    {
                        Console.WriteLine("Вы должны отложить хотя бы одну кость");
                    }
                    else if (selectedDices.Any(x => x < 1 || x > 6))
                    {
                        Console.WriteLine("Кости могут быть только от 1 до 6");
                    }
                    else if (!selectedDices.ToHashSet().IsSubsetOf(roll.ToHashSet()))
                    {
                        Console.WriteLine("Выбранные кости не совпадают с брошенными костями");
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return selectedDices;
        }
    }
}
