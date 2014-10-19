namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Factories
{
    public class ResidentialFactory : PropertyFactory
    {
        public Residential create(string name, decimal price, decimal rent, decimal houseCost, string houseColour)
        {
            return new Residential(name, price, rent, houseCost, houseColour);
        }
            
    }
}
