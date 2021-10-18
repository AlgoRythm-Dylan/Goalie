using System;
using System.IO;

namespace Goalie.Lib.Data
{
    class Directories
    {
#if (DEBUG)
        const string GOALIE_DATA_FOLDER = ".goalie-dev";
#else
        const string GOALIE_DATA_FOLDER = ".goalie";
#endif
        public static DataDir GoalieApp = new DataDir(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GOALIE_DATA_FOLDER));
        public static DataDir Profiles = new DataDir("profiles", GoalieApp);
    }
}
