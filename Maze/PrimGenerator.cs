using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

public class PrimGenerator : Generator
{

    public override void Generate(Maze maze)
    {

        base.Generate(maze);

        maze.FillAll();

        var initialCell = maze.GetRandomCell();
        initialCell.visited = true;
        var set = new HashSet<Cell>() { initialCell };
        Debug.WriteLine(set.Count);
        do
        {
            var currentCell = set.ToList().OrderBy(i => Guid.NewGuid()).FirstOrDefault();
            Debug.WriteLine(currentCell.neighbors.Value.Count);
            var connected = false;
            foreach (var neighbor in currentCell.neighbors.Value.OrderBy(i => Guid.NewGuid()))
            {
                if (!connected && neighbor.visited)
                {
                    currentCell.ConnectWith(neighbor);
                    connected = true;
                }
                else if (!neighbor.visited)
                {
                    set.Add(neighbor);
                }
            }
            currentCell.visited = true;
            set.Remove(currentCell);
        } while (set.Count != 0);
    }

    public override bool IsMazeSupported(Maze maze)
    {
        return true;
    }

}