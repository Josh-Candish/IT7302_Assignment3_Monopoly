namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Factories
{
    public class TransportFactory : PropertyFactory
    {
        public Transport create(string name)
        {
            return new Transport(name);
        }
    }
}
