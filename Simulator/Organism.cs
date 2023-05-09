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
using ExCSS;
using Microsoft.Xna.Framework;

namespace CellSimulator.Simulator {
    public class Organism {
        internal ConcurrentDictionary<Cell, byte?> cells = new();
        internal float lastDelta = 0;
        internal OrganismGraphicRepresentation organismDrawer;
        internal CancellationTokenSource shutdown = new();

        public Organism() {
            organismDrawer = new(this);
            new Thread(organismDrawer.Run).Start();
            Thread.Sleep(3000);

            cells.TryAdd(new Bacteria(this, new(400, 100), 3.234f), null);
            cells.TryAdd(new Leukocyte(this,new(150, 200), 2.221f), null);
            cells.TryAdd(new Macrophage(this, new(500, 234), 2.34f), null);
            OrganismLife();
        }

        internal void Update(GameTime time) {
            lastDelta = (float)time.ElapsedGameTime.TotalSeconds;
        }



        private async void OrganismLife() {
            while(true) {
                var r = Random.Shared.NextDouble();

                var types = cells.Keys.Select(c => c.Name).ToHashSet();
                var type = types.ElementAt(Random.Shared.Next(0, types.Count));

                if ((r < 0.3) && (type!= "Przeciwcialo")) {
                    cells.TryAdd(cells.Keys.Where(c => c.Name == type).First().Divide(), null);
                }

                await Task.Delay(500);
            }
        }
    }
}
