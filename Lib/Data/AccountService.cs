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
        public static DataDir GetAccountDataDir(Profile profile, string AccountID)
        {
            return new DataDir(AccountID, GetAccountsDataDir(profile));
        }
    }
}
