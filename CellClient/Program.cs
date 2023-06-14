using CellClient.Client;
using CellLibrary.Simulator;

string? server = Environment.GetEnvironmentVariable("SERVER_ADDRESS");
string? port = Environment.GetEnvironmentVariable("SERVER_PORT");

if (server == null) {
    Console.Error.WriteLine("Please set the SERVER_ADDRESS variable");
    return 1;
}

if (port == null) {
    port = "6543";
}
else if (!int.TryParse(port, out var _)) {
    Console.Error.WriteLine("Port number must be an integer");
    return 2;
}

string url = "https://" + server + ":" + port + "/";

if (args.Length < 1) {
    Console.WriteLine("Set command line argument to the name of agent");
    return 3;
}

string agentType = args[0].ToUpper();

List<String> knownAgents = new List<string>
{
    "BACTERIA", "MACROPHAGE", "LEUKOCYTE"
};

if (!knownAgents.Contains(agentType)) {
    Console.WriteLine("Agent doesn't exist");
    return 4;
}

Console.WriteLine("Starting " + agentType + " agent");
Console.WriteLine("Connecting to the server: " + url);


switch (agentType) {
    case "BACTERIA":
        var bacteria = new CellTypeClient<Bacteria>(url);
        await bacteria.Start();
        break;
    case "MACROPHAGE":
        var macrophage = new CellTypeClient<Macrophage>(url);
        await macrophage.Start();
        break;
    case "LEUKOCYTE":
        var leukocyte = new CellTypeClient<Leukocyte>(url);
        await leukocyte.Start();
        break;
}


return 0;