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
        public abstract int Size { get; set; }
        public abstract float Speed { get; set; }
        public abstract float DivideRate { get; set; }

        public Vector2 Position { get; set; } = new(0, 0);
        public bool Dead { get; set; } = false;
        public Guid Id { get; set; } = Guid.NewGuid();
        public float Angle { get; set; } = 0;
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

            cell.Angle = info.Angle;
            cell.Position = new(info.X, info.Y);
            cell.Speed = info.Speed;
            cell.Size = info.Size;
            cell.Id = new Guid(info.Id.Value.Memory.ToArray());

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
                Id = new UUID { Value = ByteString.CopyFrom(Id.ToByteArray()) },
                Type = (int)type,
                Size = Size,
                Speed = Speed,
                DivideRate = DivideRate,
                X = Position.X,
                Y = Position.Y,
                Angle = Angle
            };
        }
    }
}
