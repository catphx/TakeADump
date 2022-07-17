using TakeADump.Enums;
using TakeADump.Utilities;

var AuthType = new AuthType();
var ApiKey = "";
var ApiUrl = "";

Console.WriteLine($"{new string('-', 5)} Select Auth Type {new string('-', 5)}");

Console.WriteLine("[1] API KEY");
Console.WriteLine("[2] USER:PASS");

var readInput = Console.ReadKey(true).Key;

switch (readInput)
{
    case ConsoleKey.D1:
        AuthType = AuthType.APIKEY;
        break;
    case ConsoleKey.D2:
        AuthType = AuthType.USERPASS;
        break;
}

Console.Write("Input your key: ");

ApiKey = Console.ReadLine()?.Trim();

Console.Write("API Url: ");

ApiUrl = Console.ReadLine()?.Trim();

#pragma warning disable CS8604 // Possible null reference argument.
var controller = new Controller(ApiKey, ApiUrl, AuthType);
#pragma warning restore CS8604 // Possible null reference argument.

var streamFile = await controller.SendAsync(httpMethod: HttpMethod.Get);

await Extension.ConvertAndOutputAsync(streamFile);