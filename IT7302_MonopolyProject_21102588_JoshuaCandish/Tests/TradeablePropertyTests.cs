using System;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;
using NUnit.Framework;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
    [TestFixture]
    public class TradeablePropertyTests
    {
        private TradeableProperty _tradeableProperty;
        private Player _player;
        private Banker _banker;

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            var residentialFactory = new ResidentialFactory();
            _tradeableProperty = residentialFactory.create("Test Residential", 200, 50, 50);
            _player = new Player("Josh", 500);
            _banker = Banker.Access();
        }

        [Test]
        public void owner_of_property_recieves_money_from_rent()
        {
            _tradeableProperty.SetOwner(ref _banker);
            var bankerBalanceBefore = _banker.GetBalance();
            _tradeableProperty.PayRent(ref _player);
            var bankerBalanceAfter = _banker.GetBalance();

            Assert.AreEqual(bankerBalanceBefore + _tradeableProperty.GetRent(), bankerBalanceAfter);
        }

        [Test]
        public void purchasing_property_changes_owner()
        {
            _tradeableProperty.SetOwner(ref _banker);

            Assert.IsTrue(_tradeableProperty.GetOwner().Equals(_banker));

            _tradeableProperty.Purchase(ref _player);

            Assert.IsTrue(_tradeableProperty.GetOwner().Equals(_player));
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public void property_not_available_for_purchase_throws_exception()
        {
            _tradeableProperty.SetOwner(ref _player);

            var differentPlayer = new Player();

            // this should throw as _player already owns this property now
            _tradeableProperty.Purchase(ref differentPlayer);
        }

        [Test]
        public void landing_on_other_players_property_results_in_rent_payment()
        {
            _tradeableProperty.SetOwner(ref _player);

            var differentPlayer = new Player("Milford");
            var payerBalanceBeforeLandOn = differentPlayer.GetBalance();

            var ownerBalanceBeforeLandOn = _player.GetBalance();

            var returnedString = _tradeableProperty.LandOn(ref differentPlayer);

            // Owner's balance should now be equal to their original balance 
            // plus the rent cost of the property
            Assert.AreEqual(_player.GetBalance(), (ownerBalanceBeforeLandOn + _tradeableProperty.GetRent()));

            // Payer's balance should now be equal to their original balance 
            // minus the rent cost of the property
            Assert.AreEqual(differentPlayer.GetBalance(), (payerBalanceBeforeLandOn - _tradeableProperty.GetRent()));

            var expectedReturnString =
                string.Format(
                    "Milford landed on Test Residential.\nRent has been paid for Test Residential of ${0} to Josh",
                    _tradeableProperty.GetRent());

            Assert.AreEqual(expectedReturnString, returnedString);
        }

        [Test]
        public void landing_on_own_property_doesnt_result_in_rent_payment()
        {
            _tradeableProperty.SetOwner(ref _player);

            var ownerBalanceBeforeLandOn = _player.GetBalance();

            _tradeableProperty.LandOn(ref _player);

            var ownerBalanceAfterLandOn = _player.GetBalance();

            Assert.AreEqual(ownerBalanceBeforeLandOn, ownerBalanceAfterLandOn);
        }

        [Test]
        public void mortgage_value_is_correct()
        {
            const int expectedMortgageValue = 100;
            var actualMortgageValue = _tradeableProperty.GetMortgageValue();

            Assert.AreEqual(expectedMortgageValue, actualMortgageValue);
        }
    }
}
