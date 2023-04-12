using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellSimulator.Simulator
{
    internal class Antibody : Cell
    {
        public override string Name => "Przeciwcialo";
        public override string SVGSprite => "<svg viewBox=\"0 0 100 100\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <defs></defs>\r\n  <polygon points=\"40,60 60,60 50,40\" style=\"fill:green;stroke:black;stroke-width:1\" /></svg>";
        public override int Size { get; set; }
        public override float Speed { get; set; }
        public Antibody(Vector2 p, float angle) : base(p, angle)
        {
            Speed = Random.Shared.NextSingle() * (75.0f - 60f) + 60f;
            Size = Random.Shared.Next(54, 72);
        }
    }
}
