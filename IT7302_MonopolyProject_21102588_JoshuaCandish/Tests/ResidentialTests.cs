using System;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;
using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
    [TestFixture]
    public class ResidentialTests
    {
        private Residential _residentialProperty;
        private readonly ResidentialFactory _residentialFactory = new ResidentialFactory();


        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _residentialProperty = _residentialFactory.create("Cape Reinga Lighthouse", 140, 14, 100, "Red");
            _residentialProperty.GetHouseCost();
        }

        [Test]
        public void property_has_house_after_adding_house()
        {
            _residentialProperty = NewResidential();
            _residentialProperty.AddHouse();
            Assert.IsTrue(_residentialProperty.GetHouseCount() == 1);
        }

        [Test]
        public void rent_for_property_with_one_house_is_correct()
        {
            _residentialProperty = NewResidential();
            _residentialProperty.AddHouse();
            const decimal rentPlusOneHouse = 28;
            var rentWithHouse = _residentialProperty.GetRent();

            Assert.AreEqual(rentPlusOneHouse, rentWithHouse);
        }

        [Test]
        public void rent_for_mortgaged_property_is_zero()
        {
            _residentialProperty = NewResidential();
            _residentialProperty.IsMortgaged = true;
            var rentForMortgagedProperty = _residentialProperty.GetRent();

            Assert.AreEqual(Decimal.Zero, rentForMortgagedProperty);
        }

        [Test]
        public void max_houses_should_always_be_4()
        {
            // This is the case for standard monopoly rules
            Assert.AreEqual(4, Residential.GetMaxHouses());
        }

        [Test]
        public void to_string_is_correct()
        {
            _residentialProperty.AddHouse();
            var toString = _residentialProperty.ToString();

            Assert.IsTrue(toString.Contains("\t Houses: 1"));
        }

        [Test]
        public void rent_is_doubled_for_undeveloped_prop_single_owner_colour()
        {
            Board.Access().ResetBoard();
            var player = new Player("Josh");
            _residentialProperty = NewResidential();
            _residentialProperty.SetOwner(ref player);
            
            Board.Access().AddProperty(_residentialProperty);

            const decimal orginalRent = 14;
            const decimal expectedRent = orginalRent*2;
            var actualRent = _residentialProperty.GetRent();

            Assert.AreEqual(expectedRent, actualRent);
        }

        [Test]
        public void rent_is_not_doubled_for_undeveloped_prop_single_owner_colour_banker()
        {
            Board.Access().ResetBoard();
            var banker = Banker.Access();

            _residentialProperty = NewResidential();
            _residentialProperty.SetOwner(ref banker);

            Board.Access().AddProperty(_residentialProperty);

            const decimal orginalRent = 14;
            var actualRent = _residentialProperty.GetRent();

            Assert.AreEqual(orginalRent, actualRent);
        }

        [Test]
        public void property_with_hotel()
        {
            Board.Access().ResetBoard();
            _residentialProperty = NewResidential();
            Board.Access().AddProperty(_residentialProperty);
            // Multiplied by 5 houses is the expected hotel rent cost
            var expectedRent = _residentialProperty.GetRent() + (_residentialProperty.GetRent() * 5);

            _residentialProperty.HasHotel = true;

            var actualRent = _residentialProperty.GetRent();

            Assert.AreEqual(expectedRent, actualRent);
        }

        [Test]
        public void five_houses_equates_to_hotel()
        {
            _residentialProperty = NewResidential();

            // Shouldn't have houses or hotel to start with
            Assert.IsFalse(_residentialProperty.HasHotel);
            Assert.AreEqual(0, _residentialProperty.GetHouseCount());

            for (var i = 0; i <= 4; i++)   
            {
                _residentialProperty.AddHouse();
            }

            Assert.IsTrue(_residentialProperty.HasHotel);
        }

        [Test]
        public void mortgaged_property_cant_be_mortgaged_again()
        {
            _residentialProperty = NewResidential();

            _residentialProperty.MortgageProperty();

            // Should now be mortgaged after mortgaging
            Assert.IsTrue(_residentialProperty.IsMortgaged);

            // Trying to mortgage again should return false
            Assert.IsFalse(_residentialProperty.MortgageProperty());
        }

        [Test]
        public void developed_property_cant_be_mortgaged()
        {
            _residentialProperty = NewResidential();
            _residentialProperty.AddHouse();

            Assert.IsFalse(_residentialProperty.MortgageProperty());
        }

        [Test]
        public void nonmortgaged_property_cant_be_unmortgaged()
        {
            _residentialProperty = NewResidential();

            // Shouldn't be mortgaged
            Assert.IsFalse(_residentialProperty.IsMortgaged);

            // So we shouldn't be able to unmortgage it
            Assert.IsFalse(_residentialProperty.UnmortgageProperty());
        }

        [Test]
        public void mortgaging_property_results_in_correct_balance_alterations()
        {
            _residentialProperty = NewResidential();
            var testPlayer = GetMeANewPlayer();
            var mortgageValue = _residentialProperty.GetMortgageValue();
            var ownerBalanceBefore = testPlayer.GetBalance();
            var bankerBalanceBefore = Banker.Access().GetBalance();

            _residentialProperty.SetOwner(ref testPlayer);

            _residentialProperty.MortgageProperty();

            // Property should now be mortgaged
            Assert.IsTrue(_residentialProperty.IsMortgaged);

            // The property's owner should have received the mortgage money
            Assert.AreEqual(ownerBalanceBefore + mortgageValue, testPlayer.GetBalance());

            // The banker should have paid the mortgage money
            Assert.AreEqual(bankerBalanceBefore - mortgageValue, Banker.Access().GetBalance());
        }

        [Test]
        public void unmortgaging_property_results_in_correct_balance_alterations()
        {
            _residentialProperty = NewResidential();
            var testPlayer = GetMeANewPlayer();
            var paybackValue = _residentialProperty.GetMortgageValue() + (_residentialProperty.GetMortgageValue() * 10 / 100);// mortgage plus 10%

            _residentialProperty.SetOwner(ref testPlayer);
            _residentialProperty.MortgageProperty();

            var ownerBalaceBeforeUnmortgage = testPlayer.GetBalance();
            var bankerBalanceBeforeUnmortgage = Banker.Access().GetBalance();

            _residentialProperty.UnmortgageProperty();

            // The property's owner should have paid the mortgage payback value
            Assert.AreEqual(ownerBalaceBeforeUnmortgage - paybackValue, testPlayer.GetBalance());

            // The banker should have received the mortgage payback value
            Assert.AreEqual(bankerBalanceBeforeUnmortgage + paybackValue, Banker.Access().GetBalance());
        }

        [Test]
        public void adding_house_takes_away_from_board()
        {
            Board.Access().ResetBoard();

            var expectedHouses = Board.Access().Houses - 1;

            _residentialProperty.AddHouse();

            var actualHouses = Board.Access().Houses;

            Assert.AreEqual(expectedHouses, actualHouses);
        }

        [Test]
        public void adding_hotel()
        {
            Board.Access().ResetBoard();
            _residentialProperty = NewResidential();

            for (var i = 0; i <= 4; i++)
            {
                _residentialProperty.AddHouse();
            }

            Assert.AreEqual(0, _residentialProperty.GetHouseCount());
            Assert.AreEqual(32, Board.Access().Houses);
            Assert.IsTrue(_residentialProperty.HasHotel);
        }

        #region Helpers

        private Residential NewResidential()
        {
            return _residentialFactory.create("Cape Reinga Lighthouse", 140, 14, 100, "Red");
        }

        private static Player GetMeANewPlayer()
        {
            var player = new Player("TestPlayer");
            return player;
        }
        #endregion
    }
}
