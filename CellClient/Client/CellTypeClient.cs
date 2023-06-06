using CellLibrary.Simulator;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CellClient.Client {
    internal class CellTypeClient<T> where T: Cell, new() {
        private Organism.OrganismClient organism;
        private ConcurrentDictionary<T, byte?> cells = new();
        private readonly OrganismInfo serverInfo;
        static readonly int cellFrameTimeMillis = 1000 / 60;
        internal CancellationTokenSource shutdown = new();

        public CellTypeClient(string serverAddr) {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            // akceptacja TLS nawet dla samopodpisanych certyfikatów
            var httpClient = new HttpClient(httpClientHandler);

            var ch = GrpcChannel.ForAddress(serverAddr,
                new GrpcChannelOptions { HttpClient = httpClient });
            organism = new(ch);
            serverInfo = organism.getOrganismInfo(new Empty());
        }

        public async Task Start() {
            for(int i = 0; i < 2; i++)
                await RegisterCell(new(Random.Shared.Next(0, serverInfo.Width), Random.Shared.Next(0, serverInfo.Height)));

            while (!shutdown.IsCancellationRequested) {
                Life(cellFrameTimeMillis / 1000f);
                Thread.Sleep(cellFrameTimeMillis);
            }
        }

        private async void Life(float delta) {
            foreach(var cell in cells.Keys) {
                if((cell.divisionCounter += delta) > cell.DivideRate && serverInfo.MaxCellsOfType > cells.Count) {
                    cell.divisionCounter = 0;
                    if (Random.Shared.NextDouble() < 0.3) {
                        await RegisterCell(new(cell.Position.X, cell.Position.Y));
                        cell.DivideRate += 0.2f * cell.DivideRate;
                    }
                    if(Random.Shared.NextDouble() > 0.5)
                    {
                        
                        await UpdateCellSpeed(cell, 0, 0);
                    }
                }
            }
        }

        private async Task<ActionOutcome> UpdateCellSpeed(T cell, float newX, float newY)
        {
          ActionOutcome outcome =
                await organism.updateSpeedVectorAsync(new SpeedVectorUpdateRequest
            {
                Id = new UUID
                {
                    Value = ByteString.CopyFrom(cell.Id.ToByteArray())
                },
                Vector = new SpeedVector
                {
                    Exists = true,
                    SpeedX = newX,
                    SpeedY = newY
                }
            });
            return outcome;
        }

        private async Task<T> RegisterCell(Vector2 pos) {
            var cell = new T {
                Position = pos,
            };

            cells.TryAdd(cell, null);
            await organism.createCellAsync(cell.ToCellInfo());
            return cell;
        }
    }
}
