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

        public bool Equals(Cell? x, Cell? y) {
            return x?.Id == y?.Id;
        }

        public int GetHashCode(Cell obj) {
            return obj.Id.GetHashCode();
        }
    }
}
