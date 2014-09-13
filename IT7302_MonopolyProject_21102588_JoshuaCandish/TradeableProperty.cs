using System;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public abstract class TradeableProperty : Property
    {
        protected decimal Price;
        protected decimal MortgageValue;
        protected decimal Rent;

        protected TradeableProperty()
        {
            Price = 200;
            MortgageValue = 100;
            Rent = 50;
        }

        public virtual void PayRent(ref Player player)
        {
            player.Pay(Rent);
            GetOwner().Receive(Rent);
        }

        public void Purchase(ref Player buyer)
        {
            if (AvailableForPurchase())
            {
                buyer.Pay(GetPrice());
                SetOwner(ref buyer);
            }
            else
            {
                throw new ApplicationException("This property is not available for purchase.");
            }
        }

        public override bool AvailableForPurchase()
        {
            // Only if banker owns the property is it available for purchase
            return Owner == Banker.Access();
        }

        public override string LandOn(ref Player player)
        {
            if((GetOwner() != Banker.Access()) && (GetOwner() != player))
            {
                //pay rent
                PayRent(ref player);
                return base.LandOn(ref player) +
                       string.Format("Rent has been paid for {0} of ${1} to {2}", GetName(), GetRent(), Owner.GetName());
            }
            else
            {
                return base.LandOn(ref player);
            }
        }

        #region Getters/Setters

        public decimal GetPrice()
        {
            return Price;
        }

        public decimal GetMortgageValue()
        {
            return MortgageValue;
        }

        public virtual decimal GetRent()
        {
            return Rent;
        }

        #endregion
    }
}
