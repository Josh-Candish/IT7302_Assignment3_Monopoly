using System;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    [Serializable]
    public class Transport : TradeableProperty
    {
        public Transport() : this("Railway Station") {}

        public Transport(string name)
        {
            Name = name;
            Price = 200;
            MortgageValue = 100;
            Rent = 50;
            Owner = Banker.Access();
        }
    }
}
