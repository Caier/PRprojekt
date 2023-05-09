using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CellSimulator.Simulator {
    public abstract class Cell : IEqualityComparer<Cell> {
        public abstract string Name { get; }
        public abstract string SVGSprite { get; }
        public abstract int Size { get; set; }
        public abstract float Speed { get; set; }

        public Vector2 Position { get; set; } = new(0, 0);
        public Guid Id { get; } = Guid.NewGuid();
        public float Angle { get; set; } = 0;

        private readonly Organism parent;
        private readonly int cellFrameTimeMillis = 1000 / 60;

        public Cell(Organism parent) {
            this.parent = parent;
            new Thread(() => {
                while (!parent.shutdown.IsCancellationRequested) {
                    Life(cellFrameTimeMillis / 1000f);
                    Thread.Sleep(cellFrameTimeMillis);
                }
            }).Start();
        }

        public Cell(Organism parent, Vector2 pos, float angle) : this(parent) {
            Position = pos;
            Angle = angle;
        }

        public Cell Divide() {
            Cell c = (Cell)Activator.CreateInstance(this.GetType(), new object[] { parent, new Vector2(Position.X, Position.Y), Random.Shared.NextSingle() * 2 * (float)Math.PI });
            return c;
        }

        protected virtual void Life(float delta) {
            if (Position.X > parent.organismDrawer.viewPort.Width || Position.X < 0)
                Angle = -Angle;
            if (Position.Y > parent.organismDrawer.viewPort.Height || Position.Y < 0)
                Angle = (float)Math.PI - Angle;
            Position += new Vector2((float)Math.Sin(Angle) * Speed * delta, (float)-Math.Cos(Angle) * Speed * delta);
        }

        public bool Equals(Cell x, Cell y) {
            return x.Id == y.Id;
        }

        public int GetHashCode(Cell obj) {
            return obj.Id.GetHashCode();
        }
    }
}
