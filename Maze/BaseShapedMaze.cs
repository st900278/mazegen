using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using UnityEngine;
public abstract class BaseShapedMaze<T> : Maze where T : Cell
{
    int width { get; set; }
    int height { get; set; }
    Shape shape { get; set; }
    public BaseShapedMaze(int width, int height, Shape shape)
    {
        this.width = width;
        this.height = height;
        this.shape = shape;

        if (width < 1 || height < 1)
        {
            //Debug.Log("Dimensions must be at least 1.");
        }
        if (shape == Shape.TRIANGLE
                || shape == Shape.HEXAGON)
        {
            // Hexagon and triangle mazes have only one size parameter.
            this.height = width;
        }
        else
        {
            this.height = height;
        }

    }


    /**
     * The maze grid. There number of columns is the same as the maze width, except for
     * hexagon-shaped mazes where the number of columns is equal to `width * 2 - 1`.
     * The number of rows varies for each column depending on the shape of the maze.
     */
    protected virtual List<List<T>> grid { get; set; }

    /**
     * The offset of the actual Y coordinate of a cell in the grid array, for each column
     * The cell at ```grid[x][y]```'s actual coordinates are `(x ; y + rowOffsets[x])`.
     */
    protected List<int> rowOffsets;

    override public int CellCount()
    {
        var count = 0;
        for(var i = 0;i < grid.Count; i++)
        {
            count += grid[i].Count;
        }
        return count;
    }
    public override Cell CellAt(Position pos)
    {
        return CellAt((pos as Position2D).x, (pos as Position2D).y);
    }

    public T CellAt(int x, int y)
    {
        if (x < 0 || x >= grid.Count) return null;
        var actualY = y - rowOffsets[x];
        if (actualY < 0 || actualY >= grid[x].Count) return null;
        return grid[x][actualY];
    }


    override public Cell GetRandomCell()
    {
        System.Random r = new System.Random();
        var x = r.Next(grid.Count);
        return grid[x][r.Next(grid[x].Count)];
    }
    override public List<Cell> GetAllCells()
    {
        List<Cell> list = new List<Cell>();
        for (var i = 0; i < grid.Count;i++)
        {
            for (var j=0; j<grid[i].Count;j++)
            {
                list.Add(grid[i][j]);
            }
        }
        return list;
    }

    override public void ForEachCell(Action<Cell> action)
    {
        for (var i = 0; i < grid.Count; i++)
        {
            for (var j = 0; j < grid[i].Count; j++)
            {
                action(grid[i][j]);
            }
        }
    }
    override public Cell GetOpeningCell(Position opening)
    {
        int x = 0, y = 0;
        switch ((opening as Position2D).x)
        {
            case Maze.OPENING_POS_START:
                x = 0;
                break;
            case Maze.OPENING_POS_CENTER:
                x = grid.Count / 2;
                break;
            case Maze.OPENING_POS_END:
                x = grid.Count - 1;
                break;
        }
        switch ((opening as Position2D).y)
        {
            case Maze.OPENING_POS_START:
                y = 0 + rowOffsets[x];
                break;
            case Maze.OPENING_POS_CENTER:
                y = grid[x].Count / 2 + rowOffsets[x];
                break;
            case Maze.OPENING_POS_END:
                y = grid[x].Count - 1 + rowOffsets[x];
                break;
        }
        return CellAt(x, y);
    }

}

    /**
     * Possible shapes for a shaped maze.
     */
public enum Shape
{
    RECTANGLE,
    TRIANGLE,
    HEXAGON,
    RHOMBUS
}

