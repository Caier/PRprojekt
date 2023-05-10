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
        public override float DivideRate { get; set; } = 3.1f;

        public int Range => Size*3; //radius of targeting

        public float TimeUntilNextRelease { get; set; } = 0;

        public Leukocyte(Organism parent, Vector2 p, float angle) : base(parent, p, angle) {
            Speed = Random.Shared.NextSingle() * (35.0f - 14f) + 14f;
            Size = Random.Shared.Next(54, 83);
        }

        protected override void Life(float delta) {
            Leukocyte leukocyte = this;

            if ((leukocyte.TimeUntilNextRelease -= delta) <= 0) {

                var closestDistance = double.PositiveInfinity;
                Bacteria targetBacteria = null;
                foreach (var c in parent.cells.Keys) {
                    if (c is Bacteria b && !b.isAttacked) {
                        double distance = (float)Math.Sqrt(Math.Pow(c.Position.X - leukocyte.Position.X, 2.00) + Math.Pow(c.Position.Y - leukocyte.Position.Y, 2.00));
                        if ((distance < leukocyte.Range) && (distance < closestDistance)) //encountered bacteria; targets closest one within range
                        {
                            closestDistance = distance;
                            targetBacteria = (Bacteria)c;
                        }
                    }
                }

                if (targetBacteria != null) //releasing antibodies
                {
                    List<Antibody> antibodies = leukocyte.ReleaseAntibodies(targetBacteria);
                    foreach (Antibody a in antibodies) {
                        parent.cells.TryAdd(a, null);
                    }
                }

            }

            base.Life(delta);
        }

        public List<Antibody> ReleaseAntibodies(Bacteria target)
        {
            TimeUntilNextRelease = 5;
            List<Antibody> antibodies = new List<Antibody>();

            for (int i = 0; i < 5;i++)
            {
                double theta = i * (360.0 / 5);
                double x = Position.X + Size * Math.Cos(theta * Math.PI / 180)/2;
                double y = Position.Y + Size * Math.Sin(theta * Math.PI / 180)/2;
                Vector2 position = new Vector2((float)x, (float)y);

                antibodies.Add(new Antibody(parent, position, Angle, target));
            }

            return antibodies;
        }
    }
}
