using System;
using System.Collections;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Board
    {
        private static Board _board;
        private ArrayList Properties;
        private ArrayList Players;
        private int Squares = 40;
        private bool _gameOver;

        // Singleton method to access
        public static Board Access()
        {
            return _board ?? (_board = new Board());
        }

        public Board()
        {
            Properties = new ArrayList(GetSquares());
            Players = new ArrayList();
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void AddProperty(Property property)
        {
            Properties.Add(property);
        }

        #region Getters/Setters

        public int GetSquares()
        {
            return Squares;
        }

        public Property GetProperty(int propertyIndex)
        {
            return (Property)Properties[propertyIndex];
        }

        public ArrayList GetProperties()
        {
            return Properties;
        }

        public ArrayList GetPlayers()
        {
            return Players;
        }

        public Player GetPlayer(int playerIndex)
        {
            return (Player)Players[playerIndex];
        }

        public Player GetPlayer(string name)
        {
            foreach (Player player in Players)
            {
                if (player.GetName() == name)
                {
                    return player;
                }
            }

            return null;
        }

        public int GetPlayerCount()
        {
            return Players.Count;
        }

        public bool GetGameOver()
        {
            return _gameOver;
        }

        public void SetGameOver(bool gameOver)
        {
            _gameOver = gameOver;
        }

        /// <summary>
        /// VIOLATES SINGLETON PATTERN ***DO NOT USE, EXCEPT FOR TESTS***
        /// </summary>
        public void ResetBoard()
        {
            _board = null;
        }

        #endregion
    }
}
