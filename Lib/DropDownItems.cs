using System.Collections.Generic;

namespace Goalie.Lib
{
    class DropDownItems
    {
        public static Dictionary<string, GoalSavingsType> GoalSavingsTypes = new Dictionary<string, GoalSavingsType>()
        {
            { "% (percentage per paycheck)", GoalSavingsType.Percentage },
            { "$ (fixed amt / paycheck)", GoalSavingsType.Fixed },
            { "Manual (nothing per paycheck)", GoalSavingsType.Manual },
        };
    }
}
