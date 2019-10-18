using System;
using System.Linq;
using System.Threading;
using MasterMind.Interfaces;

namespace MasterMind.Helpers
{
    public class MasterMindHelper : IMasterMindHelper
    {
        #region Private Members

        private const int MaxFullNumber = 6666;
        private const int MinFullNumber = 1111;
        private const int MaxIndividualNumber = 6;
        private const int MinIndividualNumber = 1;

        #endregion Private Members

        #region Private Methods

        private static bool ValidatePlayersInput(string playerInput)
        {
            try
            {
                var convertToIntSuccess = int.TryParse(playerInput, out var intPlayerInput);

                // validate player entered a number
                if (!convertToIntSuccess)
                {
                    throw new Exception("Guess must be a number.");
                }

                // validate player entered a number between or equal to the minimum and maximum allowed
                if (intPlayerInput < MinFullNumber || intPlayerInput > MaxFullNumber)
                {
                    throw new Exception($"Guess must be greater than or equal to {MinFullNumber} and less than or equal to {MaxFullNumber}.");
                }

                // validate the player didn't enter a zero
                if (playerInput.Contains("0"))
                {
                    throw new Exception($"Guess must not contain a zero.");
                }

                // validate the player entered no repeating digits
                if (playerInput.Distinct().Count() != playerInput.Length)
                {
                    throw new Exception("Guess must not contain any repeating digits.");
                }
                
                // validate the player didn't enter an individual number greater than the maximum individual number allowed
                var playerInputIndividualCharacters = playerInput.ToCharArray();
                if (playerInputIndividualCharacters.Any(playerInputIndividualCharacter => int.Parse(playerInputIndividualCharacter.ToString()) > MaxIndividualNumber))
                {
                    throw new Exception($"Guess must not contain a digit greater than {MaxIndividualNumber}.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }

        private static int GetCountOfDigitsCorrectlyInPlace(string playerInput, string secretCode)
        {
            var correctlyInPlaceCount = 0;

            for (var i = 0; i < 4; i++)
            {
                if (playerInput[i] == secretCode[i])
                {
                    correctlyInPlaceCount++;
                }
            }

            return correctlyInPlaceCount;
        }

        private static int GetCountOfDigitsCorrectButNotInPlace(string playerInput, string secretCode)
        {
            var correctButNotInPlace = 0;

            for (var i = 0; i < 4; i++)
            {
                if (playerInput[i] != secretCode[i] && !secretCode.Contains(playerInput[i]))
                {
                    correctButNotInPlace++;
                }
            }

            return correctButNotInPlace;
        }

        #endregion Private Methods

        #region IMasterMindHelper Implementation

        /// <summary>
        /// Calculates and returns the score based on player input and secret answer
        /// </summary>
        /// <param name="playerInput"></param>
        /// <param name="secretAnswer"></param>
        /// <returns></returns>
        public string GetScore(int playerInput, int secretAnswer)
        {
            var score = $"{Environment.NewLine}Score: ";

            var digitsCorrectlyInPlace = GetCountOfDigitsCorrectlyInPlace(playerInput.ToString(), secretAnswer.ToString());
            var digitsCorrectButNotInPlace = GetCountOfDigitsCorrectButNotInPlace(playerInput.ToString(), secretAnswer.ToString());

            score += new string('+', digitsCorrectlyInPlace);
            score += new string('-', (4 - digitsCorrectlyInPlace - digitsCorrectButNotInPlace));

            return score;
        }

        /// <summary>
        /// Writes the score to the console
        /// </summary>
        /// <param name="score"></param>
        public void WriteScore(string score)
        {
            Console.WriteLine($"{score}");
            Console.WriteLine($"{new string('-', 10)}");
        }

        /// <summary>
        /// Prompts the player for input and validates their input
        /// </summary>
        /// <returns>Integer containing valid player input</returns>
        public int GetValidInput()
        {
        TryAgain:
            Console.WriteLine($"{Environment.NewLine}Make your guess:{Environment.NewLine}");

            var playerInputString = Console.ReadLine() + string.Empty;

            var validated = ValidatePlayersInput(playerInputString);

            if (!validated)
            {
                // Reason for failed validation already presented to the player in ValidatePlayersInput above
                goto TryAgain;
            }

            return int.Parse(playerInputString);
        }

        /// <summary>
        /// Compares the player's input against the secret answer
        /// </summary>
        /// <param name="input"></param>
        /// <param name="secretAnswer"></param>
        /// <returns>Boolean indicating whether the player won or not</returns>
        public bool CheckForWin(int input, int secretAnswer)
        {
            return input == secretAnswer;
        }

        /// <summary>
        /// Writes the end result of the game (win/lose) to the console.
        /// If the player lost, also display the secret answer.
        /// </summary>
        /// <param name="playerWon"></param>
        /// <param name="secretAnswer"></param>
        public void InformPlayerOfEndResult(bool playerWon, int secretAnswer)
        {
            if (playerWon)
            {
                Console.WriteLine($"{new string('-', 10)}{Environment.NewLine}");
                Console.WriteLine($"{Environment.NewLine}You solved it!");
            }
            else
            {
                Console.WriteLine($"{Environment.NewLine}You lose. :({Environment.NewLine}");
                Console.WriteLine($"The code was {secretAnswer}");
            }
        }

        /// <summary>
        /// Generates Secret Answer
        /// </summary>
        /// <returns></returns>
        public int GenerateSecretAnswer()
        {
            var random = new Random();
            var secretString = string.Empty;

            for (var i = 0; i < 4; i++)
            {
            TryAgain:
                var randomNum = random.Next(MinIndividualNumber, MaxIndividualNumber + 1);

                if (secretString.Contains(randomNum.ToString()))
                {
                    // ensure this random number hasn't already been chosen (NOTE: wasn't sure if this should be the case, but coded for it anyway)
                    goto TryAgain;
                }

                secretString += randomNum;
            }

            return int.Parse(secretString);
        }

        /// <summary>
        /// Writes a welcome message and the rules of the game to the console.
        /// </summary>
        public void PrintWelcomeMessageAndRulesToScreen()
        {
            Console.WriteLine("Welcome to Mastermind!");
            Console.WriteLine("You have 10 chances to guess the answer correctly.");
            Console.WriteLine();
            Console.WriteLine("Rules:");
            Console.WriteLine("   1) Each guess must be four (4) digits in length, with each digit ranging from 1 to 6.");
            Console.WriteLine("   2) A minus (-) sign will be printed for every digit that is correct but in the wrong position.");
            Console.WriteLine("   3) A plus (+) sign will be printed for every digit that is both correct and in the correct position.");
            Console.WriteLine("   4) Plus signs will be printed first, minus signs second, and nothing for incorrect digits.");
            Console.WriteLine();
            Console.WriteLine("Enjoy!");
            Console.WriteLine();
            Console.WriteLine();

            Thread.Sleep(1000);
        }

        #endregion IMasterMind Implementation
    }
}