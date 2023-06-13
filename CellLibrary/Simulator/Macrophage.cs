
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CellLibrary.Simulator {
    public class Macrophage : Cell {
        public override string Name => "Makrofag";
        public override string SVGSprite => "<svg viewBox=\"0 0 100 100\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <defs></defs>\r\n  <circle cx=\"50\" cy=\"50\" r=\"20\" style=\"fill: #770077\" /></svg>";
        public override float Size { get; set; } = Random.Shared.Next(93, 150);
        public override Vector2 Speed { get; set; } = new(Random.Shared.Next(-25, 25), Random.Shared.Next(-25, 25));
        public override float DivideRate { get; set; } = 3;
        public override float LayerDepth { get; set; } = 0.6f;
    }
}
