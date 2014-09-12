using System;
using IT7302_MonopolyProject_21102588_JoshuaCandish;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TraderTests
    {
        private Trader _trader;
        private int _amount;
        private int _expected;

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
    }
}
