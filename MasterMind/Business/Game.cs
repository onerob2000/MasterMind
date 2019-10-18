using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using MasterMind.Interfaces;

namespace MasterMind.Business
{
    public class Game
    {
        #region Private Members

        public bool IsOver { get; set; }
        private const int MaxFullNumber = 6666;
        private const int MinFullNumber = 1111;
        private const int MaxIndividualNumber = 6;
        private const int MinIndividualNumber = 1;

        private readonly IMasterMindHelper _masterMindHelper;

        #endregion Private Members

        public Game(IMasterMindHelper masterMindHelper)
        {
            _masterMindHelper = masterMindHelper;
        }

        #region Public Methods

        public void Start()
        {
            PrintWelcomeMessageAndRulesToScreen();

            Thread.Sleep(2000);

            var playerGuesses = 10;
            var secretAnswer = GenerateSecretAnswer();

            var winState = false;

            while (playerGuesses > 0)
            {
                if (playerGuesses != 10)
                {
                    Console.WriteLine($"Guesses Remaining: {playerGuesses}");
                }

                Console.WriteLine($"{Environment.NewLine}Make your guess:{Environment.NewLine}");

                var playerGuess = Console.ReadLine();

                var validated = ValidatePlayersGuess(playerGuess);

                if (!validated)
                {
                    playerGuesses--;
                    continue;
                }

                var intUserGuess = int.Parse(playerGuess);

                if (intUserGuess == secretAnswer) //Game has been won.
                {
                    winState = true;
                    break;
                }

                var inPlaceCount = GetCountOfDigitsCorrectlyInPlace(intUserGuess.ToString(), secretAnswer.ToString());
                var outPlaceDigitCount = GetCountOfDigitsCorrectButNotInPlace(intUserGuess.ToString(), secretAnswer.ToString());

                var strFeedback = $"{Environment.NewLine}Score: ";

                //Switch statement builds feedback string.
                #region Switch statement w/cases

                switch (inPlaceCount)
                {
                    case 0:
                        break;
                    case 1:
                        strFeedback += "+";
                        break;
                    case 2:
                        strFeedback += "++";
                        break;
                    case 3:
                        strFeedback += "+++";
                        break;
                }

                switch (4 - inPlaceCount - outPlaceDigitCount)  // out place digits are digits which are not existing and in place are digits which are in correct place
                {
                    case 0:
                        break;
                    case 1:
                        strFeedback += "-";
                        break;
                    case 2:
                        strFeedback += "--";
                        break;
                    case 3:
                        strFeedback += "---";
                        break;
                    case 4:
                        strFeedback += "----";
                        break;
                }
                #endregion

                Console.WriteLine($"{strFeedback}{Environment.NewLine}");
                Console.WriteLine($"{new string('-', 10)}{Environment.NewLine}");
                playerGuesses--;
            }
            if (winState)
            {
                Console.WriteLine($"{new string('-', 10)}{Environment.NewLine}");
                Console.WriteLine($"{Environment.NewLine}You solved it!");
            }
            else
            {
                Console.WriteLine($"{Environment.NewLine}You lose. :({Environment.NewLine}");
                Console.WriteLine($"The code was {secretAnswer}");
            }
            Console.ReadLine();
        }

        #endregion Public Methods
    }
}