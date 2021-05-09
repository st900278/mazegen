using System.Collections;
using System;
using System.Collections.Generic;
//using UnityEngine;

public abstract class Cell
{
    /**
     * Cell can be marked as visited by a generator.
     */
    public bool visited = false;

    /**
     * The cell value encoding which sides are set. Bit field of [Side] values.
     */
    public int value = 0;

    /**
     * If a distance map was generated, the minimum distance of this cell from the starting cell.
     */
    public int distanceMapValue = -1;

    /**
     * Returns a list of all possible side values.
     */

    /**
    * Returns the value of a cell with all sides set.
    */
    
    public Maze maze;
    public Position position;
    public Cell(Maze maze, Position position)
    {
        this.maze = maze;
        this.position = position;

        
        var list = new List<Cell>();
        foreach (var side in allSides)
        {
            var cell = GetCellOnSide(side);
            if (cell != null)
            {
                list.Add(cell);
            }
        }
        neighbors = list;

    }

    /**
     * The list of cells adjacent to this cell, but not necessarily connected.
     * All neighbors must be able to validly connect with this cell.
    */
    //by lazy
    public List<Cell> neighbors;

    

    /**
     * Returns a list of neighbor cells than are accessible from this cell,
     * meaning the wall they share with this cell is not set.
     */
    public List<Cell> FindAccessibleNeighbors()
    {
        List<Cell> list = new List<Cell>();
        
        foreach(var side in allSides) {
            if (!HasSide(side)) {
                Cell neighbor = GetCellOnSide(side);
                if(neighbor != null) {
                    list.Add(neighbor);
                }
            }
        }
        return list;
    }

    /**
     * Returns the neighbor cell on the [side] of the cell.
     * If the neighbor doesn't exist, returns null.
     */
    public virtual Cell GetCellOnSide(Side side){
        if (side.relativePos == null) return null;
        return maze.CellAt(position.Plus(side.relativePos));
    }

    /**
     * Returns true if [side] is set.
     */
     
    public bool HasSide(Side side)
    {
        if (side.value == 0) return value == 0;
        return (value & side.value) == side.value;
    }

    /**
     * Opens [side] of the cell. (Removes a wall)
     */
    public void OpenSide(Side side)
    {
        ChangeSide(side, (v, s) => {
            return v & ~s;
        });
    }

    /**
     * Closes [side] of the cell. (Adds a wall)
     */
    public void CloseSide(Side side)
    {
        ChangeSide(side, (v, s) => {
            return v | s;
        });
    }

    /**
     * Do [operation] on this cell's [side] wall and the cell on the other side.
     */
    
    private void ChangeSide(Side side, Func<int, int, int> operation) {
        var cell = GetCellOnSide(side);
        if (cell != null) {
            cell.value = operation(cell.value, side.opposite.value);
        }
        value = operation(value, side.value);
    }
    
    
    /**
     * Connect this cell with another cell [cell] if they are neighbors of the same maze.
     * Does nothing otherwise. The common wall of both cells is opened (removed).
     */
    public void ConnectWith(Cell cell)
    {
        Side side = FindSideOfCell(cell);
        if (side != null)
        {
            cell.value = cell.value & ~side.opposite.value;
            value = value & ~side.value;
        }
    }

    /**
     * Return the side of this cell on which [cell] is placed, if they are
     * neighbors in the same maze. Returns null otherwise.
     */
    public Side FindSideOfCell(Cell cell){
        if (cell.maze == maze)
        {
            foreach(var side in allSides)
            {
                if (GetCellOnSide(side) == cell)
                {
                    return side;
                }
            }
        }
        return null;
    }

    public int sidesCount { get { return BitCount(value); } }
    public abstract List<Side> allSides { get; set; }
    public abstract int allSidesValue { get; set; }
    /**
     * Returns the number of sides set on this cell.
     */

    private static int BitCount(int n)
    {
        var count = 0;
        while (n != 0)
        {
            count++;
            n &= (n - 1); //walking through all the bits which are set to one
        }

        return count;
    }


    //get() = Integer.bitCount(value)

    /**
     * Returns a list of all possible side values.
     */
    //abstract val allSides: List<Side>

    /**
     * Returns the value of a cell with all sides set.
     */


    /*
    public override string toString(){
        val sb = StringBuilder()
        sb.append("[pos: $position, sides: ")
        if (value == 0) {
            sb.append("NONE")
        } else {
            for (side in allSides) {
                if (hasSide(side)) {
                    sb.append(side.symbol)
                    sb.append(",")
                }
            }
            sb.deleteCharAt(sb.length - 1)
        }
        sb.append(", ")
        sb.append(if (visited) "visited" else "unvisited")
        sb.append("]")
        return sb.toString()
    }
    */

    /*
    override fun hashCode(): Int = position.hashCode()

    override fun equals(other: Any?): Boolean {
        // A cell cannot be equal to another, there is exactly
        // one cell for each position of every maze.
        return other === this
    }
    */


    public interface Side
    {
        /**
         * Side value, a single bit different for each side.
         */
        int value { get; set; }

        /**
         * Relative position of a cell on this side.
         * Can be null if not always the same or not applicable.
         */
        Position relativePos { get; set; }

        /**
         * The symbol for this side, used by [Cell.toString].
         */
        string symbol { get; set; }

        /*
        public Side(int value, Position relativePos, string symbol)
        {
            this.value = value;
            this.relativePos = relativePos;
            this.symbol = symbol;
        }
        */

        /**
         * The side opposite to this side.
         */
        Side opposite { get; set; }
    }

}

