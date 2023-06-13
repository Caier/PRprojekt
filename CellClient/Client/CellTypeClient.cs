<<<<<<< HEAD
﻿using CellLibrary;
using CellLibrary.Simulator;
=======
﻿using CellLibrary.Simulator;
>>>>>>> networking2
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CellClient.Client {
    internal class CellTypeClient<T> where T: Cell, new() {
        private Organism.OrganismClient organism;
        private ConcurrentDictionary<Guid, Cell> cells = new();
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

                if (cellInfo.Info.Type == CellType.Macrophage) {
                    if (cellInfo.Info.Target is null) {
                        var scan = await organism.findCellsNearbyAsync(new LocationRequest { Distance = 200, From = cellInfo.Info.Id })!;
                        var target = scan.Cells.Where(c => c.Cell.Type == CellType.Bacteria && !c.Cell.IsTargeted && new Vector2(c.Cell.SpeedX, c.Cell.SpeedY).Length() <= 4.5).OrderBy(c => c.Distance);
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
                } else if(cell is Leukocyte leuk) {
                    if((leuk.TimeUntilNextRelease -= delta) <= 0) {
                        var scan = await organism.findCellsNearbyAsync(new LocationRequest { Distance = 200, From = cellInfo.Info.Id })!;
                        var target = scan.Cells.Where(c => c.Cell.Type == CellType.Bacteria).OrderBy(c => c.Distance);
                        if(target.Count() > 0) {
                            var t = target.First();
                            leuk.TimeUntilNextRelease = Random.Shared.Next(15, 25);
                            Task[] antibodies = new Task[5];

                            for (int i = 0; i < 5; i++) {
                                double theta = i * (360.0 / 5);
                                double x = cellInfo.Info.X + cellInfo.Info.Size * Math.Cos(theta * Math.PI / 180) / 2;
                                double y = cellInfo.Info.Y + cellInfo.Info.Size * Math.Sin(theta * Math.PI / 180) / 2;
                                Vector2 position = new((float)x, (float)y);

                                var ant = new Antibody { Position = position, Target = t.Cell.Id.FromMessage(),
                                    Offset = new(t.Cell.Size / 2 * (float)Math.Cos(theta * Math.PI / 180) / 2, t.Cell.Size / 2 * (float)Math.Sin(theta * Math.PI / 180) / 2) 
                                };
                                antibodies[i] = Task.Run(() => {
                                    cells.TryAdd(ant.Id, ant);
                                    organism.createCell(ant.ToCellInfo());
                                });
                            }

                            Task.WaitAll(antibodies);
                        }
                    }
                }
                else if (cell is Antibody antb)
                {
                    var target = await organism.getCellInfoAsync(new GetCellInfoRequest { Self = cellInfo.Info.Id, About = cellInfo.Info.Target })!;
                    if (target.Info is CellInfo t)
                    {
                        if (t.Dead)
                        {
                            await organism.killCellAsync(cellInfo.Info.Id);
                            cells.TryRemove(cellInfo.Info.Id.FromMessage(), out var _);
                        } else {
                            if(!antb.isUsed && (new Vector2(cellInfo.Info.X, cellInfo.Info.Y) - new Vector2(t.X, t.Y)).Length() <= t.Size) {
                                await organism.changeSpeedAsync(new ChangeSpeedRequest { Self = t.Id, SpeedX = t.SpeedX * 0.8f, SpeedY = t.SpeedY * 0.8f });
                                antb.isUsed = true;
                            }
                        }
                    }
                }

                if ((cell.divisionCounter += delta) > cell.DivideRate && serverInfo.MaxCellsOfType > cells.Count) {
                    cell.divisionCounter = 0;
                    if (Random.Shared.NextDouble() < 0.3) {
                        await RegisterCell(new(cellInfo.Info.X, cellInfo.Info.Y));
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

            cells.TryAdd(cell.Id, cell);
            await organism.createCellAsync(cell.ToCellInfo());
            return cell;
        }
    }
}
