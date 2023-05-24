using CellLibrary.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace OrganismServer.Services
{
    public enum CellType
    {
        BACTERIA = 1,
        LEUKOCYTE = 2,
        ANTIBODY = 3,
        MACROPHAGE = 4
    }

    internal class CellFactory
    {

        /*public Cell createCellFromCellInfo(OrganismLogic parent, CellInfo cellInfo)
        {
            CellType type = (CellType)cellInfo.Type;
            float x = cellInfo.X;
            float y = cellInfo.Y;
            Vector2 position = new Vector2(x, y);
            float angle = cellInfo.Angle;
            Cell cell = createCellWithType(type, parent, position, angle);
            if(cell == null)
            {
                return null;
            }
            cell.Speed = cellInfo.Speed;
            cell.DivideRate = cellInfo.DivideRate;
            var id = cellInfo.Id.Value;

            cell.Id = new Guid(id.Memory.ToArray());
            cell.Size = (int) cellInfo.Size;
            return cell;
        }

        private Cell createCellWithType(CellType cellType, 
            OrganismLogic parent, Vector2 position, float angle)
        {
            switch (cellType)
            {
                case CellType.BACTERIA:
                    return new Bacteria(parent, position, angle);
                case CellType.LEUKOCYTE:
                    return new Leukocyte(parent, position, angle);
                case CellType.ANTIBODY:
                    return new Antibody(parent, position, angle, null);
                case CellType.MACROPHAGE:
                    return new Macrophage(parent, position,angle);
            }
            return null;
        }*/
    }
}
