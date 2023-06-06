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
using Google.Protobuf;
using System.Runtime.CompilerServices;

namespace CellLibrary.Simulator {
    public enum CellType {
        BACTERIA = 1,
        LEUKOCYTE = 2,
        ANTIBODY = 3,
        MACROPHAGE = 4
    }

    public abstract class Cell : IEqualityComparer<Cell> {
        public abstract string Name { get; }
        public abstract string SVGSprite { get; }
        public abstract float Size { get; set; }
        public abstract float DivideRate { get; set; }
        public abstract Vector2 Speed { get; set; }

        public Vector2 Position { get; set; } = new(0, 0);
        public bool Dead { get; set; } = false;
        public bool IsTargeted { get; set; } = false;
        public Guid Id { get; set; } = Guid.NewGuid();
        public virtual float LayerDepth { get; set; } = 0.5f;
        public Guid? Target { get; set; } = null;
        public float divisionCounter = 0;

        public bool Equals(Cell? x, Cell? y) {
            return x?.Id == y?.Id;
        }

        public int GetHashCode(Cell obj) {
            return obj.Id.GetHashCode();
        }

        public static Cell FromCellInfo(CellInfo info) {
            Cell cell = (CellType)info.Type switch {
                CellType.BACTERIA => new Bacteria(),
                CellType.LEUKOCYTE => new Leukocyte(),
                CellType.MACROPHAGE => new Macrophage(),
                CellType.ANTIBODY => new Antibody(),
                _ => throw new Exception("Invalid cell type")
            };

            cell.Position = new(info.X, info.Y);
            cell.Speed = new(info.SpeedX, info.SpeedY);
            cell.Size = info.Size;
            cell.Id = info.Id.FromMessage();
            cell.Dead = info.Dead;
            cell.Target = info.Target?.FromMessage();
            cell.IsTargeted = info.IsTargeted;

            return cell;
        }

        public CellInfo ToCellInfo() {
            var type = this switch {
                Bacteria => CellType.BACTERIA,
                Leukocyte => CellType.LEUKOCYTE,
                Macrophage => CellType.MACROPHAGE,
                Antibody => CellType.ANTIBODY,
                _ => throw new Exception("Invalid cell type")
            };

            return new CellInfo {
                Id = Id.ToMessage(),
                Type = (int)type,
                Size = Size,
                SpeedX = Speed[0],
                SpeedY = Speed[1],
                X = Position.X,
                Y = Position.Y,
                Dead = Dead,
                Target = Target?.ToMessage(),
                IsTargeted = IsTargeted
            };
        }
    }
}
