using Google.Protobuf;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:6543");
var server = new Organism.OrganismClient(channel);

var reply = await server.createCellAsync(new CellInfo {
    Id = new UUID { Value = ByteString.CopyFrom(Guid.NewGuid().ToByteArray()) },
    Type = 1,
    Size = 56,
    Speed = 35.5f,
    DivideRate = 3000,
    X = 500,
    Y = 400,
    DivisionCounter = 0,
    Angle = 2
});

Console.WriteLine(reply);