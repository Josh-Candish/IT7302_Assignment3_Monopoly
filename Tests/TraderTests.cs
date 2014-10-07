using System;
using System.Collections;
using System.Linq;
using IT7302_MonopolyProject_21102588_JoshuaCandish;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TraderTests
    {
        private Trader _trader;
        private int _amount;
        private int _expected;
        private readonly TestHelpers _testHelper = new TestHelpers();

        [TestFixtureSetUp]
        public void BeforeAll()
        {
            _trader = new Trader();
        }

        [Test]
        public void when_paying_money_balance_should_go_down()
        {
            _trader.SetBalance(500);
            _amount = 50;
            _expected = 450;
            _trader.Pay(_amount);

            Assert.AreEqual(_trader.GetBalance(), _expected);
        }

        [Test]
        public void when_balance_below_0_trader_should_go_bankrupt()
        {
            _trader.SetBalance(0);

            var methodUnderTest = new TestDelegate(_trader.CheckBankrupt);

            Assert.Throws<ApplicationException>(methodUnderTest);
        }

        [Test]
        public void when_receiving_money_balance_should_go_up()
        {
            _trader.SetBalance(500);
            _amount = 50;
            _expected = 550;
            _trader.Receive(_amount);

            Assert.AreEqual(_expected, _trader.GetBalance());
        }

        [Test]
        public void to_string_has_correct_output()
        {
            var trader = new Trader("Josh", 500);

            const string expected = "Name: Josh \nBalance: 500";

            var actual = trader.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void when_a_property_is_obtained_it_should_be_included_in_properties_owned()
        {
            var property = new Property("Wiklow Street");

            var propertiesOwned = _trader.GetPropertiesOwned();

            // first test that the trader has no properties owned at this stage
            Assert.AreEqual(propertiesOwned.Count, 0);

            _trader.ObtainProperty(ref property);

            // test that a property has been obtained
            Assert.AreEqual(propertiesOwned.Count, 1);

            var propertyOwned = (Property)propertiesOwned[0];

            // test that the actual property in PropetiesOwned is the sames as the property added
            Assert.AreEqual(propertyOwned, property);
        }

        [Test]
        public void when_trading_property_owner_becomes_purchaser()
        {
            var purchaser = new Player("Hubert");

            var tradeableProperty = _testHelper.TradeProperty(purchaser);
            
            Assert.AreEqual(tradeableProperty.GetOwner(), purchaser);
        }

        [Test]
        public void after_trading_property_purchasers_balance_is_modified_correctly()
        {
            var purchaser = new Player("Josh");
            var purchaserBalanceBeforePurchase = purchaser.GetBalance();

            var tradeableProperty = _testHelper.TradeProperty(purchaser);

            var expectedPurchaserBalanceAfterPurchase = (purchaserBalanceBeforePurchase - tradeableProperty.GetPrice());
            var actualPurchaserBalanceAfterPurchase = purchaser.GetBalance();

            Assert.AreEqual(expectedPurchaserBalanceAfterPurchase, actualPurchaserBalanceAfterPurchase);
        }
    }
}
