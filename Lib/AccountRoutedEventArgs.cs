using System.Windows;
using Goalie.Lib.Models;

namespace Goalie.Lib
{
    public class AccountRoutedEventArgs : RoutedEventArgs
    {
        public Account Account { get; set; }
        public AccountRoutedEventArgs(RoutedEvent routedEvent, Account account) : base(routedEvent)
        {
            Account = account;
        }
    }
}
