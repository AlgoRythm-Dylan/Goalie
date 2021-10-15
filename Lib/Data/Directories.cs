using System;
using System.IO;

namespace Goalie.Lib.Data
{
    class Directories
    {
        public static DataDir GoalieApp = new DataDir(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".goalie"));
        public static DataDir Profiles = new DataDir("profiles", GoalieApp);
    }
}
