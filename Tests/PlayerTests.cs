using System;
using IT7302_MonopolyProject_21102588_JoshuaCandish;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PlayerTests
    {
        private Player _emptyPlayer;
        private TestHelpers _testHelper = new TestHelpers();

        [TestFixtureSetUp]
        public void CreateEmptyPlayer()
        {
            _emptyPlayer = new Player();
        }

        [Test]
        public void PropertiesOwnedToString_returns_correct_string()
        {
            var noProperties = _emptyPlayer.PropertiesOwnedToString();

            Assert.AreEqual("None", noProperties);

            _emptyPlayer.SetBalance(500);
            _emptyPlayer.SetName("Josh");

            // trade the properties
            var tradaebleProperty1 = _testHelper.TradeProperty(_emptyPlayer);
            var tradaebleProperty2 = _testHelper.TradeProperty(_emptyPlayer);

            // add the properties to the board
            Board.Access().AddProperty(tradaebleProperty1);
            Board.Access().AddProperty(tradaebleProperty2);

            const string expected = "Railway Station: \t Owned by: Josh\nRailway Station: \t Owned by: Josh\n";
            var actual = _emptyPlayer.PropertiesOwnedToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void a_bankrupt_player_should_be_inactive()
        {
            _emptyPlayer.SetBalance(0);
            _emptyPlayer.CheckBankrupt();

            Assert.IsFalse(_emptyPlayer.IsActive());
        }

        [Test]
        public void a_bankrupt_players_properties_should_become_the_bankers()
        {
            // bankrupt that mofo
            _emptyPlayer.SetBalance(0);
            _emptyPlayer.PlayerBankrupt += _testHelper.TestPlayerBankruptHandler;

            // create a property, add it to the board, and set the owner to the player under test
            var propertyOfBankruptPlayer = new Property("Test Property");
            Board.Access().AddProperty(propertyOfBankruptPlayer);
            propertyOfBankruptPlayer.SetOwner(ref _emptyPlayer);

            _emptyPlayer.CheckBankrupt();

            // owner of the property should now be the banker
            Assert.IsTrue(propertyOfBankruptPlayer.GetOwner().Equals(Banker.Access()));
        }

        [Test]
        public void player_should_receive_200_after_passing_go()
        {
            _emptyPlayer.SetBalance(500);
            _emptyPlayer.PlayerPassGo += _testHelper.TestPlayerPassGoHandler;
            _emptyPlayer.SetLocation(45);

            Assert.AreEqual(700, _emptyPlayer.GetBalance());
        }

        [Test]
        public void verify_ToString_methods()
        {
            // need to add a property to the board for these tests
            var luckFactory = new LuckFactory();
            Board.Access().AddProperty(luckFactory.create("Go", false, 200));

            _emptyPlayer.SetBalance(500);
            _emptyPlayer.SetName("Josh");
            _emptyPlayer.SetLocation(3);

            const string expectedStandardToString = "Josh";
            var actualStandardToString = _emptyPlayer.ToString();

            Assert.AreEqual(expectedStandardToString, actualStandardToString);

            const string expectedBriefDetailToString = "You are on Railway Station.\tYou have $500.";
            var actualBriefDetailToString = _emptyPlayer.BriefDetailToString();

            Assert.AreEqual(expectedBriefDetailToString, actualBriefDetailToString);

            var expectedFullDetailToString = string.Format("Player:Josh.\nBalance: $500\nLocation: Railway Station: \t Owned by: Josh (Square 3) \nProperties Owned:\n{0}", _emptyPlayer.PropertiesOwnedToString());
            var actualFullDetailToString = _emptyPlayer.FullDetailToString();

            Assert.AreEqual(expectedFullDetailToString, actualFullDetailToString);

            const string expectedDiceRollToString = "Rolling dice: \t Dice 1: 0 \t Dice 2: 0";
            var actualDiceRollToString = _emptyPlayer.DiceRollingToString();
        }
    }
}
