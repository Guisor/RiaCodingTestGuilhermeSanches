Console.WriteLine("Please enter a payout value: ");
var payoutValue = int.Parse(Console.ReadLine());

var _cadridgesService = new DenominationRoutine.Service.CartridgesService();

_cadridgesService.PrintAllCatridgesOptionsForPayout(payoutValue);
