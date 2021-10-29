using Goalie.Lib.Models;
using System.IO;

namespace Goalie.Lib.Data
{
    public class AccountService : ServiceBase
    {
        private static DataDir GetAccountsDataDir(Profile profile)
        {
            return new DataDir("accounts", ProfileService.GetDataDir(profile));
        }
        public static DataDir GetAccountDataDir(Profile profile, string accountID)
        {
            return new DataDir(accountID, GetAccountsDataDir(profile));
        }
        public static void Delete(Profile profile, string accountID)
        {
            try
            {
                Directory.Delete(GetAccountDataDir(profile, accountID).Path, true);
            }
            catch { } // Profile may or may not have a folder with all it's transactions.
        }
    }
}
