namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Property
    {
        protected string Name;
        protected Trader Owner;

        public Property()
        {
            
        }

        public Property(string name)
        {
            Name = name;
        }

        public Property(string name, ref Trader owner)
        {
            Name = name;
            Owner = owner;
        }

        public virtual bool AvailableForPurchase()
        {
            return false;
        }

        public virtual string LandOn(ref Player player)
        {
            return string.Format("{0} landed on {1}", player.GetName(), GetName());
        }

        public override string ToString()
        {
            return string.Format("{0}: \t Owned by: {1}", GetName(), GetOwner().GetName());
        }

        #region Getters/Setters

        public Trader GetOwner()
        {
            return Owner;
        }

        public string GetName()
        {
            return Name;
        }

        public void SetOwner(ref Player newOwner)
        {
            Owner = newOwner;
        }

        public void SetOwner(ref Banker newOwner)
        {
            Owner = newOwner;
        }

        #endregion
    }
}
