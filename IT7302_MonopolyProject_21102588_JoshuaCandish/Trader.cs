using System;
using System.Collections;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Trader
    {
        protected ArrayList PropertiesOwned = new ArrayList();
        protected decimal Balance;
        protected string Name;

        public Trader()
        {
            
        }

        public Trader(string name, decimal balance)
        {
            Balance = balance;
            Name = name;
        }

        public virtual void CheckBankrupt()
        {
            if (GetBalance() <= 0)
            {
                throw new ApplicationException(string.Format("{0} is now bankrupt.\n Balance: {1}", GetName(), GetBalance()));
            }
        }

        public override string ToString()
        {
            return string.Format("Name: {0} \nBalance: {1}", GetName(), GetBalance());
        }

        public void Pay(decimal amount)
        {
            Balance -= amount;
            CheckBankrupt();
        }

        public void Receive(decimal amount)
        {
            Balance += amount;
        }

        public void ObtainProperty(ref Property property)
        {
            PropertiesOwned.Add(property);
        }

        public void TradeProperty(ref TradeableProperty property, ref Player purchaser, decimal amount, decimal mortgageAmount)
        {
            // If the property isn't mortgaged the mortgage amount will just be 0
            purchaser.Pay(amount + mortgageAmount);
            Receive(amount);
            property.SetOwner(ref purchaser);
            Banker.Access().Receive(mortgageAmount);
        }

        #region Getters/Setters

        public decimal GetBalance()
        {
            return Balance;
        }

        public void SetBalance(decimal value)
        {
            Balance = value;
        }

        public string GetName()
        {
            return Name;
        }

        public void SetName(string value)
        {
            Name = value;
        }

        public ArrayList GetPropertiesOwned()
        {
            return PropertiesOwned;
        }

        #endregion
    }
}
