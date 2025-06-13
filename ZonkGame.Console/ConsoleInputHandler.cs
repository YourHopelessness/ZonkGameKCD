using ZonkGameCore.InputParams;

namespace ZonkGameCore.InputHandler
{
    public class ConsoleInputHandler : IInputAsyncHandler
    {
        /// <summary>
        /// Reads user input for selecting dice from the console.
        /// </summary>
        /// <param name="roll">Current dice roll</param>
        /// <returns>Indices of selected dice</returns>
        public Task<IEnumerable<int>> HandleSelectDiceInputAsync(IEnumerable<int> roll)
        {
            return Task.FromResult(GetHandleSelectDiceInput(roll));
        }

        /// <summary>
        /// Asks the user whether to continue the game.
        /// </summary>
        /// <returns>User decision</returns>
        public Task<bool> HandleShouldContinueGameInputAsync()
        {
            return Task.FromResult(GetHandleShouldContinueGameInput());
        }

        private bool GetHandleShouldContinueGameInput()
        {
            while (true)
            {
                Console.WriteLine("Want to continue the game? (y/n)");
                var input = Console.ReadLine();
                if (input == null)
                {
                    Console.WriteLine("Your answer should not be empty");
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
                Console.WriteLine("Select the bones that you want to postpone (through a comma):");
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("You must postpone at least one bone");
                }
                else
                {
                    selectedDices = [.. input.Split(',', ' ', '.').Select(d => int.Parse(d.Trim()))];
                    if (selectedDices.Count == 0)
                    {
                        Console.WriteLine("You must postpone at least one bone");
                    }
                    else if (selectedDices.Any(x => x < 1 || x > 6))
                    {
                        Console.WriteLine("Bones can only be from 1 to 6");
                    }
                    else if (!selectedDices.ToHashSet().IsSubsetOf(roll.ToHashSet()))
                    {
                        Console.WriteLine("Selected bones do not coincide with abandoned bones");
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return selectedDices;
        }

        /// <inheritdoc />
        public Task<IEnumerable<int>> HandleSelectDiceInputAsync(IEnumerable<int> roll, Guid gameid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> HandleShouldContinueGameInputAsync(Guid gameid)
        {
            throw new NotImplementedException();
        }
    }
}
