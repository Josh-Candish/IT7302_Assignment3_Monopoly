namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Luck : Property
    {
        private bool _isPenalty;
        private decimal _penaltyOrBenefitAmount;

        public Luck() : this("Luck Property", true, 50)
        {
        }

        public Luck(string name, bool isPenalty, decimal penaltyOrBenefitAmount)
        {
            Name = name;
            _isPenalty = isPenalty;
            _penaltyOrBenefitAmount = penaltyOrBenefitAmount;
        }

        public override string LandOn(ref Player player)
        {
            if (!_isPenalty)
            {
                player.Receive(_penaltyOrBenefitAmount);
                return base.LandOn(ref player) +
                       string.Format("{0} has received {1}.", player.GetName(), _penaltyOrBenefitAmount);
            }
            else
            {
                player.Pay(_penaltyOrBenefitAmount);
                return base.LandOn(ref player) +
                                       string.Format("{0} has paid {1}.", player.GetName(), _penaltyOrBenefitAmount);
            }
        }
    }
}
