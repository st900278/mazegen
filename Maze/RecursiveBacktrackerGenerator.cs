using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RecursiveBacktrackerGenerator: Generator
{

    public override void Generate(Maze maze) 
    {
        
        base.Generate(maze);
        
        maze.FillAll();

        // Get cell on a random starting location
        var currentCell = maze.GetRandomCell();
        currentCell.visited = true;

        var stack = new Stack<Cell>();
        while (true)
        {
            // Find an unvisited neighbor cell
            var unvisitedNeighbors = currentCell.neighbors.Value.OrderBy(i => Guid.NewGuid()).ToList<Cell>().Where(data => !data.visited).ToList();
            var unvisitedNeighbor = unvisitedNeighbors.Count == 0 ? null : unvisitedNeighbors.FirstOrDefault();
            if (unvisitedNeighbor != null)
            {
                // Connect with current cell
                currentCell.ConnectWith(unvisitedNeighbor);

                // Add current cell to the stack
                stack.Push(currentCell);

                // Make the connected cell the current cell
                currentCell = unvisitedNeighbor;
                currentCell.visited = true;

            }
            else
            {
                // No unvisited neighbor cell
                // Go back to last cell in stack to check for unvisited neighbors
                if (stack.Count != 0)
                {
                    currentCell = stack.Pop();
                }
                else
                {
                    break;
                }
            }
        }
    }

    public override bool IsMazeSupported(Maze maze)
    {
        return true;
    }

}