using System;
using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
    [TestFixture]
    public class MonopolyTests
    {
        private Monopoly _gameOfMonopoly;
        private Player _player1, _player2;

        [TestFixtureSetUp]
        public void SetUp()
        {
            Board.Access().ResetBoard();

            _gameOfMonopoly = new Monopoly();
            _gameOfMonopoly.SetUpProperties();
            _gameOfMonopoly.SetUpCards();

            _player1 = new Player("Josh");
            _player2 = new Player("Hubert");

            Board.Access().AddPlayer(_player1);
            Board.Access().AddPlayer(_player2);
        }

        [Test]
        public void board_has_40_properties()
        {      
            const int expected = 40;
            Assert.AreEqual(expected, Board.Access().GetProperties().Count);
        }

        [Test]
        public void playing_game_moves_player()
        {
            SetUp();

            var originalPlayerPosition = _player1.GetLocation();

            try
            {
                // Will throw an argument null error because console input 
                // is not given so we need to catch it to perform the test
                _gameOfMonopoly.PlayGame();
            }
            catch (ArgumentNullException)
            {
                var expectedPlayerLocation = originalPlayerPosition + _player1.GetLastMove();
                var actualLocation = _player1.GetLocation();

                // Player's new location should be it's original 
                // location plus the last move rolled for them
                Assert.AreEqual(expectedPlayerLocation, actualLocation);
            }
        }

        [Test]
        public void game_ends_when_1_active_player_left()
        {
            // Bankrupt the player to make inactive
            _player1.SetBalance(0);
            _player1.CheckBankrupt();

            _gameOfMonopoly.PlayGame();

            Assert.IsTrue(Board.Access().GetGameOver());
        }

        [Test]
        public void purchasing_property()
        {
            SetUp();     
            
            _player1.SetLocation(5);

            _gameOfMonopoly.PurchaseProperty(_player1, true);

            var propertyOwner = Board.Access().GetProperty(5).GetOwner();

            Assert.AreEqual(propertyOwner, _player1);
        }

        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void setting_up_players_fails_with_less_than_2_players()
        {
            Board.Access().ResetBoard();
            _gameOfMonopoly = new Monopoly();
            _gameOfMonopoly.SetUpProperties();

            _gameOfMonopoly.SetUpPlayers(1);
        }

        [Test]
        public void setting_up_players_with_valid_players()
        {
            Board.Access().ResetBoard();
            _gameOfMonopoly = new Monopoly();
            _gameOfMonopoly.SetUpProperties();

            _gameOfMonopoly.SetUpPlayers(2,"Josh");

            Assert.AreEqual(2, Board.Access().GetPlayerCount());
        }

        [Test]
        public void Events()
        {
            // Check logs correct output to console
            Monopoly.playerBankruptHandler(_player1, EventArgs.Empty);
            Monopoly.playerPassGoHandler(_player1, EventArgs.Empty);
        }

        [Test]
        public void board_has_32_chance_community_cards()
        {
            const int expected = 32;
            var actual = Board.Access().GetChanceCards().Count + Board.Access().GetCommunityChestCards().Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void board_has_16_community_cards()
        {
            const int expected = 16;
            var actual = Board.Access().GetCommunityChestCards().Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void board_has_16_chance_cards()
        {
            const int expected = 16;
            var actual = Board.Access().GetChanceCards().Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void loading_board_from_saved_board()
        {
            Board.Access().ResetBoard();

            CollectionAssert.IsEmpty(Board.Access().GetProperties());

            _gameOfMonopoly.SetUpProperties();
            Board.Access().AddPlayer(new Player());
            _gameOfMonopoly.SaveGame();

            _gameOfMonopoly.LoadGame();

            Assert.AreEqual(40, Board.Access().GetProperties().Count);
        }
    }
}
