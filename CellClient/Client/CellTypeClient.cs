using CellLibrary;
using CellLibrary.Simulator;
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
        private ConcurrentDictionary<Guid, T> cells = new();
        private readonly OrganismInfo serverInfo;
        static readonly int cellFrameTimeMillis = 1000;
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
            foreach(var cell in cells.Values) {
                var cellInfo = await organism.getCellInfoAsync(new GetCellInfoRequest { Self = cell.Id.ToMessage(), About = cell.Id.ToMessage() });
                if (cellInfo.Outcome.Result == ActionResult.CellDead) {
                    cells.TryRemove(cell.Id, out var _);
                    continue;
                }
                if (cellInfo.Outcome.Result != ActionResult.Ok)
                    throw new Exception("Cannot get self info");

                if ((CellType)cellInfo.Info.Type == CellType.MACROPHAGE) {
                    if (cellInfo.Info.Target is null) {
                        var scan = await organism.findCellsNearbyAsync(new LocationRequest { Distance = 350, From = cellInfo.Info.Id })!;
                        var target = scan.Cells.Where(c => (CellType)c.Cell.Type == CellType.BACTERIA && !c.Cell.IsTargeted).OrderBy(c => c.Distance);
                        if (target.Count() > 0) {
                            var t = target.First();
                            var res = await organism.setTargetAsync(new TargetRequest { Self = cellInfo.Info.Id, Target = t.Cell.Id })!;
                            if (res.Result != ActionResult.Ok) throw new Exception("Could not set target cell");
                        }
                    } else {
                        var target = await organism.getCellInfoAsync(new GetCellInfoRequest { Self = cellInfo.Info.Id, About = cellInfo.Info.Target })!;
                        if(target.Info is CellInfo t) {
                            if((new Vector2(t.X, t.Y) - new Vector2(cellInfo.Info.X, cellInfo.Info.Y)).Length() <= 1) {
                                Task.WaitAll(organism.killCellAsync(target.Info.Id).ResponseAsync, organism.killCellAsync(cell.Id.ToMessage()).ResponseAsync);
                                cells.TryRemove(cellInfo.Info.Id.FromMessage(), out var _);
                            }
                        }
                    }
                } 

                if((cell.divisionCounter += delta) > cell.DivideRate && serverInfo.MaxCellsOfType > cells.Count) {
                    cell.divisionCounter = 0;
                    if (Random.Shared.NextDouble() < 0.3) {
                        await RegisterCell(new(cellInfo.Info.X, cellInfo.Info.Y));
                        cell.DivideRate += 0.2f * cell.DivideRate;
                    }
                }
            }
        }

        private async Task<T> RegisterCell(Vector2 pos) {
            var cell = new T {
                Position = pos,
            };

            cells.TryAdd(cell.Id, cell);
            await organism.createCellAsync(cell.ToCellInfo());
            return cell;
        }
    }
}
