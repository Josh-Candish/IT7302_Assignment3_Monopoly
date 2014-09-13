namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Factories
{
    public class UtilityFactory : PropertyFactory
    {
        public Utility create(string name)
        {
            return new Utility(name);
        }
    }
}
