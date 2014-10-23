using System.Linq;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Residential : TradeableProperty
    {
        private decimal HouseCost;
        private int HouseCount;
        private const int MaxHouses = 4;
        private string HouseColour;

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
            // This is the rent value for a property with a hotel
            if (HasHotel) return Rent + (Rent*5);

            // The rent is double if the owner owns all the properties of this colour and the property is undeveloped (exclude the banker)
            if ((Board.Access().GetAllPropertiesOfColour(HouseColour).All(x => x.Owner == Owner && Owner != Banker.Access())) && (HouseCount == 0))
            {
                return Rent * 2;
            }

            // The rent multiplier is dependent on if the property
            // is at the hotel stage or not yet
            return Rent + (Rent*HouseCount);
        }

        public void AddHouse()
        {
            GetOwner().Pay(HouseCost);
            HouseCount++;

            if (HouseCount > 4)
            {
                HasHotel = true;
            }
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

        public bool HasHotel { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("\t Houses: {0}", HouseCount);
        }
    }
}
