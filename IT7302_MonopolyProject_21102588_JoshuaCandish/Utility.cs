using System;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    [Serializable]
    public class Utility : TradeableProperty
    {
        private static int _rentMultiplier = 6;

        public Utility() : this("Utility") {}

        public Utility(string name)
        {
            Name = name;
            Owner = Banker.Access();
        }

        public override void PayRent(ref Player player)
        {
            player.Pay(GetRent(ref player));
            GetOwner().Receive(GetRent());
        }

        public decimal GetRent(ref Player player)
        {
            return (_rentMultiplier*player.GetLastMove());
        }

        public override string LandOn(ref Player player)
        {
            if ((GetOwner() != Banker.Access()) && (GetOwner() != player))
            {
                PayRent(ref player);
                return string.Format("You rolled a total of {0}. So your rent is {0} x {1} = ${2}", player.GetLastMove(),
                    _rentMultiplier, (player.GetLastMove()*_rentMultiplier));
            }
            else
            {
                return base.LandOn(ref player);
            }
        }
    }
}
