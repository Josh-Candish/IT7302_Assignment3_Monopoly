
namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public interface IGame
    {
        bool EndOfGame();
        void InitialiseGame();
        void MakePlay(int player);
        void PrintWinner();
    }
}
