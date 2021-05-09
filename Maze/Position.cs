using System.Collections;
using System.Collections.Generic;


public interface Position
{

    /**
     * Return the distance from this position to [pos].
     * Used by the maze solver as an arbritrary cost heuristic.
     */
    int DistanceTo(Position pos);

    /**
     * Return a new position corresponding to the sum of this position and [pos].
     */
    Position Plus(Position pos);

    /**
     * Compare this position with another position, [pos].
     */
    int CompareTo(Position pos);

}