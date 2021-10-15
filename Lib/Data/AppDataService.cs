using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Goalie.Lib.Models;

namespace Goalie.Lib.Data
{
    public class AppDataService
    {
        public static async Task<AppData> ReadAsync()
        {
            try
            {
                return JsonSerializer.Deserialize<AppData>(
                    await File.ReadAllTextAsync(
                            Path.Combine(Directories.GoalieApp.Path, "App.json")
                        )
                    );
            }
            catch
            {
                return null;
            }
        }
        public static async Task WriteAsync(AppData appData)
        {
            Directories.GoalieApp.Ensure();
            await File.WriteAllTextAsync(
                Path.Combine(Directories.GoalieApp.Path, "App.json"),
                JsonSerializer.Serialize(appData)
            );
        }
    }
}
