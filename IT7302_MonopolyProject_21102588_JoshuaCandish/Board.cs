using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    [Serializable]
    public class Board
    {
        private static Board _board;
        private ArrayList Properties;
        private ArrayList Players;
        private ArrayList CommunityChestCards;
        private ArrayList ChanceCards;
        private int CardAmount = 16;
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
            CommunityChestCards = new ArrayList(CardAmount);
            ChanceCards = new ArrayList(CardAmount);
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void AddProperty(Property property)
        {
            Properties.Add(property);
        }

        public void AddChanceCard(Luck chanceCard)
        {
            ChanceCards.Add(chanceCard);
        }

        public void AddCommunityChestCard(Luck communityChestCard)
        {
            CommunityChestCards.Add(communityChestCard);
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

        public ArrayList GetChanceCards()
        {
            return ChanceCards;
        }

        public ArrayList GetCommunityChestCards()
        {
            return CommunityChestCards;
        }

        public Luck GetChanceCard()
        {
            var random = new Random();
            Luck cardToReturn = null;
            var cardFoundAtIndex = false;

            if (ChanceCards.Count > 0)
            {
                while (!cardFoundAtIndex)
                {
                    try
                    {
                        // Get a random card
                        var randomCard = random.Next(0, 15);
                        cardToReturn = (Luck) ChanceCards[randomCard];

                        // Remove the card from the deck
                        ChanceCards.Remove(cardToReturn);
                        cardFoundAtIndex = true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // If an error was thrown it's because the index from the
                        // random number doesn't exist anymore, so a card wasn't found
                        // and we need to try again (there is a card left
                        // in the collection otherwise we wouldn't be in this codeblock)
                        cardFoundAtIndex = false;
                    }
                }
            }

            return cardToReturn;
        }

        public Luck GetCommunityChestCard()
        {
            var random = new Random();
            Luck cardToReturn = null;
            var cardFoundAtIndex = false;

            if (CommunityChestCards.Count > 0)
            {
                while (!cardFoundAtIndex)
                {
                    try
                    {
                        // Get a random card
                        var randomCard = random.Next(0, 15);
                        cardToReturn = (Luck)CommunityChestCards[randomCard];

                        // Remove the card from the deck
                        CommunityChestCards.Remove(cardToReturn);
                        cardFoundAtIndex = true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // If an error was thrown it's because the index from the
                        // random number doesn't exist anymore, so a card wasn't found
                        // and we need to try again (there is a card left
                        // in the collection otherwise we wouldn't be in this codeblock)
                        cardFoundAtIndex = false;
                    }
                }
            }

            return cardToReturn;
        }

        public Player GetPlayer(int playerIndex)
        {
            return (Player)Players[playerIndex];
        }

        public Player GetPlayer(string name)
        {
            return Players.Cast<Player>().FirstOrDefault(player => player.GetName() == name);
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

        public List<Residential> GetAllPropertiesOfColour(string houseColour)
        {
            return GetProperties()
                .OfType<Residential>() // Only residential properties have a colour
                .Where(property => property.GetHouseColour() == houseColour)
                .ToList();
        }

        /// <summary>
        /// ***DO NOT USE, EXCEPT FOR TESTS***
        /// </summary>
        public void ResetBoard()
        {
            _board = null;
        }

        public void SetBoardFromLoadedBoard(Board board)
        {
            _board = board;
        }

        #endregion
    }
}
