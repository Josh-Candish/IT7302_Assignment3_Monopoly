namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Residential : TradeableProperty
    {
        private decimal HouseCost;
        private int HouseCount;
        private const int MaxHouses = 4;
        private string HouseColour;
        private bool HasHotel;

        public Residential() : this ("Residential Property") {}

        public Residential(string name) : this(name,200,50,50,"Red") {}

        public Residential(string name, decimal price, decimal rent, decimal houseCost, string houseColour)
        {
            Name = name;
            Price = price;
            MortgageValue = price/2;
            Rent = rent;
            HouseCost = houseCost;
            HouseColour = houseColour;
        }

        public override decimal GetRent()
        {
            return Rent + (Rent * HouseCount);
        }

        public void AddHouse()
        {
            GetOwner().Pay(HouseCost);
            HouseCount++;
        }

        public decimal GetHouseCost()
        {
            return HouseCost;
        }

        public int GetHouseCount()
        {
            return HouseCount;
        }

        public static int GetMaxHouses()
        {
            return MaxHouses;
        }

        public string GetHouseColour()
        {
            return HouseColour;
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("\t Houses: {0}", HouseCount);
        }
    }
}
