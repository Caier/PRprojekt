
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
        public override int Size { get; set; }
        public override float Speed { get; set; }
        public override float DivideRate { get; set; } = 5;
    }
}
