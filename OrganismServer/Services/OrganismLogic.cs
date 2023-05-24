using CellLibrary.Simulator;
using System.Collections.Concurrent;
using OrganismServer.Monogame;
using System.Numerics;

namespace OrganismServer.Services {
    public class OrganismLogic {
        internal ConcurrentDictionary<Cell, byte?> cells = new();
        internal OrganismGraphicRepresentation organismDrawer;
        internal CancellationTokenSource shutdown = new();
        internal readonly int maxCells = 50;

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
            foreach(var cell in cells.Keys) {
                if (cell.Position.X > organismDrawer.viewPort.Width || cell.Position.X < 0)
                    cell.Angle = -cell.Angle;
                if (cell.Position.Y > organismDrawer.viewPort.Height || cell.Position.Y < 0)
                    cell.Angle = (float)Math.PI - cell.Angle;
                cell.Position += new Vector2((float)Math.Sin(cell.Angle) * cell.Speed * delta, (float)-Math.Cos(cell.Angle) * cell.Speed * delta);
            }
        }
    }
}
