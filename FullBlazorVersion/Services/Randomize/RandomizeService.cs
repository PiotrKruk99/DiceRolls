using System;

namespace DiceRolls.Services
{
    public class RandomizeService : IRandomizeService
    {
        private readonly Random random;

        public RandomizeService()
        {
            random = new Random(Guid.NewGuid().GetHashCode());
        }
    }
}