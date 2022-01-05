using System;

namespace BobReactRemaster.Helper
{
    public class RandomGenerator : IRandomGenerator
    {
        private Random RNG; 
        public RandomGenerator()
        {
            RNG = new Random();
        }
        public int Generate(int min, int max)
        {
            return RNG.Next(min, max);
        }
    }
}
