using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellSimulator.Simulator {
    internal class Antibody : Cell {
        public override string Name => "Przeciwcialo";
        public override string SVGSprite => "<svg viewBox=\"0 0 100 100\" xmlns=\"http://www.w3.org/2000/svg\">\r\n  <defs></defs>\r\n  <polygon points=\"40,60 60,60 50,40\" style=\"fill:green;stroke:black;stroke-width:1\" /></svg>";
        public override int Size { get; set; }
        public override float Speed { get; set; }
        public override float DivideRate { get; set; } = float.PositiveInfinity;

        public bool isAttached { get; set; } = false;
        public Bacteria Target { get; set; } = null;
        private Vector2 attachedPosition;

        public Antibody(Organism parent, Vector2 p, float angle, Bacteria target) : base(parent, p, angle) {
            Speed = Random.Shared.NextSingle() * (75.0f - 60f) + 60f;
            Size = Random.Shared.Next(54, 72);
            Target = target;
        }

        protected override void Life(float delta) {
            if (isAttached) {//moves with bacteria
                Angle = Target.Angle;
                Position = Target.Position - attachedPosition;
            }
            else //pursuing target
            {
                Bacteria target = Target;

                var difference = target.Position - Position;
                Angle = (float)Math.Atan2(difference.X, -difference.Y);

                double distance = (float)Math.Sqrt(Math.Pow(target.Position.X - Position.X, 2.00) + Math.Pow(target.Position.Y - Position.Y, 2.00));
                if (distance < target.Size / 5) //check if target found
                {
                    isAttached = true;
                    target.isAttacked = true;
                    Speed = target.Speed;
                    attachedPosition = difference;
                }
            }

            if (!isAttached) {
                if (Position.X > parent.organismDrawer.viewPort.Width || Position.X < 0)
                    Angle = -Angle;
                if (Position.Y > parent.organismDrawer.viewPort.Height || Position.Y < 0)
                    Angle = (float)Math.PI - Angle;
                Position += new Vector2((float)Math.Sin(Angle) * Speed * delta, (float)-Math.Cos(Angle) * Speed * delta);
            }
        }
    }
}
