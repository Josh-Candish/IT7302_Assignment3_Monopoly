using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public abstract class Game : IGame
    {
        private int _playerCount;

        public abstract bool EndOfGame();
        public abstract void InitialiseGame();
        public abstract void MakePlay(int player);
        public abstract void PrintWinner();

        public void PlayOneGame(int playerCount)
        {
            _playerCount = playerCount;
            InitialiseGame();

            var j = 0;

            while (!EndOfGame())
            {
                MakePlay(j);
                j += 1;
            }

            PrintWinner();
        }
    }
}
