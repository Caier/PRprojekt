using Grpc.Core;
using System.Collections.Concurrent;
using System;
using OrganismServer.Monogame;
using Cells = CellLibrary.Simulator;
using System.Diagnostics;
using ExCSS;
using System.Dynamic;
using Google.Protobuf.WellKnownTypes;

namespace OrganismServer.Services {
    public class OrganismService : Organism.OrganismBase, Cells.IOrganism {
        private OrganismLogic logic;
        
        public OrganismService(OrganismLogic logic, ILoggerFactory _) {
            this.logic = logic;
        }

        public override Task<ActionResult> createCell(CellInfo request, ServerCallContext context) {
            try {
                var c = Cells.Cell.FromCellInfo(request);
                logic.cells.TryAdd(c, null);
                return Task.FromResult(new ActionResult { Result = 0 });
            }
            catch {
                return Task.FromResult(new ActionResult { Result = -1 });
            }
        }

        public override Task<ActionResult> killCell(UUID request, ServerCallContext context) {
            var guid = new Guid(request.Value.Memory.ToArray());
            var c = logic.cells.Keys.Where(c => c.Id == guid).First();
            if(c is not null) {
                c.Dead = true;
                return Task.FromResult(new ActionResult { Result = 0 });
            }
            return Task.FromResult(new ActionResult { Result = -1 });
        }

        public override Task<LocationResponse> findCellsNearby(LocationRequest request, ServerCallContext context) {
            /*var cells = new List<CellInfo>();
            var from = logic.cells.Keys.Where(c => c.Id == new Guid(request.From.Value.Memory.ToArray())).First();
            if (from is null) {
                return Task.FromResult(new LocationResponse { Result = new ActionResult { Result = -1 } });
            }
            foreach (var cell in logic.cells.Keys) {
                if (Math.Sqrt(Math.Pow(from.Position.X - cell.Position.X, 2) 
                    + Math.Pow(from.Position.Y - cell.Position.Y, 2)) <= request.Distance) {
                    cells.Add(cell.ToCellInfo());
                }
            }

            var response = new LocationResponse {
                Result = new ActionResult { Result = 0 },
                Self = from.ToCellInfo(),
            };
            return Task.FromResult();*/
            return base.findCellsNearby(request, context);
        }

        public override Task<OrganismInfo> getOrganismInfo(Empty _, ServerCallContext context) {
            return Task.FromResult(new OrganismInfo {
                Height = logic.organismDrawer.viewPort.Height,
                Width = logic.organismDrawer.viewPort.Width,
                MaxCellsOfType = logic.maxCells
            });
        }
    }
}
