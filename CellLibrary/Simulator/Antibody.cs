﻿
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CellLibrary.Simulator {
    public class Antibody : Cell
    {
        public override string Name => "Przeciwcialo";
        public override string SVGSprite => "<svg viewBox=\"0 0 100 100\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <defs></defs>\r\n  <polygon points=\"40,60 60,60 50,40\" style=\"fill:green;stroke:black;stroke-width:1\" /></svg>";
        public override float Size { get; set; } = Random.Shared.Next(54, 72);
        public override Vector2 Speed { get; set; } = new(35, 35);
        public override float DivideRate { get; set; } = float.PositiveInfinity;

        public bool isAttached { get; set; } = false;
        public bool isUsed = false;
        private Vector2 attachedPosition;
    }
      
}
