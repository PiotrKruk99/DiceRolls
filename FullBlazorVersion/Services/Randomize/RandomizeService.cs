using FullBlazorVersion.Enums;
using System.Security.Cryptography;

namespace FullBlazorVersion.Services.Randomize
{
    public class RandomizeService : IRandomizeService
    {
        public IEnumerable<int> GetRandomDice(int count, DiceTypeEnum diceType)
        {
            var result = new List<int>();
            int range = (int)diceType;

            for (int i = 0; i < count; i++)
            {
                int value;

                do
                {
                    byte[] bytes = new byte[4];
                    RandomNumberGenerator.Fill(bytes);
                    value = BitConverter.ToInt32(bytes, 0) & int.MaxValue;
                } while (value >= int.MaxValue - (int.MaxValue % range));

                result.Add(1 + (value % range));
            }

            return result;
        }
    }
}