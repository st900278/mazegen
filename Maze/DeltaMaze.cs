using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;
public class DeltaMaze : BaseShapedMaze<DeltaCell>
{
    protected override List<List<DeltaCell>> grid { get; set;}
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
                    else return 2 * width;
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

        grid = new List<List<DeltaCell>>();

        for (int i = 0; i < gridWith; i++)
        {
            grid.Add(new List<DeltaCell>());
            rowOffsets[i] = rowOffset(i);
            Debug.WriteLine(rowsForColumn(i));
            for (int j = 0; j < rowsForColumn(i); j++)
            {
                grid[i].Add(new DeltaCell(this, new Position2D(i, j + rowOffsets[i])));
                Debug.WriteLine("tttt");
            }
        }
    }

    public override string ToJson(Configuration.JsonOutput jsonOutput)
    {
        ExportFormat exportFormat = new ExportFormat();
        ForEachCell((cell) =>
        {
            ExportFormat.Cell tempCell = new ExportFormat.Cell();

            tempCell.gridPosition = Tuple.Create((cell.position as Position2D).x, (cell.position as Position2D).y);

            Tuple<string, double, double> topPoint = Tuple.Create("none", 0.0, 0.0), rightPoint = Tuple.Create("none", 0.0, 0.0), leftPoint = Tuple.Create("none", 0.0, 0.0);
            if (((cell.position as Position2D).x + (cell.position as Position2D).y) % 2 == 1)
            {
                topPoint = Tuple.Create("top", ((cell.position as Position2D).x * 0.5 + 0.5) * jsonOutput.sideLength, (-(cell.position as Position2D).y * Math.Sqrt(3) / 2) * jsonOutput.sideLength);
                leftPoint = Tuple.Create("left", ((cell.position as Position2D).x * 0.5) * jsonOutput.sideLength, (-(cell.position as Position2D).y * Math.Sqrt(3) / 2 - Math.Sqrt(3) / 2) * jsonOutput.sideLength);
                rightPoint = Tuple.Create("right", ((cell.position as Position2D).x * 0.5 + 1) * jsonOutput.sideLength, (-(cell.position as Position2D).y * Math.Sqrt(3) / 2 - Math.Sqrt(3) / 2) * jsonOutput.sideLength);
            }
            else
            {
                topPoint = Tuple.Create("top", ((cell.position as Position2D).x * 0.5 + 0.5) * jsonOutput.sideLength, (-(cell.position as Position2D).y * Math.Sqrt(3) / 2 - Math.Sqrt(3) / 2) * jsonOutput.sideLength);
                leftPoint = Tuple.Create("left", ((cell.position as Position2D).x * 0.5) * jsonOutput.sideLength, (-(cell.position as Position2D).y * Math.Sqrt(3) / 2) * jsonOutput.sideLength);
                rightPoint = Tuple.Create("right", ((cell.position as Position2D).x * 0.5 + 1) * jsonOutput.sideLength, (-(cell.position as Position2D).y * Math.Sqrt(3) / 2) * jsonOutput.sideLength);
            }
            tempCell.pillows.Add(topPoint);
            tempCell.pillows.Add(rightPoint);
            tempCell.pillows.Add(leftPoint);
            tempCell.center = Tuple.Create((topPoint.Item2 + leftPoint.Item2 + rightPoint.Item2) / 3, (topPoint.Item3 + leftPoint.Item3 + rightPoint.Item3) / 3);

            cell.allSides.ForEach((side) =>
            {

                if (!cell.HasSide(side))
                {
                    return;
                }
                
                switch (side.symbol)
                {
                    case "E":
                        tempCell.sides.Add(Tuple.Create("E", "top", "right"));
                        break;
                    case "W":
                        tempCell.sides.Add(Tuple.Create("W", "top", "left"));
                        break;
                    case "B":
                        tempCell.sides.Add(Tuple.Create("B", "left", "right"));
                        break;
                }
            });
            
            exportFormat.cells.Add(tempCell);

        });
        var jsonString = JsonSerializer.Serialize<ExportFormat>(exportFormat);
        return jsonString;
    }
}
