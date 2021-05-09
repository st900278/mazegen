using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Position2D : Position {

    /**
     * Compute the Manhattan distance between this position and [pos].
     */
    public int x, y;

    public Position2D(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int DistanceTo(Position pos)
    {
        return Math.Abs((pos as Position2D).x - x) + Math.Abs((pos as Position2D).y - y);
    }


    public Position Plus(Position pos)
    {
        return new Position2D(x + (pos as Position2D).x, y + (pos as Position2D).y);
    }
            

    public int CompareTo(Position pos)
    {
        if (x == (pos as Position2D).x && y == (pos as Position2D).y)
            return 0;
        else if (x > (pos as Position2D).x || x == (pos as Position2D).x && y > (pos as Position2D).y)
            return 1;
        else
            return -1;
    }
            
    /*
    override fun equals(other: Any?): Boolean {
        if (other === this) return true
        if (other !is Position2D) return false
        return x == other.x && y == other.y
    }

override fun hashCode(): Int {
        // Unique hashcodes up to 65536
        return (x shl 16) and y
    }

    override fun toString() = "[x: $x, y: $y]"
    */

}
