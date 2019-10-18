using MasterMind.Business;

namespace MasterMind
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var game = new MasterMindGame();

            game.PlayGame();
        }
    }
}