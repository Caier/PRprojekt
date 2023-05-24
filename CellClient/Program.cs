using CellClient.Client;
using CellLibrary.Simulator;
using Google.Protobuf;
using Grpc.Net.Client;

var server = "https://localhost:6543";

new Thread(async () => {
    var bc = new CellTypeClient<Bacteria>(server);
    await bc.Start();
}).Start();

new Thread(async () => {
    var bc = new CellTypeClient<Macrophage>(server);
    await bc.Start();
}).Start();

new Thread(async () => {
    var bc = new CellTypeClient<Leukocyte>(server);
    await bc.Start();
}).Start();

while (true) ;