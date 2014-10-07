namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public abstract class Game : IGame
    {
        private int _playerCount;

        public abstract bool EndOfGame();
        public abstract void InitialiseGame();
        public abstract void MakePlay(int player);
        public abstract void PrintWinner();
    }
}
