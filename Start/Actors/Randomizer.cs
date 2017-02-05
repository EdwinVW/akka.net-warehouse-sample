using System;

namespace Actors
{
    public static class Randomizer
    {
        private static Random _rnd = new Random((int)DateTime.Now.Ticks);

        public static int Next()
        {
            return _rnd.Next();
        }

        public static int Next(int maxValue)
        {
            return _rnd.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return _rnd.Next(minValue, maxValue);
        }
    }
}
