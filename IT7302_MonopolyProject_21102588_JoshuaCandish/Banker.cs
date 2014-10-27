using System;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    [Serializable]
    public class Banker : Trader
    {
        private static Banker _banker;

        public Banker()
        {
            SetName("Leeroy Jenkins");
            SetBalance(InitialValueAccessor.GetBankerStartingBanker());
        }

        public static Banker Access()
        {
            return _banker ?? (_banker = new Banker());
        }
    }
}
