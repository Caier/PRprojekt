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
            // TODO
            return base.killCell(request, context);
        }

        public override Task<LocationResponse> findCellsNearby(LocationRequest request, ServerCallContext context) {
            // TODO
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
