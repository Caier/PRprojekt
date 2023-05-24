using Grpc.Core;
using System.Collections.Concurrent;
using System;
using OrganismServer.Monogame;
using CellLibrary.Simulator;
using System.Diagnostics;
using ExCSS;
using System.Dynamic;

namespace OrganismServer.Services {
    public class OrganismService : Organism.OrganismBase, IOrganism {
        private OrganismLogic logic;
        
        public OrganismService(OrganismLogic logic, ILoggerFactory _) {
            this.logic = logic;
        }

        public override Task<ActionResult> createCell(CellInfo request, ServerCallContext context) {
            var b = new Bacteria {
                Size = request.Size,
                Speed = request.Speed,
                Position = new(request.X, request.Y),
                Angle = request.Angle,
                Id = new Guid(request.Id.Value.Memory.ToArray())
            };

            logic.cells.TryAdd(b, null);

            return Task.FromResult(new ActionResult { Result = 0 });
        }

        public override Task<ActionResult> killCell(UUID request, ServerCallContext context) {
            // TODO
            return base.killCell(request, context);
        }

        public override Task<LocationResponse> findCellsNearby(LocationRequest request, ServerCallContext context) {
            // TODO
            return base.findCellsNearby(request, context);
        }
    }
}
