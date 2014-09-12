
namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    interface IGame
    {
        bool EndOfGame();
        void InitialiseGame();
        void MakePlay(int player);
        void PlayOneGame(int playerCount);
        void PrintWinner();
    }
}
