﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellSimulator.Simulator {
    internal class Macrophage : Cell {
        public override string Name => "Makrofag";
        public override string SVGSprite => "<svg viewBox=\"0 0 100 100\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <defs></defs>\r\n  <circle cx=\"50\" cy=\"50\" r=\"20\" style=\"fill: #770077\" /></svg>";
        public override int Size { get; set; }
        public override float Speed { get; set; }
        public Macrophage(Vector2 p, float angle) : base(p, angle) {
            Speed = Random.Shared.NextSingle() * (13.0f - 6f) + 6f;
            Size = Random.Shared.Next(93, 150);
        }
    }
}
