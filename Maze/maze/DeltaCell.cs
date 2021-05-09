using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

/**
 * A triangular cell in [DeltaMaze].
 * Has west, east and base (north or south) sides.
 */
public class DeltaCell: Cell
{
    public DeltaCell(DeltaMaze maze, Position2D position) : base(maze, position)
    {
        
    }
    public override Cell GetCellOnSide(Cell.Side side)
    {
        
        if (side == Side.BASE)
        {
            // The cell on the base side can be either up or down, depending on the X position.
            var flatTopped = ((position as Position2D).x + (position as Position2D).y) % 2 == 0;
            return maze.CellAt((position as Position2D).Plus(new Position2D(0, (flatTopped) ? -1 : 1)));
        }
        return base.GetCellOnSide(side);
     }

    public override List<Cell.Side> allSides { get; set; } = Side.all;

    public override int allSidesValue { get; set; } = Side.allValue;

    /**
     * Enum class for the sides of a delta cell.
     */
    public new class Side : Cell.Side
    {
        public int value { get; set; }

        public Position relativePos { get; set; }


        public string symbol { get; set; }

        public Cell.Side opposite { get; set; }

        public Side(int value, Position2D realtivePos, string symbol)
        {
            this.value = value;
            this.relativePos = relativePos;
            this.symbol = symbol;
            if(this == BASE)
            {
                opposite = BASE;
            }
            else if(this == EAST)
            {
                opposite = WEST;
            }
            else if (this == WEST)
            {
                opposite = EAST;
            }
        }

        public static Side BASE = new Side(1, null, "B");
        public static Side EAST = new Side(2, new Position2D(1,0), "E");
        public static Side WEST = new Side(4, new Position2D(-1,0), "W");
        
        public static int allValue = 7;
        public static List<Cell.Side> all = new List<Cell.Side>() { BASE, EAST, WEST };

    }



}