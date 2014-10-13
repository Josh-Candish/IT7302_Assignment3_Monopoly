using System;
using IT7302_MonopolyProject_21102588_JoshuaCandish.Factories;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Tests
{
    public class TestHelpers
    {
        public TradeableProperty TradeProperty(Player purchaser)
        {
            var transportFactory = new TransportFactory();

            TradeableProperty tradeableProperty = transportFactory.create("Railway Station");

            // The trader should be the banker as they own the property
            Trader trader = Banker.Access();

            trader.TradeProperty(ref tradeableProperty, ref purchaser, tradeableProperty.GetPrice());

            return tradeableProperty;
        }

        public void TestPlayerBankruptHandler(object obj, EventArgs args)
        {
            var p = (Player)obj;
        }

        public void TestPlayerPassGoHandler(object obj, EventArgs args)
        {
            var p = (Player)obj;
        }
    }
}
