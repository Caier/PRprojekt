using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellSimulator.Simulator
{
    internal class Leukocyte : Cell
    {

        public override string Name => "Leukocyt";
        public override string SVGSprite => "<svg viewBox=\"0 0 100 100\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <defs></defs>\r\n  <circle cx=\"50\" cy=\"50\" r=\"20\" style=\"fill: #ffffff\" /></svg>";
        public override int Size { get; set; }
        public override float Speed { get; set; }
        public Leukocyte(Vector2 p, float angle) : base(p, angle)
        {
            Speed = Random.Shared.NextSingle() * (35.0f - 14f) + 14f;
            Size = Random.Shared.Next(54, 72);
        }


    }
}
