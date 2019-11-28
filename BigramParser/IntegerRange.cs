using System;

namespace BigramParser
{
    public class IntegerRange
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public IntegerRange(int min, int max)
        {
            if (min >= max)
            {
                throw new ArgumentException("Minimum value cannot be greater than or equal to Maximum value.");
            }

            Min = min;
            Max = max;
        }

        public bool ContainsValue(int val)
        {
            return val >= Min && val <= Max;
        }

        public override string ToString()
        {
            return "(" + Min + "-" + Max + ")";
        }
    }
}
