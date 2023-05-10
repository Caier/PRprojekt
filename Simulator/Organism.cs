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
        }

        internal void StartLife() {
            for (int i = 0; i < 2; i++) {
                var s = organismDrawer.viewPort;
                cells.TryAdd(new Bacteria(this, new((float)Random.Shared.Next(0, s.Width), (float)Random.Shared.Next(0, s.Height)), (float)(Random.Shared.NextSingle() * 2 * Math.PI)), null);
                cells.TryAdd(new Macrophage(this, new((float)Random.Shared.Next(0, s.Width), (float)Random.Shared.Next(0, s.Height)), (float)(Random.Shared.NextSingle() * 2 * Math.PI)), null);
                cells.TryAdd(new Leukocyte(this, new((float)Random.Shared.Next(0, s.Width), (float)Random.Shared.Next(0, s.Height)), (float)(Random.Shared.NextSingle() * 2 * Math.PI)), null);
            }
        }

        internal void Update(GameTime time) {
            
        }
    }
}
