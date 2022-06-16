using System.CommandLine;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using RxRcon;

var ipOption = new Option<string>(
    "--ip",
    "Server IP");

var portOption = new Option<int>(
    "--port",
    description: "RCON port",
    getDefaultValue: () => 28016);

var passwordOption = new Option<string>(
    "--pass",
    "RCON password");

var rootCommand = new RootCommand("Connect to a Rust server via RCON")
{
    ipOption,
    portOption,
    passwordOption
};

rootCommand.SetHandler((ip, port, password) =>
{
    var client = new RconClient(ip, port, password);

    client.Messages
        .Where(x => !string.IsNullOrWhiteSpace(x.Message))
        .Select(x => x.Message)
        .Subscribe(Console.WriteLine);

    client.Connect();

    while (true)
    {
        var command = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(command)) continue;

        client.Send(command);
    }
    // ReSharper disable once FunctionNeverReturns
}, ipOption, portOption, passwordOption);

await rootCommand.InvokeAsync(args);