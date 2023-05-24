using CellLibrary.Simulator;
using System.Collections.Concurrent;
using OrganismServer.Monogame;

namespace OrganismServer.Services {
    public class OrganismLogic {
        internal ConcurrentDictionary<Cell, byte?> cells = new();
        internal OrganismGraphicRepresentation organismDrawer;
        internal CancellationTokenSource shutdown = new();
        internal readonly int maxCells = 150;
        internal CellFactory cellFactory;

        public OrganismLogic() {
            organismDrawer = new(this);
            cellFactory = new CellFactory();
            new Thread(organismDrawer.Run).Start();
        }
    }
}
