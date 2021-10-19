using Goalie.Lib.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Goalie.Lib.Data
{
    public class ProfileService : ServiceBase
    {
        public static async Task<List<Profile>> ReadAllAsync()
        {
            return await Directories.Profiles.ReadAllItemsAsync<Profile>();
        }
        public static async Task<Profile> ReadAsync(string ID)
        {
            try
            {
                return JsonSerializer.Deserialize<Profile>(
                    await File.ReadAllTextAsync(
                        Path.Combine(Directories.Profiles.Path, $"{ID}.profile.json")
                    )
                );
            }
            catch
            {
                return null;
            }
        }
        public static async Task WriteAsync(Profile profile)
        {
            Directories.Profiles.Ensure();
            await File.WriteAllTextAsync(
                Path.Combine(Directories.Profiles.Path, $"{profile.ID}.profile.json"),
                JsonSerializer.Serialize(profile)
            );
        }
        public static DataDir GetDataDir(string ID)
        {
            return new DataDir(ID, Directories.Profiles);
        }
        public static DataDir GetDataDir(Profile profile)
        {
            return GetDataDir(profile.ID);
        }
    }
}
