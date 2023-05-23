using CellLibrary.Simulator;
using Grpc.Net.Client;

namespace CellClient
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string address = args[0]; // "https://localhost:5000" - dla serwera localhost na porcie 5000

            IOrganism organism = new RemoteOrganism(address);

            // tworzenie przykładowej komórki - kod oczywiście powinien być bardziej złożony
            Leukocyte leukocyte = new Leukocyte(organism, new System.Numerics.Vector2(50, 50), 30);

            organism.createCell(leukocyte);
            // TODO other operations on the cell

            // IMPORTANT: DON't do the cell logic in the constructor, use other method for that!
        }
    }
}