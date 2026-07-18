using System;

namespace ImageStitcher
{
    public class RandomLCG
    { // https://gist.github.com/ekepes/8aed1310c3e7af31c99d
        private const long m = 4294967296; // aka 2^32
        private const long a = 1664525;
        private const long c = 1013904223;
        private long _last;

        public RandomLCG()
        {
            _last = DateTime.Now.Ticks % m;
        }

        public RandomLCG(long seed)
        {
            _last = seed;
        }

        public long Next()
        {
            _last = ((a * _last) + c) % m;

            return _last;
        }

        public long Next(long maxValue)
        {
            return Next() % maxValue;
        }
    }
}