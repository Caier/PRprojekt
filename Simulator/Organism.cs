using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CellLibrary.Simulator;
using CellSimulator.Monogame;
using Grpc.Core;
using Microsoft.Xna.Framework;
using static OrganismService;

namespace CellSimulator.Simulator  {
    public class Organism : OrganismServiceBase, IOrganism
    {
        internal ConcurrentDictionary<Cell, byte?> cells = new();
        internal OrganismGraphicRepresentation organismDrawer;
        internal CancellationTokenSource shutdown = new();
        internal readonly int maxCells = 150;
        internal CellFactory cellFactory;

        public Organism() {
            organismDrawer = new(this);
            cellFactory = new CellFactory();
            new Thread(organismDrawer.Run).Start();
            
        }

        internal void StartLife() {
            

        }

        internal void Update(GameTime time) {
            
        }


        public override Task<ActionResult> createCell(CellInfo request, ServerCallContext context)
        {
            Cell cell = cellFactory.createCellFromCellInfo(this, request);
            if (cell == null)
            {
                cells.TryAdd(cell, null);

            }
            ActionResult actionResult = new ActionResult();
            actionResult.Result = 0; // SUCCESS (one option for success, many options for failure)
            return Task.FromResult(actionResult);
        }

        public override Task<ActionResult> killCell(UUID request, ServerCallContext context)
        {
            // TODO
            return base.killCell(request, context);
        }

        public override Task<LocationResponse> findCellsNearby(LocationRequest request, ServerCallContext context)
        {
            // TODO
            return base.findCellsNearby(request, context);
        }
    }
}
