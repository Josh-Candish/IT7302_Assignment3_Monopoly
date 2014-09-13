namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Luck : Property
    {
        private bool _isBenefitNotPenalty;
        private decimal _penaltyOrBenefitAmount;

        public Luck() : this("Luck Property", true, 50)
        {
        }

        public Luck(string name, bool isBenefitNotPenalty, decimal penaltyOrBenefitAmount)
        {
            Name = name;
            _isBenefitNotPenalty = isBenefitNotPenalty;
            _penaltyOrBenefitAmount = penaltyOrBenefitAmount;
        }

        public override string LandOn(ref Player player)
        {
            if (_isBenefitNotPenalty)
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
