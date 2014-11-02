using System;
using System.Linq;
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
            SetUp();
            const int expected = 32;
            var actual = Board.Access().GetChanceCards().Count + Board.Access().GetCommunityChestCards().Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void board_has_16_community_cards()
        {
            SetUp();
            const int expected = 16;
            var actual = Board.Access().GetCommunityChestCards().Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void board_has_16_chance_cards()
        {
            SetUp();
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
            Board.Access().AddPlayer(new Player());
            _gameOfMonopoly.SaveGame();

            _gameOfMonopoly.LoadGame();

            Assert.AreEqual(40, Board.Access().GetProperties().Count);

            var bankerOwnedProperty = (Property) Board.Access().GetProperties().ToArray().First();

            // Owner should be the banker for this property
            Assert.AreEqual(Banker.Access(), bankerOwnedProperty.GetOwner());

            // Need to reset the the save file to a blank board
            // for when actually playing the game
            Board.Access().ResetBoard();
            _gameOfMonopoly.SaveGame();
        }

        [Test]
        public void make_play_when_player_is_criminal()
        {
            SetUp();

            _player1.RolledDoublesCount = 3;
            _gameOfMonopoly.MakePlay(0);

            Assert.IsTrue(_player1.IsInJail);
        }

        #region Buying House

        [Test]
        public void no_houses_left()
        {
            var property = new Residential();
            Board.Access().Houses = 0;

            _gameOfMonopoly.BuyHouse(_player1, property, true);

            Assert.AreEqual(0, property.GetHouseCount());

            Board.Access().ResetBoard();
        }

        [Test]
        public void player_in_jail()
        {
            var property = new Residential();
            _player1.IsInJail = true;

            _gameOfMonopoly.BuyHouse(_player1, property, true);

            Assert.AreEqual(0, property.GetHouseCount());
        }

        [Test]
        public void player_doesnt_own_properties()
        {
            Board.Access().ResetBoard();
            var property = new Residential();

            _gameOfMonopoly.BuyHouse(_player1, property, true);

            Assert.AreEqual(0, property.GetHouseCount());
        }

        [Test]
        public void property_isnt_residential()
        {
            var property = new Transport();
            property.SetOwner(ref _player1);
            Board.Access().AddProperty(property);

            _gameOfMonopoly.BuyHouse(_player1, property, true);
        }

        [Test]
        public void property_is_mortgaged()
        {
            var property = new Residential();
            property.SetOwner(ref _player1);
            property.MortgageProperty();

            Board.Access().AddProperty(property);

            _gameOfMonopoly.BuyHouse(_player1, property, true);

            Assert.AreEqual(0, property.GetHouseCount());
        }

        [Test]
        public void player_doesnt_own_all_properties_of_colour()
        {
            var property = new Residential("Test");
            var property2 = new Residential("Test2");

            property.SetOwner(ref _player1);

            Board.Access().AddProperty(property);
            Board.Access().AddProperty(property2);

            _gameOfMonopoly.BuyHouse(_player1, property, true);

            Assert.AreEqual(0, property.GetHouseCount());
        }

        [Test]
        public void property_cant_be_developed_further()
        {
            Board.Access().ResetBoard();

            var property = new Residential("Test");
            var property2 = new Residential("Test2");
            property.SetOwner(ref _player1);
            property2.SetOwner(ref _player1);
            Board.Access().AddProperty(property);
            Board.Access().AddProperty(property2);

            property.AddHouse();

            _gameOfMonopoly.BuyHouse(_player1, property, true);

            Assert.AreEqual(1, property.GetHouseCount());
        }

        [Test]
        public void property_has_hotel()
        {
            Board.Access().ResetBoard();

            var property = new Residential("Test");
            property.SetOwner(ref _player1);
            property.HasHotel = true;
            Board.Access().AddProperty(property);

            _gameOfMonopoly.BuyHouse(_player1, property, true);

            Assert.AreEqual(0, property.GetHouseCount());
        }

        [Test]
        public void no_hotels_left()
        {
            Board.Access().ResetBoard();
            Board.Access().Hotels = 0;

            var property = new Residential("Test");
            property.SetOwner(ref _player1);

            for (var i = 0; i <= 3; i++)
            {
                property.AddHouse();
            }

            Board.Access().AddProperty(property);

            _gameOfMonopoly.BuyHouse(_player1, property, true);

            Assert.AreEqual(4, property.GetHouseCount());
            Assert.IsFalse(property.HasHotel);
        }

        [Test]
        public void adding_house()
        {
            Board.Access().ResetBoard();

            var property = new Residential("Test");
            property.SetOwner(ref _player1);
            Board.Access().AddProperty(property);

            _gameOfMonopoly.BuyHouse(_player1, property, true);

            Assert.AreEqual(1, property.GetHouseCount());
        }

        #endregion

        #region Selling House

        [Test]
        public void player_doesnt_own_any_properties()
        {
            Board.Access().ResetBoard();

            _gameOfMonopoly.SellHouse(_player1);

            Assert.Pass();
        }

        [Test]
        public void player_doesnt_own_residential_properties()
        {
            Board.Access().ResetBoard();
            var property = new Transport("Test");
            property.SetOwner(ref _player1);

            Board.Access().AddProperty(property);

            _gameOfMonopoly.SellHouse(_player1);

            Assert.Pass();
        }

        [Test]
        public void player_doesnt_own_property_with_house()
        {
            Board.Access().ResetBoard();
            var property = new Residential("Test");
            property.SetOwner(ref _player1);

            Board.Access().AddProperty(property);

            _gameOfMonopoly.SellHouse(_player1);

            Assert.Pass();
        }

        [Test]
        public void selling_house()
        {
            Board.Access().ResetBoard();

            var property = new Residential("Test");
            property.SetOwner(ref _player1);
            property.AddHouse();

            var playersBalanceBefore = _player1.GetBalance();
            var expectedIncreaseAmount = property.GetHouseCost()/2;
            
            Board.Access().AddProperty(property);

            _gameOfMonopoly.SellHouse(_player1, property);

            // Players balance should go up by half house cost
            Assert.AreEqual(playersBalanceBefore + expectedIncreaseAmount, _player1.GetBalance());
            // Property shouldn't have house anymore
            Assert.AreEqual(0, property.GetHouseCount());
            // Board's houses should be back to full
            Assert.AreEqual(32, Board.Access().Houses);
        }

        #endregion
    }
}
