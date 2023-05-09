using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellSimulator.Simulator {
    internal class Leukocyte : Cell {
        public override string Name => "Leukocyt";
        public override string SVGSprite => "<svg viewBox=\"0 0 100 100\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <defs></defs>\r\n  <circle cx=\"50\" cy=\"50\" r=\"20\" style=\"fill: #ffffff\" /></svg>";
        public override int Size { get; set; }
        public override float Speed { get; set; }

        public int Range => Size*3; //radius of targeting

        public int TimeUntilNextRelease { get; set; } = 0;

        public Leukocyte(Vector2 p, float angle) : base(p, angle) {
            Speed = Random.Shared.NextSingle() * (35.0f - 14f) + 14f;
            Size = Random.Shared.Next(54, 83);
            
        }

        public List<Antibody> ReleaseAntibodies(Bacteria target)
        {
            TimeUntilNextRelease = 1000;
            List<Antibody> antibodies = new List<Antibody>();

            for (int i = 0; i < 5;i++)
            {
                double theta = i * (360.0 / 5);
                double x = Position.X + Size * Math.Cos(theta * Math.PI / 180)/2;
                double y = Position.Y + Size * Math.Sin(theta * Math.PI / 180)/2;
                Vector2 position = new Vector2((float)x, (float)y);

                antibodies.Add(new Antibody(position, Angle, target));
            }

            return antibodies;

        }
    }
}
