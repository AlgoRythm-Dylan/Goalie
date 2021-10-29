using System.Windows;
using Goalie.Lib.Models;

namespace Goalie.Lib
{
    public class AccountRoutedEventArgs : RoutedEventArgs, IShouldDelete, IShouldSave
    {
        public Account Account { get; set; }
        public bool ShouldSave { get; set; }
        public bool ShouldDelete { get; set; }

        public AccountRoutedEventArgs(RoutedEvent routedEvent, Account account) : base(routedEvent)
        {
            Account = account;
        }
    }
}
