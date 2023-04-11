using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellSimulator.Simulator {
    public abstract class Cell : IEqualityComparer<Cell> {
        public abstract string Name { get; }
        public abstract string SVGSprite { get; }
        public abstract int Size { get; set; }
        public abstract float Speed { get; set; }

        public Vector2 Position { get; set; } = new(0, 0);
        public Guid Id { get; } = Guid.NewGuid();
        public float Angle { get; set; } = 0;

        public Cell() { }

        public Cell(Vector2 pos, float angle) {
            Position = pos;
            Angle = angle;
        }

        public Cell Divide() {
            Cell c = (Cell)Activator.CreateInstance(this.GetType(), new object[] { new Vector2(Position.X, Position.Y), Random.Shared.NextSingle() * 2 * (float)Math.PI });
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
