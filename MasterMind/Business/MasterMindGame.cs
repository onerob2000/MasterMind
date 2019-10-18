using System;
using System.Threading;
using MasterMind.Helpers;
using MasterMind.Interfaces;

namespace MasterMind.Business
{
    public class MasterMindGame
    {
        #region Private members

        private readonly IMasterMindHelper _masterMindHelper;

        private const int NumberOfPlayerTurns = 10;

        #endregion Private members

        #region Constructor

        public MasterMindGame()
        {
            _masterMindHelper = new MasterMindHelper();
        }

        #endregion Constructor

        #region Private Methods

        public void PlayGame()
        {
            var playGame = true;

            while (playGame)
            {
                _masterMindHelper.PrintWelcomeMessageAndRulesToScreen();

                var playerTurns = NumberOfPlayerTurns;

                var secretAnswer = _masterMindHelper.GenerateSecretAnswer();

                var playerWon = false;

                while (playerTurns > 0)
                {
                    if (playerTurns != 10)
                    {
                        Console.WriteLine($"Guesses Remaining: {playerTurns}");
                    }

                    var playerInput = _masterMindHelper.GetValidInput();

                    playerWon = _masterMindHelper.CheckForWin(playerInput, secretAnswer);

                    if (playerWon) //Game has been won.
                    {
                        break;
                    }

                    var score = _masterMindHelper.GetScore(playerInput, secretAnswer);

                    _masterMindHelper.WriteScore(score);

                    playerTurns--;
                }

                _masterMindHelper.InformPlayerOfEndResult(playerWon, secretAnswer);

                Console.WriteLine("Would you like to play again? (Press Y for Yes or any other key to exit.)");

                if (Console.ReadKey().Key != ConsoleKey.Y)
                {
                    playGame = false;
                }

                Console.Clear();
            }
        }

        #endregion Private Methods

    }
}