﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CellSimulator.Monogame;
using Microsoft.Xna.Framework;

namespace CellSimulator.Simulator {
    public class Organism {
        internal ConcurrentDictionary<Cell, byte?> cells = new();
        OrganismGraphicRepresentation organismDrawer;

        public Organism() {
            organismDrawer = new(this);
            cells.TryAdd(new Bacteria(new(400, 100), 3.234f), null);
            new Thread(organismDrawer.Run).Start();
            OrganismLife();
        }

        //wołane z OrganismGraphicRepresentation, przeznaczone do aktualizowania stanu niezwiązanego z wyglądem - pozycja itp.
        internal void Update(GameTime time) {
            float delta = (float)time.ElapsedGameTime.TotalSeconds;

            foreach(var cell in cells.Keys) {
                if (cell.Position.X > organismDrawer.viewPort.Width || cell.Position.X < 0)
                    cell.Angle = -cell.Angle;
                if (cell.Position.Y > organismDrawer.viewPort.Height || cell.Position.Y < 0)
                    cell.Angle = (float)Math.PI - cell.Angle;
                cell.Position += new Vector2((float)Math.Sin(cell.Angle) * cell.Speed * delta, (float)-Math.Cos(cell.Angle) * cell.Speed * delta);
            }
        }

        private async void OrganismLife() {
            while(true) {
                var r = Random.Shared.NextDouble();

                if (r < 0.1) {
                    var c = cells.ElementAt(Random.Shared.Next(0, cells.Count - 1)).Key;
                    cells.TryAdd(c.Divide(), null);
                }

                await Task.Delay(500);
            }
        }
    }
}
