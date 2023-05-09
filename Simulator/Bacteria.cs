using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellSimulator.Simulator {
    public class Bacteria : Cell {
        public override string Name => "Bakteria";
        public override string SVGSprite => "<svg viewBox=\"0 0 48 204\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <defs></defs>\r\n  <ellipse style=\"stroke: rgb(0, 0, 0); stroke-width: 3px; fill: rgb(156, 248, 158);\" cx=\"23.974\" cy=\"100.598\" rx=\"15.411\" ry=\"96.319\"></ellipse>\r\n</svg>";
        public override int Size { get; set; }
        public override float Speed { get; set; }
        public override float DivideRate { get; set; } = 3;

        public bool isAttacked = false;

        public Bacteria(Organism parent, Vector2 p, float angle) : base(parent, p, angle) {
            Speed = Random.Shared.NextSingle() * (35.0f - 14f) + 14f;
            Size = Random.Shared.Next(54, 72);
        }
    }
}
