namespace ExchangeRates
{
    public class Statistics
    {
        public float Min { get; private set; }
        public float Max { get; private set; }
        public float Sum { get; private set; }
        public int Count { get; private set; }
        public float Average
        {
            get
            {
                return Sum / Count;
            }
        }

        public Statistics()
        {
            Count = 0;
            Sum = 0;
            Max = float.MinValue;
            Min = float.MaxValue;
        }

        public void AddValue(float value)
        {
            Count++;
            Sum += value;
            Min = Math.Min(Min, value);
            Max = Math.Max(Max, value);
        }
    }
}