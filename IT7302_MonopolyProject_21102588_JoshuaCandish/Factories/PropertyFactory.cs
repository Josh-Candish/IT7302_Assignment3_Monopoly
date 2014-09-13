
namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Factories
{
    public class PropertyFactory
    {
        public Property Create(string name)
        {
            return new Property(name);
        }
    }
}
