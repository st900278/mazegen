using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DeltaMaze : BaseShapedMaze<DeltaCell>
{


    List<List<DeltaCell>> grid;
    List<int> val;
    /**
     * The offset of the actual Y coordinate of a cell in the grid array, for each column
     * The cell at ```grid[x][y]```'s actual coordinates are `(x ; y + rowOffsets[x])`.
     */

    public DeltaMaze(int width, int height, Shape shape) : base(width, height, shape)
    {
        var gridWith = width * 2 - 1;
        Func<int, int> rowsForColumn = (column) => 0;
        Func<int, int> rowOffset = (column) => 0;
        //val rowOffset: (column: Int)->Int;
        switch (shape)
        {
            case Shape.RECTANGLE:
                rowsForColumn = (column) => height;
                rowOffset = (column) => 0;
                break;
            case Shape.HEXAGON:
                gridWith = 4 * width - 1;
                rowsForColumn = (column) =>
                {
                    if (column < width)
                        return 2 * (column + 1);
                    else if (column >= gridWith - width)
                        return 2 * (gridWith - column);
                    else return width;
                };

                rowOffset = (column) =>
                {
                    if (column < width)
                        return width - column - 1 + width % 2;
                    else if (column >= gridWith - width)
                        return width + column - gridWith + width % 2;
                    else return width % 2;
                };
                break;
            case Shape.TRIANGLE:
                rowOffset = (column) => 0;
                rowsForColumn = (column) => width - Math.Abs(column - gridWith / 2);
                break;
            case Shape.RHOMBUS:

                gridWith = 2 * width + height - 1;
                rowsForColumn = (column) =>
                {
                    var rows = height;
                    if (column < height)
                    {
                        rows -= height - column - 1;
                    }
                    if (column >= gridWith - height)
                    {
                        rows -= column - (gridWith - height);
                    }
                    return rows;
                };

                rowOffset = (column) => Math.Max(0, height - gridWith + column);
                break;
        }



        rowOffsets = new List<int>(gridWith);
        for (int i = 0; i < gridWith; i++)
            rowOffsets.Add(0);

        grid = new List<List<DeltaCell>>(gridWith);
        for (int i = 0; i < gridWith; i++)
        {
            rowOffsets[i] = rowOffset(i);
            for (int j = 0; j < rowsForColumn(i); i++)
            {
                grid[i][j] = new DeltaCell(this, new Position2D(i, j + rowOffsets[i]));
            }
        }
    }


}
/*
override fun drawTo(canvas: Canvas, style: Configuration.Style)
{
    var maxHeight = 0
        for (x in 0 until grid.size)
    {
        val height = grid[x].size + rowOffsets[x]
            if (height > maxHeight) maxHeight = height
        }

    val csive = style.cellSize
        val cheight = sqrt(3.0) / 2 * csive
        canvas.init((grid.size / 2.0 + 0.5) * csive + style.stroke.lineWidth,
                maxHeight * cheight + style.stroke.lineWidth)

        // Draw the background
    if (style.backgroundColor != null)
    {
        canvas.zIndex = 0
            canvas.color = style.backgroundColor
            canvas.drawRect(0.0, 0.0, canvas.width, canvas.height, true)
        }

    val offset = style.stroke.lineWidth / 2.0
        canvas.translate = Point(offset, offset)

        // Draw the distance map
    if (hasDistanceMap)
    {
        canvas.zIndex = 10

            val distMapColors = style.generateDistanceMapColors(this)
            drawForEachCell {
            cell, x, y, flatTopped->
canvas.color = distMapColors[cell.distanceMapValue]
                val vertices = if (flatTopped)
            {
                listOf(Point(x * csive / 2, y * cheight),
                        Point((x + 2) * csive / 2, y * cheight),
                        Point((x + 1) * csive / 2, (y + 1) * cheight))
                }
            else
            {
                listOf(Point(x * csive / 2, (y + 1) * cheight),
                        Point((x + 2) * csive / 2, (y + 1) * cheight),
                        Point((x + 1) * csive / 2, y * cheight))
                }
            canvas.drawPath(vertices, true)
            }
    }

    // Draw the maze
    canvas.zIndex = 20
        canvas.color = style.color
        canvas.stroke = style.stroke
        drawForEachCell {
        cell, x, y, flatTopped->
            if (cell.hasSide(Side.BASE))
        {
            if (flatTopped)
            {
                canvas.drawLine(x * csive / 2, y * cheight,
                        (x + 2) * csive / 2, y * cheight)
                }
            else if (cell.getCellOnSide(Side.BASE) == null)
            {
                canvas.drawLine(x * csive / 2, (y + 1) * cheight,
                        (x + 2) * csive / 2, (y + 1) * cheight)
                }
        }
        if (cell.hasSide(Side.EAST))
        {
            if (flatTopped)
            {
                canvas.drawLine((x + 2) * csive / 2, y * cheight,
                        (x + 1) * csive / 2, (y + 1) * cheight)
                }
            else if (cell.getCellOnSide(Side.EAST) == null)
            {
                canvas.drawLine((x + 1) * csive / 2, y * cheight,
                        (x + 2) * csive / 2, (y + 1) * cheight)
                }
        }
        if (cell.hasSide(Side.WEST))
        {
            if (flatTopped)
            {
                canvas.drawLine(x * csive / 2, y * cheight,
                        (x + 1) * csive / 2, (y + 1) * cheight)
                }
            else if (cell.getCellOnSide(Side.WEST) == null)
            {
                canvas.drawLine((x + 1) * csive / 2, y * cheight,
                        x * csive / 2, (y + 1) * cheight)
                }
        }
    }

    // Draw the solution
    if (solution != null)
    {
        canvas.zIndex = 30
            canvas.color = style.solutionColor
            canvas.stroke = style.solutionStroke

            val points = LinkedList<Point>()
            for (cell in solution!!)
        {
            val pos = cell.position as Position2D
                val flatTopped = (pos.x + pos.y) % 2 == 0
                val px = (pos.x + 1.0) * csive / 2.0
                val py = (pos.y + (if (flatTopped) 1 else 2) / 3.0) *cheight
                points.add(Point(px, py))
            }
    canvas.drawPath(points)
        }
    }

    */
/**
 * For each cell, call [draw] with the cell and some parameters.
 
private inline fun drawForEachCell(draw: (cell: DeltaCell, x: Int, y: Int, flatTopped: Boolean) -> Unit) {
    for (x in 0 until grid.size)
{
for (y in 0 until grid[x].size)
{
    val actualY = y + rowOffsets[x]
            draw(grid[x][y], x, actualY, (x + actualY) % 2 == 0)
        }
}
}

}*/
