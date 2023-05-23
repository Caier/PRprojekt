using CellLibrary.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellClient
{
    internal class CellSerializator
    {
        public CellInfo serializeFullCell(Cell cell)
        {
            CellInfo cellInfo = new CellInfo();
            cellInfo.Id = new UUID {
                Value = cell.Id.ToString()
            };
            cellInfo.X = cell.Position.X;
            cellInfo.Y = cell.Position.Y;
            cellInfo.Angle = cell.Angle;
            cellInfo.Size = cell.Size;
            cellInfo.DivideRate = cell.DivideRate;
            cellInfo.DivisionCounter = 0;
            cellInfo.Speed = cell.Speed;
            if(cell is Leukocyte)
            {
                cellInfo.Type = 2;   
            } 
            else if(cell is Macrophage)
            {
                cellInfo.Type = 4;
            } 
            else if(cell is Bacteria)
            {
                cellInfo.Type = 1;
            } 
            else if(cell is Antibody)
            {
                cellInfo.Type = 3;
            }
            return cellInfo;
        }

    }
}
