using Grpc.Core;
using System.Collections.Concurrent;
using System;
using OrganismServer.Monogame;
using Cells = CellLibrary.Simulator;
using System.Diagnostics;
using ExCSS;
using System.Dynamic;
using Google.Protobuf.WellKnownTypes;
<<<<<<< HEAD
using CellLibrary;
=======
using CellLibrary.Simulator;
>>>>>>> networking2

namespace OrganismServer.Services {
    public class OrganismService : Organism.OrganismBase, Cells.IOrganism {
        private OrganismLogic logic;
        
        public OrganismService(OrganismLogic logic, ILoggerFactory _) {
            this.logic = logic;
        }

        public override Task<ActionOutcome> createCell(CellInfo request, ServerCallContext context) {
            try {
                var c = Cells.Cell.FromCellInfo(request);
                logic.cells.TryAdd(c.Id, c);
                return Task.FromResult(new ActionOutcome { Result = ActionResult.Ok });
            }
            catch {
                return Task.FromResult(new ActionOutcome { Result = ActionResult.OtherErr, Message = "Could not add cell" });
            }
        }

        public override Task<ActionOutcome> updateSpeedVector(SpeedVectorUpdateRequest request, ServerCallContext context)
        {
            Guid guid = new Guid(request.Id.Value.Memory.ToArray());
            Cell? c = logic.cells.Select(a => a.Key)
                .Where(a => a.Id.Equals(guid))
                .First();
            if (c == null)
            {
                return Task.FromResult(new ActionOutcome { Result = ActionResult.CellDead });
            } else
            {
                c.Speed = new System.Numerics.Vector2
                {
                    X = request.Vector.SpeedX,
                    Y = request.Vector.SpeedY
                };
                // TODO concurrency control
                return Task.FromResult(new ActionOutcome { Result = ActionResult.Ok});
            }
                
        }

        public override Task<ActionOutcome> killCell(UUID request, ServerCallContext context) {
            logic.cells.TryGetValue(request.FromMessage(), out var c);
            if(c is not null) {
                c.Dead = true;
                return Task.FromResult(new ActionOutcome { Result = ActionResult.Ok });
            }
            return Task.FromResult(new ActionOutcome { Result = ActionResult.InvalidCell });
        }

        public override Task<LocationResponse> findCellsNearby(LocationRequest request, ServerCallContext context) {
            var cells = new List<CellInfoWithDistance>();
            logic.cells.TryGetValue(request.From.FromMessage(), out var from);
            if (from is null)
                return Task.FromResult(new LocationResponse { Result = new ActionOutcome { Result = ActionResult.InvalidCell } });
            if (from.Dead)
                return Task.FromResult(new LocationResponse { Result = new ActionOutcome { Result = ActionResult.CellDead } });

            foreach (var cell in logic.cells.Values) {
                var dist = (from.Position - cell.Position).Length();
                if (dist <= request.Distance && cell != from && !cell.Dead) {
                    cells.Add(new CellInfoWithDistance { Cell = cell.ToCellInfo(), Distance = (float)dist });
                }
            }

            var response = new LocationResponse {
                Result = new ActionOutcome { Result = ActionResult.Ok },
                Self = from.ToCellInfo(),
            };

            response.Cells.AddRange(cells);

            return Task.FromResult(response);
        }

        public override Task<OrganismInfo> getOrganismInfo(Empty _, ServerCallContext context) {
            return Task.FromResult(new OrganismInfo {
                Height = logic.organismDrawer.viewPort.Height,
                Width = logic.organismDrawer.viewPort.Width,
                MaxCellsOfType = logic.maxCells
            });
        }

<<<<<<< HEAD
        public override Task<GetCellInfoResponse> getCellInfo(GetCellInfoRequest request, ServerCallContext context) {
            logic.cells.TryGetValue(request.Self.FromMessage(), out var self);
            if(self is null)
                return Task.FromResult(new GetCellInfoResponse { Outcome = new ActionOutcome { Result = ActionResult.InvalidCell } });
            if(self.Dead)
                return Task.FromResult(new GetCellInfoResponse { Outcome = new ActionOutcome { Result = ActionResult.CellDead } });
            logic.cells.TryGetValue(request.About.FromMessage(), out var c);
            if (c is null)
                return Task.FromResult(new GetCellInfoResponse { Outcome = new ActionOutcome { Result = ActionResult.InvalidCell } });
            return Task.FromResult(new GetCellInfoResponse { Outcome = new ActionOutcome { Result = ActionResult.Ok }, Info = c.ToCellInfo() });
        }

        public override Task<ActionOutcome> setTarget(TargetRequest request, ServerCallContext context) {
            logic.cells.TryGetValue(request.Self.FromMessage(), out var self);
            if(self is null)
                return Task.FromResult(new ActionOutcome { Result = ActionResult.InvalidCell });
            if(self.Dead)
                return Task.FromResult(new ActionOutcome { Result = ActionResult.CellDead });

            if (request.Target is null)
                self.Target = null;
            else {
                logic.cells.TryGetValue(request.Target.FromMessage(), out var other);
                if(other is null)
                    return Task.FromResult(new ActionOutcome { Result = ActionResult.InvalidCell });
                self.Target = request.Target.FromMessage();
                other.IsTargeted = true;
            }

            return Task.FromResult(new ActionOutcome { Result = ActionResult.Ok });
        }

        public override Task<ActionOutcome> changeSpeed(ChangeSpeedRequest request, ServerCallContext context) {
            logic.cells.TryGetValue(request.Self.FromMessage(), out var self);
            if (self is null)
                return Task.FromResult(new ActionOutcome { Result = ActionResult.InvalidCell });
            if (self.Dead)
                return Task.FromResult(new ActionOutcome { Result = ActionResult.CellDead });
            self.Speed = new(request.HasSpeedX ? request.SpeedX : self.Speed.X, request.HasSpeedY ? request.SpeedY : self.Speed.Y);
            return Task.FromResult(new ActionOutcome { Result = ActionResult.Ok });
        }
=======
>>>>>>> networking2
    }
}
