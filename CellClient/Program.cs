using CellClient.Client;
using CellLibrary.Simulator;
using Google.Protobuf;
using Grpc.Net.Client;

if(args.Length != 2)
{
    Console.Error.WriteLine("Usage CellClient {HOSTNAME:PORT} {ORGANISM}");
    return 1;
}

var server = args[0];
var organismType = args[1].ToUpper();

if (organismType.Equals("BACTERIA"))
{
    var bc = new CellTypeClient<Bacteria>(server);
    await bc.Start();
} else if (organismType.Equals("MACROPHAGE"))
{
    var bc = new CellTypeClient<Macrophage>(server);
    await bc.Start();
} else if (organismType.Equals("LEUKOCYTE"))
{
    var bc = new CellTypeClient<Leukocyte>(server); 
    await bc.Start();
}

while (true) ;