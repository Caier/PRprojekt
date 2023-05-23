using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellLibrary.Simulator
{
    public enum CellAttribute { 
        SPEED, SIZE, ANGLE
    }

    public interface IOrganism
    {
        void createCell(Cell cell);
        void moveCell(Cell cell, float x, float y);
        void changeCellAttribute(Cell cell, CellAttribute cellAttribute, float value);
        void killCell(Cell cell);
    }
}
