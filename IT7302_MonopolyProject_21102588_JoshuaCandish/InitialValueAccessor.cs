
namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    static class InitialValueAccessor
    {
        private static readonly FileReader FileReader = new FileReader();

        public static decimal GetBankerStartingBanker()
        {
            return FileReader.ReadInitialValuesFromCSV()[0];
        }

        public static decimal GetPlayerStartingBanker()
        {
            return FileReader.ReadInitialValuesFromCSV()[1];
        }
    }
}
