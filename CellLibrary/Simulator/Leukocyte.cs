
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CellLibrary.Simulator {
    public class Leukocyte : Cell {
        public override string Name => "Leukocyt";
        public override string SVGSprite => "<svg viewBox=\"0 0 100 100\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <defs></defs>\r\n  <circle cx=\"50\" cy=\"50\" r=\"20\" style=\"fill: #ffffff\" /></svg>";
        public override int Size { get; set; }
        public override float Speed { get; set; }
        public override float DivideRate { get; set; } = 3.1f;

        public int Range => Size*3; //radius of targeting

        public float TimeUntilNextRelease { get; set; } = 0;
        
    }
}
