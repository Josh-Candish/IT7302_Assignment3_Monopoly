using System;

namespace IT7302_MonopolyProject_21102588_JoshuaCandish
{
    public class Die
    {
        private static readonly Random NumGenerator = new Random();
        private static int _numberRolled;


        public int Roll()
        {
            _numberRolled = NumGenerator.Next(1, 7);
            return _numberRolled;
        }

        public int NumberLastRolled()
        {
            return _numberRolled;
        }

        public override string ToString()
        {
            return _numberRolled.ToString();
        }
    }
}
