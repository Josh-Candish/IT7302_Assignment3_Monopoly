namespace IT7302_MonopolyProject_21102588_JoshuaCandish.Factories
{
    public class LuckFactory : PropertyFactory
    {
        public Luck create(string name, bool isPenalty, decimal amount)
        {
            return new Luck(name, isPenalty, amount);
        }
    }
}
