using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CellSimulator.Monogame;
using Microsoft.Xna.Framework;

namespace CellSimulator.Simulator {
    public class Organism {
        internal ConcurrentDictionary<Cell, byte?> cells = new();
        internal OrganismGraphicRepresentation organismDrawer;
        internal CancellationTokenSource shutdown = new();
        internal readonly int maxCells = 150;

        public Organism() {
            organismDrawer = new(this);
            new Thread(organismDrawer.Run).Start();
            Thread.Sleep(2000);

            cells.TryAdd(new Bacteria(this, new(400, 100), 3.234f), null);
            cells.TryAdd(new Leukocyte(this,new(150, 200), 2.221f), null);
            cells.TryAdd(new Macrophage(this, new(500, 234), 2.34f), null);
        }

        internal void Update(GameTime time) {
            
        }
    }
}
