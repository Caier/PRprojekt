using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using CellLibrary.Simulator;
using System.Numerics;

namespace CellLibrary.Simulator {
    public abstract class Cell : IEqualityComparer<Cell> {
        public abstract string Name { get; }
        public abstract string SVGSprite { get; }
        public abstract int Size { get; set; }
        public abstract float Speed { get; set; }
        public abstract float DivideRate { get; set; }

        public Vector2 Position { get; set; } = new(0, 0);
        public Guid Id { get; set; } = Guid.NewGuid();
        public float Angle { get; set; } = 0;

        protected readonly IOrganism parent;
        protected readonly int cellFrameTimeMillis = 1000 / 60;
        private float divisionCounter = 0;

        public Cell(IOrganism parent) {
            this.parent = parent;
        }

        public Cell(IOrganism parent, Vector2 pos, float angle) : this(parent) {
            Position = pos;
            Angle = angle;
        }

        public Cell Divide() {
            Cell c = (Cell)Activator.CreateInstance(this.GetType(), new object[] { parent, new Vector2(Position.X, Position.Y), Random.Shared.NextSingle() * 2 * (float)Math.PI });
            return c;
        }


        public bool Equals(Cell x, Cell y) {
            return x.Id == y.Id;
        }

        public int GetHashCode(Cell obj) {
            return obj.Id.GetHashCode();
        }
    }
}
