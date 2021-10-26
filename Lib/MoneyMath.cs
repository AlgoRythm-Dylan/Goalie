using System;

namespace Goalie.Lib
{
    // Normal math stuff but respects two decimal spaces
    public class MoneyMath
    {
        // simply cuts off sub-pennies
        public static decimal Clamp(decimal amt)
        {
            return Floor(amt);
        }

        public static decimal Floor(decimal amt)
        {
            return Math.Floor(amt * 100) / 100;
        }

        public static decimal Round(decimal amt)
        {
            return Math.Round(amt * 100) / 100;
        }
    }
}
