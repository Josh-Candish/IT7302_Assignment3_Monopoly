using System;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    [Serializable]
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
            if (!_isPenalty) // Go to jail and get out of jail cards are set to not be penalties by default
            {
                // These name checks are enough to determine if the card drawn
                // is a go to jail or get out of jail card
                if (Name.ToLower().Contains("go to jail"))
                {
                    player.GoToJail();
                    return base.LandOn(ref player);
                }

                if (Name.ToLower().Contains("get out of jail free"))
                {
                    player.GetOutOfJailCardCount++;
                    return base.LandOn(ref player);
                }

                player.Receive(_penaltyOrBenefitAmount);
                return base.LandOn(ref player) + string.Format(" {0} has received {1}.", player.GetName(), _penaltyOrBenefitAmount);
            }
            else
            {
                player.Pay(_penaltyOrBenefitAmount);
                return base.LandOn(ref player) + string.Format(" {0} has paid {1}.", player.GetName(), _penaltyOrBenefitAmount);
            }
        }
    }
}
