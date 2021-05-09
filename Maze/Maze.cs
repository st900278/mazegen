﻿using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public abstract class Maze
{
    private List<Cell> openings = new List<Cell>();

    /**
     * The maze solution, a list of the path's cells starting from the first
     * opening and ending on the second. Null if no solution was found yet.
     */
    private List<Cell> solution;


    /**
     * Whether a distance map has been generated or not.
     */
    private bool hasDistanceMap = false;

    /**
     * Get the total number of cells in this maze.
     */
    public abstract int CellCount();

    /**
     * Returns the cell at [pos] if it exists, otherwise returns null.
     */
    public abstract Cell CellAt(Position pos);

    /**
     * Returns a random cell in the maze.
     */
    public abstract Cell GetRandomCell();

    /**
     * Creates and returns a list containing all the cells in this maze.
     */
    public abstract List<Cell> GetAllCells();

    /**
     * Call [action] on every cell.
     */
    //public abstract forEachCell(action: (Cell) -> Unit);
    public abstract void ForEachCell(Action<Cell> action);

    public Maze()
    {
        openings = new List<Cell>();

    }
    /**
     * Clears all sides of all cells in the maze, resets all visited and distance map flags.
     */
    public void ResetAll()
    {
        ForEachCell((Cell cell) =>
        {
            cell.value = 0;
            cell.visited = false;
            cell.distanceMapValue = -1;
        });
        solution = null;
        openings.Clear();
    }

    /**
     * Sets all sides of all cells in the maze, resets all visited and distance map flags.
     */
    public void FillAll()
    {
        ForEachCell((Cell cell) =>
        {
            cell.value = cell.allSidesValue;
            cell.visited = false;
            cell.distanceMapValue = -1;
        });
        solution = null;
        openings.Clear();
    }

    /**
     * Create an [opening] in the maze. An exception is thrown if the opening position
     * doesn't match any cell or if the opening already exists.
     */
    public void CreateOpening(Position opening)
    {
        Cell cell = GetOpeningCell(opening);
        if (cell != null)
        {
            if (openings.IndexOf(cell) < 0)
            {
                //Debug.Log("error");
                //paramError("Duplicate opening for position ${cell.position}.")
            }

            foreach (var side in cell.allSides)
            {
                if (cell.GetCellOnSide(side) == null)
                {
                    cell.OpenSide(side);
                    break;
                }
            }

            openings.Add(cell);
        }
        else
        {
            //Debug.Log("Opening describes no cell in the maze.");
        }
    }

    /**
     * Get the cell described by the [opening] position.
     */
    public abstract Cell GetOpeningCell(Position opening);

    /**
     * Find the solution of the maze between two cells.
     * The solution is a list of cells in the path, starting
     * from the start cell, or null if there's no solution.
     *
     * This uses the A* star algorithm as described
     * [here](https://en.wikipedia.org/wiki/A*_search_algorithm#Description).
     *
     * 1. Make the starting cell the root node and add it to a list.
     * 2. Find the node in the list with the lowest "cost". Cost is the sum of the
     *    cost from the start and the lowest possible cost to the end. The cost from
     *    the start is the number of cells travelled to get to the node's cell. The
     *    lowest cost to the end is the minimum number of cells that have to be
     *    travelled to get to the end cell. Remove this node from the list.
     * 3. Add nodes of all unvisited neighbor of this node's cell
     *    to the list and mark them as visited.
     * 4. Repeat step 2 and 3 until the list is empty, hence there is no solution, or
     *    when the cell of the node selected at step 2 is the end cell.
     *
     * Runtime complexity is O(n) and memory space is O(n).
     *
     * @param start Start cell for solving.
     * @param end End cell for solving.
     */
    /*
   fun solve(start: Cell = openings[0], end: Cell = openings[1]) : Boolean {
       forEachCell { it.visited = false }

val nodes = PriorityQueue<Node>()

       // Add the start cell as the initial node
nodes.add(Node(null, start, 0,
               start.position.distanceTo(end.position)))
       start.visited = true

       while (nodes.isNotEmpty()) {
           // Remove the node with the lowest cost from the queue.
           val node = nodes.remove()
           val cell = node.cell

           if (cell === end) {
               // Found path to the end cell
               val path = mutableListOf<Cell>()
               var currentNode: Node? = node
               while (currentNode != null) {
                   path.add(0, currentNode.cell)
                   currentNode = currentNode.parent
               }
               solution = path
               return true
           }

           for (neighbor in cell.findAccessibleNeighbors()) {
               if (!neighbor.visited) {
                   // Add all unvisited neighbors to the nodes list
                   nodes.add(Node(node, neighbor, node.costFromStart + 1,
                           neighbor.position.distanceTo(end.position)))
                   neighbor.visited = true
               }
           }
       }

       // All cells were visited, no path was found.
       solution = null
       return false
   }
   */
    /**
     * If maze was previously solved, clear the solution.
     */
    public void ClearSolution()
    {
        solution = null;
    }


    private class Node : IComparable
    {
        Node parent;
        Cell cell;
        int costFromStart;
        int costToEnd;
        public Node(Node parent, Cell cell, int costFromStart, int costToEnd)
        {
            this.parent = parent;
            this.cell = cell;
            this.costFromStart = costFromStart;
            this.costToEnd = costToEnd;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Node other = obj as Node;
            return (costFromStart + costToEnd).CompareTo(other.costFromStart + other.costToEnd);
        }
        /*
        override fun equals(other: Any?): Boolean {
            if (other === this) return true
            if (other !is Node) return false
            return cell === other.cell
        }
        */
        //override fun hashCode(): Int = cell.hashCode()

    }

    /**
     * Open a number of deadends set by the braiding [setting].
     * Distance map is cleared and will need to be regenerated after braiding.
     */
    public void Braid(Braiding setting)
    {
        //clearDistanceMap()

        var deadends = new List<Cell>();
        ForEachCell((Cell cell) =>
        {
            if (cell.sidesCount == cell.allSides.Count - 1)
            {
                // A cell is a deadend if it has only one side opened.
                deadends.Add(cell);
            }
        });

        var count = setting.GetNumberOfDeadendsToRemove(deadends.Count);

        var removed = 0;
        System.Random r = new System.Random();
        while (deadends.Count != 0 && removed < count)
        {
            var index = r.Next(deadends.Count);
            var deadend = deadends[index];
            deadends.RemoveAt(index);

            foreach (var side in deadend.allSides)
            {
                if (!deadend.HasSide(side))
                {
                    var deadside = side.opposite.Value;
                    var other = deadend.GetCellOnSide(deadside);
                    if (other != null)
                    {
                        // If the deadside is not a border, open it.
                        deadend.ConnectWith(other);
                    }
                    break;
                }
            }
            removed++;
        }
    }

    /**
     * Braiding setting for a maze.
     */
    public class Braiding
    {

        private double value;
        private bool byCount;

        /**
         * Braiding setting to remove [count] deadends.
         */
        public Braiding(int count)
        {
            if (count < 0)
            {
                //Debug.Log("Braiding parameter must be a positive number.");
            }

            value = count;
            byCount = true;
        }

        /**
         * Braiding setting to remove a percentage, [percent], of the total number of deadends.
         */
        public Braiding(double percent)
        {
            if (percent < 0 || percent > 1)
            {
                //Debug.Log("Braiding percentage must be between 0 and 1 inclusive.");
            }

            value = percent;
            byCount = false;
        }

        /**
         * Get the number of deadends to remove with this braiding
         * setting out of the [total] number of deadends.
         */
        public int GetNumberOfDeadendsToRemove(int total)
        {
            if (byCount)
            {
                return Math.Min((int)value, total);
            }
            else
            {
                return (int)Math.Round(total * value);
            }
        }
    }
    public abstract string ToJson(Configuration.JsonOutput jsonOutput);

    public const int OPENING_POS_START = Int32.MinValue;
    public const int OPENING_POS_CENTER = Int32.MinValue + 1;
    public const int OPENING_POS_END = Int32.MinValue + 2;
    
}


        /*
            override fun toString() = "Remove " + if (byCount) {
                "$value deadends"
            } else {
                "${value.toDouble() * 100}% of deadends"
            }
        }
        */

    /**
     * Generate the distance map on this maze, using Dijkstra's algorithm to find
     * the shortest distance to every cell in the map, starting from [startPos].
     * See [this page](https://en.wikipedia.org/wiki/Dijkstra's_algorithm) for the algorithm.
     *
     * 1. Assign an arbitrarly high distance to all cells but the start cell, which is assigned a distance of 0.
     * 2. Create a list of unvisited cells containing all cells.
     * 3. Remove the cell with the lowest distance in the list. For each of its accessible
     *      neighbors, take the smallest distance between their current one and the
     *      one calculated from the removed cell.
     * 4. Repeat step 3 until the list is empty.
     *
     * Runtime complexity is O(n²) and memory space is O(n).
     * Implementation would be probably faster using a Fibonnaci heap.
     *
     * @param startPos Starting position (distance of zero), can be `null` for a random cell.
     */
     /*
    public void GenerateDistanceMap(Position startPos)
    {
        int inf = Int.MAX_VALUE - 1;
            forEachCell {
            it.visited = false
                it.distanceMapValue = inf
            }

    val startCell = if (startPos == null)
    {
        getRandomCell()
        }
    else
    {
        getOpeningCell(startPos) ?: paramError(
                "Distance map start position describes no cell in the maze.")
        }
    startCell.distanceMapValue = 0

        val cellList = getAllCells()
        while (cellList.isNotEmpty())
    {
        // Get and remove the cell with the lowest distance
        var minIndex = 0
            for (i in 1 until cellList.size)
        {
            val cell = cellList[i]
                if (cell.distanceMapValue < cellList[minIndex].distanceMapValue)
            {
                minIndex = i
                }
        }

        val minCell = cellList.removeAt(minIndex)
            minCell.visited = true

            if (minCell.distanceMapValue == inf)
        {
            // This means the only cells left are inaccessible from the start cell.
            // The distance map can't be generated completely.
            error("Could not generate distance map, maze has inaccessible cells.")
            }

        // Compare unvisited accessible neighbors distance calculated from this cell with
        // their current distance. If new distance is smaller, update it.
        for (neighbor in minCell.findAccessibleNeighbors())
        {
            if (!neighbor.visited)
            {
                val newDistance = minCell.distanceMapValue + 1
                    if (newDistance < neighbor.distanceMapValue)
                {
                    neighbor.distanceMapValue = newDistance
                    }
            }
        }
    }

    hasDistanceMap = true
    }
    */

/**
 * Clear the distance map values on all cells.
 */
     /*
    open fun clearDistanceMap()
    {
        if (hasDistanceMap)
        {
            forEachCell {
                it.distanceMapValue = -1
                }
            hasDistanceMap = false
            }
    }
    */
    /**
     * Draw the maze to a [canvas] with [style] settings.
     */
     /*
    abstract fun drawTo(canvas: Canvas, style: Configuration.Style)


        companion object {
            const val OPENING_POS_START = Int.MIN_VALUE
            const val OPENING_POS_CENTER = Int.MIN_VALUE + 1
            const val OPENING_POS_END = Int.MIN_VALUE + 2
        }

    }
    */
