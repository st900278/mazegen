using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class Generator
{
    public virtual void Generate(Maze maze)
    {
        if (IsMazeSupported(maze))
        {
            //Debug.Log("error");
        }
    }

    public abstract bool IsMazeSupported(Maze maze);
}
