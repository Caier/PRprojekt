using CellLibrary.Simulator;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellClient
{
    internal class RemoteOrganism : IOrganism
    {

        private OrganismService.OrganismServiceClient organismServiceClient;
        private CellSerializator cellSerializator;
        // TODO put every action throught the service
        public RemoteOrganism(string address)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            // akceptacja TLS nawet dla samopodpisanych certyfikatów
            var httpClient = new HttpClient(httpClientHandler);

            var ch = GrpcChannel.ForAddress(address,
                new GrpcChannelOptions { HttpClient = httpClient });
            organismServiceClient =
                new OrganismService.OrganismServiceClient(ch);
            cellSerializator = new CellSerializator();
        }

        public void changeCellAttribute(Cell cell, CellAttribute cellAttribute, float value)
        {
            throw new NotImplementedException();
        }

        public void createCell(Cell cell)
        {
            CellInfo cellInfo = cellSerializator.serializeFullCell(cell);
            organismServiceClient.createCell(cellInfo);
        }

        public void killCell(Cell cell)
        {
            UUID uuid = new UUID();
            uuid.Value = cell.Id.ToString();
            organismServiceClient.killCell(uuid);
        }

        public void moveCell(Cell cell, float x, float y)
        {
            throw new NotImplementedException();
        }
         
    }
}
