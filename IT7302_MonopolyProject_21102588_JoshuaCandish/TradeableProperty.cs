using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class TradeableProperty : Property
    {
        private readonly decimal _price;
        private readonly decimal _mortgageValue;
        private readonly decimal _rent;

        public TradeableProperty()
        {
            _price = 200;
            _mortgageValue = 100;
            _rent = 50;
        }

       

        public virtual void PayRent(ref Player player)
        {
            player.Pay(_rent);
            GetOwner().Receive(_rent);
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
            return _price;
        }

        public decimal GetMortgageValue()
        {
            return _mortgageValue;
        }

        public virtual decimal GetRent()
        {
            return _rent;
        }

        #endregion
    }
}
