using CellLibrary.Simulator;
using System.Collections.Concurrent;
using OrganismServer.Monogame;
using System.Numerics;

namespace OrganismServer.Services {
    public class OrganismLogic {
        internal ConcurrentDictionary<Guid, Cell> cells = new();
        internal OrganismGraphicRepresentation organismDrawer;
        internal CancellationTokenSource shutdown = new();
        internal readonly int maxCells = 50;
        internal Microsoft.Xna.Framework.Color background = new Microsoft.Xna.Framework.Color(255, 0, 100);
        static readonly int cellFrameTimeMillis = 1000 / 60;

        public OrganismLogic() {
            organismDrawer = new(this);
            new Thread(organismDrawer.Run).Start();
            new Thread(() => {
                while(!shutdown.IsCancellationRequested) {
                    Life(cellFrameTimeMillis / 1000f);
                    Thread.Sleep(cellFrameTimeMillis);
                }
            }).Start();
        }

        //common behaviour for all cells in the organism eg. movement
        private void Life(float delta) {
            int bacteria = 0;
            foreach(var cell in cells.Values) {
                if(cell.Dead) {
                    if ((cell.Size /= 1.02f) == 0)
                        cells.TryRemove(cell.Id, out var _);
                    continue;
                }

                if (cell.Target is not null) {
                    cells.TryGetValue(cell.Target.Value, out var target);
                    if (target is not null) {
                        var diff = target.Position - cell.Position;
                        diff /= diff.Length();
                        cell.Speed = diff * Math.Max(target.Speed.Length() * 1.3f, cell.Speed.Length());
                    }
                }
                else {
                    if (cell.Position.X > organismDrawer.viewPort.Width || cell.Position.X < 0)
                        cell.Speed = new(-cell.Speed.X, cell.Speed.Y);
                    if (cell.Position.Y > organismDrawer.viewPort.Height || cell.Position.Y < 0)
                        cell.Speed = new(cell.Speed.X, -cell.Speed.Y);
                }

                cell.Position += cell.Speed * delta;
                if (cell.Name == "Bakteria")
                {
                    bacteria++;
                }
            }

            background.G = (byte)(255 * 0.1 * bacteria);
            background.R = (byte)(255 - 150 * 0.1 * bacteria);

            if (background.G >= 140)
            {
                background.G = 150;
            }
            if (background.R <= 110)
            {
                background.R = 100;
            }
        }
    }
}
