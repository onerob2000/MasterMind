namespace MasterMind.Interfaces
{
    public interface IMasterMindHelper
    {
        void PrintWelcomeMessageAndRulesToScreen();

        void WriteScore(string score);

        int GenerateSecretAnswer();

        string GetScore(int playerInput, int secretAnswer);

        int GetValidInput();

        bool CheckForWin(int input, int secretAnswer);

        void InformPlayerOfEndResult(bool playerWon, int secretAnswer);

    }
}