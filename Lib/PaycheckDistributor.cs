using Goalie.Lib.Models;
using System;
using System.Collections.Generic;

namespace Goalie.Lib
{
    public class PaycheckDistributionError : ArgumentOutOfRangeException
    {
        public PaycheckDistributionError(string message) : base(null, message) { }
    }
    public class PaycheckDistributor
    {
        public static Dictionary<Account, decimal> DistributePaycheck(List<Account> selectedAccounts, decimal paycheckAmount)
        {
            var results = new Dictionary<Account, decimal>();
            // Sum the exact amount of money required for fixed-amount goals. Make sure there is enough
            var fixedAmountAccounts = new List<Account>();
            var percentageAmountAccounts = new List<Account>();
            decimal totalRequiredFixedSavings = 0;
            decimal totalPercentageSavings = 0;
            foreach (var account in selectedAccounts)
            {
                if (account.SavingsType == GoalSavingsType.Fixed)
                {
                    fixedAmountAccounts.Add(account);
                    totalRequiredFixedSavings += account.SavingsAmount ?? 0;
                    results[account] = account.SavingsAmount ?? 0;
                }
                else if (account.SavingsType == GoalSavingsType.Percentage)
                {
                    percentageAmountAccounts.Add(account);
                    totalPercentageSavings += account.SavingsAmount ?? 0;
                }
            }
            if (paycheckAmount < totalRequiredFixedSavings)
                throw new PaycheckDistributionError($"Not enough money to satisfy fixed goals. " +
                    $"Required: {totalRequiredFixedSavings:C}, provided: {paycheckAmount:C}");
            if (totalPercentageSavings > 100)
                throw new PaycheckDistributionError($"Percentage goals sum to more than 100% ({totalPercentageSavings}%)");
            // Calculate the MINIMUM required for the precentage-amount goals
            decimal minForPercentages = Math.Floor(paycheckAmount * (totalPercentageSavings / 100));
            if (minForPercentages + totalRequiredFixedSavings > paycheckAmount)
                throw new PaycheckDistributionError($"Not enough income to satisfy both percentage and fixed goals. " +
                    $"(Required: {minForPercentages + totalRequiredFixedSavings:C}, provided: {paycheckAmount:C})");
            // Give each percentage goal MINIMUM amount of money, calculate remainder
            decimal percentageGoalsDistributed = 0;
            foreach (var percentageGoal in percentageAmountAccounts)
            {
                decimal thisGoalMin = Math.Floor(paycheckAmount * ((percentageGoal.SavingsAmount ?? 0) / 100));
                percentageGoalsDistributed += thisGoalMin;
                results[percentageGoal] = thisGoalMin;
            }
            // Evenly distribute remainder across all accounts
            decimal remainder = minForPercentages - percentageGoalsDistributed;
            int iterator = 0;
            while(remainder > 0)
            {
                if (remainder < 1) break;
                results[percentageAmountAccounts[percentageAmountAccounts.Count % iterator]]++;
                iterator++;
            }
            return results;
        }
    }
}
