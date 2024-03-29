﻿using System.Collections.Generic;
using System.Linq;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;
using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
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
            // need to reset and add a property to the board for these tests
            var luckFactory = new LuckFactory();
            Board.Access().ResetBoard();
            Board.Access().AddProperty(luckFactory.create("Go", false, 200));

            _emptyPlayer.SetBalance(500);
            _emptyPlayer.SetName("Josh");
            _emptyPlayer.SetLocation(0);

            const string expectedStandardToString = "Josh";
            var actualStandardToString = _emptyPlayer.ToString();

            Assert.AreEqual(expectedStandardToString, actualStandardToString);

            const string expectedBriefDetailToString = "You are on Go.\tYou have $500.";
            var actualBriefDetailToString = _emptyPlayer.BriefDetailToString();

            Assert.AreEqual(expectedBriefDetailToString, actualBriefDetailToString);

            var expectedFullDetailToString = string.Format("Player:Josh.\nBalance: $500\nLocation: Go: \t Owned by: Leeroy Jenkins (Square 0) \nProperties Owned:\n{0}", _emptyPlayer.PropertiesOwnedToString());
            var actualFullDetailToString = _emptyPlayer.FullDetailToString();

            Assert.AreEqual(expectedFullDetailToString, actualFullDetailToString);

            const string expectedDiceRollToString = "Rolling dice: \t Dice 1: 0 \t Dice 2: 0";
            var actualDiceRollToString = _emptyPlayer.DiceRollingToString();
        }

        [Test]
        public void setting_location_not_greater_than_board()
        {
            FreshBoard();

            _emptyPlayer.SetLocation(30);

            var expectedProperty = Board.Access().GetProperty(30);
            var actualProperty = Board.Access().GetProperty(_emptyPlayer.GetLocation());

            Assert.AreEqual(expectedProperty, actualProperty);
        }

        [Test]
        public void setting_location_greater_than_board()
        {
            FreshBoard();

            _emptyPlayer.SetLocation(49);

            const string expectedPropertyName = "Waitangi Treaty Grounds";
            var actualPropertyName = Board.Access().GetProperty(_emptyPlayer.GetLocation()).GetName();

            Assert.AreEqual(expectedPropertyName, actualPropertyName);
        }

        [Test]
        public void setting_location_to_board_size()
        {
            FreshBoard();

            _emptyPlayer.SetLocation(40);

            var expectedPropertyName = "Rangitoto";
            var actualPropertyName = Board.Access().GetProperty(_emptyPlayer.GetLocation()).GetName();

            Assert.AreEqual(expectedPropertyName, actualPropertyName);

            _emptyPlayer.SetLocation(41);

            expectedPropertyName = "Go";
            actualPropertyName = Board.Access().GetProperty(_emptyPlayer.GetLocation()).GetName();

            Assert.AreEqual(expectedPropertyName, actualPropertyName);
        }

        [Test]
        public void landing_on_go_to_jail_sends_player_to_jail()
        {
            FreshBoard();
            PutPlayerInJail();

            Assert.IsTrue(_emptyPlayer.IsInJail);

            // Check they are now on the jail property
            Assert.AreEqual(Board.Access().GetProperty(10), Board.Access().GetProperty(_emptyPlayer.GetLocation()));
        }

        [Test]
        public void paying_fee_gets_out_of_jail()
        {
            PutPlayerInJail();

            Assert.IsTrue(_emptyPlayer.IsInJail);
            _emptyPlayer.SetBalance(500);
            _emptyPlayer.PayJailFee();

            Assert.IsFalse(_emptyPlayer.IsInJail);
        }

        [Test]
        public void paying_fee_without_enough_money_stays_in_jail()
        {
            PutPlayerInJail();

            Assert.IsTrue(_emptyPlayer.IsInJail);

            _emptyPlayer.SetBalance(10);
            _emptyPlayer.PayJailFee();

            Assert.IsTrue(_emptyPlayer.IsInJail);
        }

        [Test]
        public void failure_to_roll_doubles_in_jail()
        {
            FreshBoard();
            PutPlayerInJail();

            Assert.IsTrue(_emptyPlayer.IsInJail);

            Assert.AreEqual(0, _emptyPlayer.RollDoublesFailureCount);

            _emptyPlayer.Move();

            if (!_emptyPlayer.RolledDoubles) Assert.AreEqual(1, _emptyPlayer.RollDoublesFailureCount);
        }

        [Test]
        public void player_move_in_jail_doesnt_actually_move()
        {
            FreshBoard();
            PutPlayerInJail();

            var locationBeforeMove = _emptyPlayer.GetLocation();

            _emptyPlayer.Move();

            var locationAfterMove = _emptyPlayer.GetLocation();

            Assert.AreEqual(locationBeforeMove, locationAfterMove);
        }

        [Test]
        public void when_player_doesnt_own_all_properties_of_colour()
        {
            FreshBoard();

            // Get all the residential properties on the board
            var residentialProperties =
                Board.Access()
                    .GetProperties()
                    .Cast<object>()
                    .Where(property => property.GetType() == typeof (Residential))
                    .Cast<Residential>()
                    .ToList();

            // Get the first one's colour
            var randomResidentialColour = residentialProperties.First().GetHouseColour();

            // Shouldn't own all of the properties of that colour
            Assert.IsFalse(_emptyPlayer.OwnsAllPropertiesOfColour(randomResidentialColour));
        }

        [Test]
        public void when_player_owns_all_properties_of_colour()
        {
            FreshBoard();
            const string colour = "Red";
            // Get all the red properties on the board
            var redProperties = Board.Access().GetAllPropertiesOfColour(colour);

            // Set the player to be the owner of them
            foreach (var property in redProperties)
            {
                property.SetOwner(ref _emptyPlayer);
            }

            // Should now be the owner of all of those properties
            Assert.IsTrue(_emptyPlayer.OwnsAllPropertiesOfColour(colour));
        }

        [Test]
        public void attempt_property_development()
        {
            FreshBoard();
            var blueProperties = Board.Access().GetAllPropertiesOfColour("Blue");

            foreach (var blueProperty in blueProperties)
            {
                blueProperty.SetOwner(ref _emptyPlayer);
            }

            // Just get a random one of those properties
            var randomBlueProperty = blueProperties.First();

            // Add a house to that property
            randomBlueProperty.AddHouse();

            // Now that property shouldn't be allowed to be developed anymore because 
            // the other properties in this colour group don't have one house as well
            Assert.IsFalse(_emptyPlayer.CanDevelopPropertyFurther(randomBlueProperty));

            // Get another property
            var anotherRandomBlueProperty = blueProperties.Last();

            // This property should be okay to develop to bring it to the same development
            // of the previous property
            Assert.IsTrue(_emptyPlayer.CanDevelopPropertyFurther(anotherRandomBlueProperty));
        }

        #region Helpers

        private void PutPlayerInJail()
        {
            _emptyPlayer.SetLocation(30);
            _emptyPlayer.IsCriminal();
        }

        private static void FreshBoard()
        {
            Board.Access().ResetBoard();
            // Need to add all properties to board
            var monopoly = new Monopoly();
            monopoly.SetUpProperties();
        }

        #endregion
    }
}
