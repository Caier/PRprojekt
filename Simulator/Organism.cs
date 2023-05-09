using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CellSimulator.Monogame;
using ExCSS;
using Microsoft.Xna.Framework;

namespace CellSimulator.Simulator {
    public class Organism {
        internal ConcurrentDictionary<Cell, byte?> cells = new();
        OrganismGraphicRepresentation organismDrawer;

        public Organism() {
            organismDrawer = new(this);
            cells.TryAdd(new Bacteria(new(400, 100), 3.234f), null);
            cells.TryAdd(new Leukocyte(new(150, 200), 2.221f), null);
            cells.TryAdd(new Macrophage(new(500, 234), 2.34f), null);
            new Thread(organismDrawer.Run).Start();
            OrganismLife();
        }

        //wołane z OrganismGraphicRepresentation, przeznaczone do aktualizowania stanu niezwiązanego z wyglądem - pozycja itp.
        internal void Update(GameTime time) {
            float delta = (float)time.ElapsedGameTime.TotalSeconds;

            foreach(var cell in cells.Keys) {

                if(cell.Name == "Leukocyt")
                {

                    Leukocyte leukocyte = (Leukocyte)cell;

                    if (leukocyte.TimeUntilNextRelease-- <= 0)
                    {
                                       
                        var closestDistance = double.PositiveInfinity;
                        Bacteria targetBacteria = null;
                        foreach (var c in cells.Keys)
                        {                        
                            if(c.Name == "Bakteria")
                            {                           
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
                            foreach (Antibody a in antibodies)
                            {
                                cells.TryAdd(a, null);
                            }
                        }

                    }
                }

                if(cell.Name == "Przeciwcialo")
                {
                    Antibody antibody = (Antibody)cell;
                    if(antibody.isAttached) //moves with bacteria
                    {
                        antibody.Angle = antibody.Target.Angle;
                    }
                    else //pursuing target
                    {
                        Bacteria target = antibody.Target;

                        var difference = target.Position - antibody.Position;
                        antibody.Angle = (float)Math.Atan2(difference.X, -difference.Y);

                        double distance = (float)Math.Sqrt(Math.Pow(target.Position.X - cell.Position.X, 2.00) + Math.Pow(target.Position.Y - cell.Position.Y, 2.00));
                        if (distance < target.Size/5) //check if target found
                        {
                            antibody.isAttached = true;
                            antibody.Speed = target.Speed;
                        }
                    }
                }

                if (cell.Position.X > organismDrawer.viewPort.Width || cell.Position.X < 0)
                    cell.Angle = -cell.Angle;
                if (cell.Position.Y > organismDrawer.viewPort.Height || cell.Position.Y < 0)
                    cell.Angle = (float)Math.PI - cell.Angle;
                cell.Position += new Vector2((float)Math.Sin(cell.Angle) * cell.Speed * delta, (float)-Math.Cos(cell.Angle) * cell.Speed * delta);
            }
        }



        private async void OrganismLife() {
            while(true) {
                var r = Random.Shared.NextDouble();

                var types = cells.Keys.Select(c => c.Name).ToHashSet();
                var type = types.ElementAt(Random.Shared.Next(0, types.Count));

                if ((r < 0.3) && (type!= "Przeciwcialo")) {
                    cells.TryAdd(cells.Keys.Where(c => c.Name == type).First().Divide(), null);
                }

                await Task.Delay(500);
            }
        }
    }
}
