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

            var percentageAmountAccounts = new List<Account>();
            decimal totalRequiredFixedSavings = 0;
            decimal totalPercentageSavings = 0;
            foreach (var account in selectedAccounts)
            {
                if (account.SavingsType == GoalSavingsType.Fixed)
                {
                    decimal thisSavingsAmount = account.SavingsAmount ?? 0;
                    if (account.FixedGoal != null && !account.ContinueSavingAfterGoalMet)
                    {
                        decimal untilGoalMet = (account.FixedGoal ?? 0) - account.Balance;
                        if (untilGoalMet < thisSavingsAmount)
                            thisSavingsAmount = untilGoalMet;
                    }
                    totalRequiredFixedSavings += thisSavingsAmount;
                    results[account] = thisSavingsAmount;
                }
                else if (account.SavingsType == GoalSavingsType.Percentage)
                {
                    percentageAmountAccounts.Add(account);
                    decimal percentageThisGoal = account.SavingsAmount ?? 0;
                    if (account.FixedGoal != null && !account.ContinueSavingAfterGoalMet)
                    {
                        decimal untilGoalMet = (account.FixedGoal ?? 0) - account.Balance;
                        decimal dollarAmount = paycheckAmount * (percentageThisGoal / 100);
                        if(dollarAmount > untilGoalMet)
                        {
                            percentageThisGoal = untilGoalMet / paycheckAmount;
                        }
                    }
                    totalPercentageSavings += percentageThisGoal;
                }
            }
            if (paycheckAmount < totalRequiredFixedSavings)
                throw new PaycheckDistributionError($"Not enough money to satisfy fixed goals. " +
                    $"Required: {totalRequiredFixedSavings:C}, provided: {paycheckAmount:C}");
            if (totalPercentageSavings > 100)
                throw new PaycheckDistributionError($"Percentage goals sum to more than 100% ({totalPercentageSavings}%)");
            // Calculate the MINIMUM required for the precentage-amount goals
            decimal minForPercentages = MoneyMath.Floor(paycheckAmount * (totalPercentageSavings / 100));
            if (minForPercentages + totalRequiredFixedSavings > paycheckAmount)
                throw new PaycheckDistributionError($"Not enough income to satisfy both percentage and fixed goals. " +
                    $"(Required: {MinimumRequired(selectedAccounts):C}, provided: {paycheckAmount:C})");
            // Give each percentage goal MINIMUM amount of money, calculate remainder
            decimal percentageGoalsDistributed = 0;
            foreach (var percentageGoal in percentageAmountAccounts)
            {
                decimal thisGoalMin = MoneyMath.Floor(paycheckAmount * ((percentageGoal.SavingsAmount ?? 0) / 100));
                if(percentageGoal.FixedGoal != null && !percentageGoal.ContinueSavingAfterGoalMet)
                {
                    decimal untilGoalMet = (percentageGoal.FixedGoal ?? 0) - percentageGoal.Balance;
                    if (untilGoalMet < thisGoalMin)
                        thisGoalMin = untilGoalMet;
                }
                percentageGoalsDistributed += thisGoalMin;
                results[percentageGoal] = thisGoalMin;
            }
            // Evenly distribute remainder across all accounts
            decimal remainder = MoneyMath.Floor(minForPercentages - percentageGoalsDistributed);
            int iterator = 0;
            while(iterator > percentageAmountAccounts.Count)
            {
                if (remainder < 1) break;
                var thisGoal = percentageAmountAccounts[iterator];
                // Prevent adding remainder to full accounts
                if(thisGoal.FixedGoal != null && !thisGoal.ContinueSavingAfterGoalMet)
                {
                    if(thisGoal.Balance < (thisGoal.FixedGoal ?? 0))
                        results[percentageAmountAccounts[iterator]]++;
                }
                else
                {
                    results[percentageAmountAccounts[iterator]]++;
                }
                iterator++;
            }
            return results;
        }

        public static decimal MinimumRequired(List<Account> accounts)
        {
            decimal totalRequiredFixedSavings = 0;
            decimal totalPercentageSavings = 0;
            foreach (var account in accounts)
            {
                if (account.SavingsType == GoalSavingsType.Fixed)
                {
                    decimal amountForThisAccount = account.SavingsAmount ?? 0;
                    if(account.FixedGoal != null && !account.ContinueSavingAfterGoalMet)
                    {
                        decimal untilGoalMet = (account.FixedGoal ?? 0) - account.Balance;
                        if (amountForThisAccount > untilGoalMet)
                            amountForThisAccount = untilGoalMet;
                    }
                    totalRequiredFixedSavings += amountForThisAccount;
                }
                else if (account.SavingsType == GoalSavingsType.Percentage)
                {
                    totalPercentageSavings += account.SavingsAmount ?? 0;
                }
            }
            if(totalPercentageSavings == 0 || totalPercentageSavings > 100)
            {
                return totalRequiredFixedSavings;
            }
            else
            {
                decimal min = MoneyMath.Floor(totalRequiredFixedSavings / (1 - (totalPercentageSavings / 100)));
                // Now we subtract any goals that were met or will be met
                foreach(var account in accounts)
                {
                    if(account.FixedGoal != null && !account.ContinueSavingAfterGoalMet)
                    {
                        decimal untilGoalMet = (account.FixedGoal ?? 0) - account.Balance;
                        decimal minPayment = MoneyMath.Floor(((account.SavingsAmount ?? 0) / 100) * min);
                        if (untilGoalMet < minPayment)
                            min -= minPayment - untilGoalMet; // Just take the difference off
                    }
                }
                return min;
            }
        }

    }
}
