
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CellLibrary.Simulator {
    public class Bacteria : Cell {
        public override string Name => "Bakteria";
        public override string SVGSprite => "<svg viewBox=\"0 0 48 204\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <defs></defs>\r\n  <ellipse style=\"stroke: rgb(0, 0, 0); stroke-width: 3px; fill: rgb(156, 248, 158);\" cx=\"23.974\" cy=\"100.598\" rx=\"15.411\" ry=\"96.319\"></ellipse>\r\n</svg>";
        public override float Size { get; set; } = Random.Shared.Next(54, 72);
        public override Vector2 Speed { get; set; } = new(Random.Shared.Next(-35, 35), Random.Shared.Next(-35, 35));
        public override float DivideRate { get; set; } = 3;
    }
}
