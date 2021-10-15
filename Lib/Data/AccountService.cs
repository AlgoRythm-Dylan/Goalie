using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Goalie.Lib.Models;

namespace Goalie.Lib.Data
{
    public class AccountService : ServiceBase
    {
        private static DataDir GetAccountsDataDir(Profile profile)
        {
            return new DataDir("accounts", ProfileService.GetDataDir(profile));
        }
        public static Task<List<Account>> ReadAll(Profile profile)
        {
            return GetAccountsDataDir(profile).ReadAllItemsAsync<Account>();
        }
        public static async Task<Account> Read(Profile profile, string ID)
        {
            return JsonSerializer.Deserialize<Account>(
                await File.ReadAllTextAsync(Path.Combine(GetAccountsDataDir(profile).Path, $"{ID}.account.json"))
            );
        }
        public static async Task Write(Profile profile, Account account)
        {
            var accountsDataDir = GetAccountsDataDir(profile);
            var accountDataDir = new DataDir(account.ID, accountsDataDir);
            accountDataDir.Ensure();
            Directory.CreateDirectory(Path.Combine("transactions", accountDataDir.Path));
            await File.WriteAllTextAsync(Path.Combine(accountsDataDir.Path, $"{account.ID}.account.json"),
                JsonSerializer.Serialize(account));
        }
    }
}
