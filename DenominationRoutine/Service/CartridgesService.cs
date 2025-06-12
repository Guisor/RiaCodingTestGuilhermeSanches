using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DenominationRoutine.Service
{
    public class CartridgesService
    {
        private int[] catridgesAvaliable = new int[] { 10, 50, 100 };

        private const int MinCatridgeValue = 10;
        private const int MiddleCatridgeValue = 50;
        private const int MaxCatridgeValue = 100;

        public int[] GetCatridgesAvaliable()
        {
            return catridgesAvaliable;
        }

        public int[] GetValidCatridgesForPayout(int payout)
        {
            List<int> validCartridges = new List<int>();

            foreach (var cartridge in catridgesAvaliable)
            {
                if (payout >= cartridge)
                {
                    validCartridges.Add(cartridge);
                }
            }

            return validCartridges.ToArray();
        }

        public void PrintAllCatridgesOptionsForPayout(int payout, string previousCadridgeAmount = "")
        {
            var validCartridges = GetValidCatridgesForPayout(payout);

            foreach (var cartridge in validCartridges)
            {
                var numberOfCadridge = GetNumberOfCartridges(payout, cartridge);

                if (IsPayoutCompleted(payout, cartridge))
                {
                    Console.WriteLine(GetFormatedTotalCartridgeString(numberOfCadridge, cartridge, previousCadridgeAmount));
                }

                if (IsThereOtherCartridgesOptions(cartridge, numberOfCadridge, payout) || IsRemainingPayoutAmount(cartridge, payout))
                {
                    PrintOtherCatridgesOptions(cartridge, numberOfCadridge, payout);
                }
            }
        }

        private string GetFormatedTotalCartridgeString(int amount, int catridgeValue, string previousTotalCatridge = "")
        {
            if (string.IsNullOrEmpty(previousTotalCatridge))
            {
                return $"{amount} x {catridgeValue} EUR";
            }
            else
            {
                return $"{previousTotalCatridge} + {amount} x {catridgeValue} EUR";
            }
        }
        
        private void PrintOtherCatridgesOptions(int catridgeValue, int numberOfCadridge, int payout)
        {
            for (int i = 1; i <= numberOfCadridge; i++)
            {
                switch (catridgeValue)
                {
                    case MiddleCatridgeValue:
                        PrintOptionsForMiddleCatridge(payout, i);
                        break;
                    case MaxCatridgeValue:
                        PrintOptionsForMaxCatridge(payout, i);
                        break;
                    default:
                        break;
                }
            }
        }

        private void PrintOptionsForMiddleCatridge(int payout, int catridgeCount)
        {
            var remainingAmount = payout - (MiddleCatridgeValue * catridgeCount);

            if (remainingAmount > 0)
            {
                var optionOfCatridgeForRemaning = remainingAmount / MinCatridgeValue;
                var previousCatridgeAmount = GetFormatedTotalCartridgeString(MiddleCatridgeValue, catridgeCount);
                Console.WriteLine(GetFormatedTotalCartridgeString(optionOfCatridgeForRemaning, MinCatridgeValue, previousCatridgeAmount));
            }
        }

        private void PrintOptionsForMaxCatridge(int payout, int catridgeCount)
        {
            var remainingAmount = payout - (MaxCatridgeValue * catridgeCount);

            string numberOfCatrigeOptions = "";

            if (remainingAmount > MiddleCatridgeValue)
            {
                var optionsUsingMiddleCatrige = remainingAmount / MiddleCatridgeValue;
                numberOfCatrigeOptions = GetFormatedTotalCartridgeString(MiddleCatridgeValue, optionsUsingMiddleCatrige);
            }

            if (remainingAmount % MiddleCatridgeValue != 0)
            {
                var optionsUsingMiddleCatrige = remainingAmount / MiddleCatridgeValue;

                var remainingAfterUsingMiddleCatriges = remainingAmount - (MiddleCatridgeValue * optionsUsingMiddleCatrige);
                var optionsUsingMinCatrige = remainingAfterUsingMiddleCatriges / MinCatridgeValue;
                var numberOfCatrigeUsingMinn = GetFormatedTotalCartridgeString(MinCatridgeValue, optionsUsingMinCatrige);

                if (!string.IsNullOrEmpty(numberOfCatrigeOptions))
                {
                    numberOfCatrigeOptions = $"{numberOfCatrigeOptions} + {numberOfCatrigeUsingMinn}";
                }
                else
                {
                    numberOfCatrigeOptions = $"{numberOfCatrigeUsingMinn}";
                }

            }

            Console.WriteLine($"{GetFormatedTotalCartridgeString(MaxCatridgeValue, catridgeCount)} + {numberOfCatrigeOptions}");
        }

        private int GetNumberOfCartridges(int payout, int cadridgValue)
        {
            int numberOfCadriges = payout / cadridgValue;

            return numberOfCadriges;
        }

        private bool IsPayoutCompleted(int payout, int cadridgValue)
        {
            return (payout % cadridgValue) == 0;
        }

        private bool IsThereOtherCartridgesOptions(int cadridgeValue, int numberOfCadriges, int payout)
        {
            return (cadridgeValue != MinCatridgeValue && numberOfCadriges > 1);
        }

        private bool IsRemainingPayoutAmount(int cadridgeValue, int payout)
        {
            return (payout % cadridgeValue) != 0;
        }

    }
}
