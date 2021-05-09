using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

public class MazeGenerator
{
    private Configuration config;
    /**
     * Generate, solve and export all mazes described by [config].
     */
    public MazeGenerator(Configuration config)
    {
        this.config = config;
        
    }
    public void Generate()
    {
        var mazeSet = config.mazeSets[0];

        var maze = mazeSet.creator();
        maze = GenerateMaze(maze, mazeSet);

        ExportToJson(maze);
        maze.ForEachCell((cell) =>
        {
            Debug.WriteLine((cell.position as Position2D).x + "," + (cell.position as Position2D).y + "," + cell.value);
            cell.allSides.ForEach((side) =>
            {
                Debug.WriteLine(side.symbol + " open: " + !cell.HasSide(side));
                
                var tempcell = cell.GetCellOnSide(side);
                if(tempcell != null)
                    Debug.WriteLine("neighbor: " + (tempcell.position as Position2D).x + "," + (tempcell.position as Position2D).y);
                else
                {
                    Debug.WriteLine("nothing");
                }
            });
        });
    }

    private Maze GenerateMaze(Maze maze, Configuration.MazeSet mazeSet)
    {
        var vmaze = maze;
        mazeSet.generator.Generate(vmaze);

        return vmaze;
        
    }

    private void ExportToJson(Maze maze)
    {
        string json = maze.ToJson(this.config.jsonOutput);
        string path = this.config.jsonOutput.path +"output.json";
        Directory.CreateDirectory(this.config.jsonOutput.path);
        File.WriteAllText(path, json);
    }

}

/*
    val nbFmt = NumberFormat.getInstance()

        val totalDuration = measureTimeMillis {
        for (i in 0 until config.mazeSets.size)
        {
            val mazeSet = config.mazeSets[i]

                var indent1 = ""
                if (config.mazeSets.size > 1)
            {
                println("Generating maze set '${mazeSet.name}' (${i + 1} / ${config.mazeSets.size})")
                    indent1 += "  "
                }

            for (j in 1..mazeSet.count)
            {
                var filename = mazeSet.name
                    var indent2 = indent1
                    if (mazeSet.count > 1)
                {
                    println(indent1 + "Generating maze $j / ${mazeSet.count}")
                        indent2 += "  "
                        filename += "-$j"
                    }

                // Generate
                print(indent2 + "Generating...\r")
                    var maze = mazeSet.creator()
                    var duration = measureTimeMillis {
                    maze = generateMaze(maze, mazeSet)
                    }
                println(indent2 + "Generated in ${nbFmt.format(duration)} ms")

                    if (mazeSet.separateExport)
                {
                    print(indent2 + "Exporting maze...\r")
                        duration = exportMaze(maze, filename)
                        println(indent2 + "Exported maze in ${nbFmt.format(duration)} ms")
                    }

                // Solve
                if (mazeSet.solve)
                {
                    print(indent2 + "Solving...\r")
                        duration = measureTimeMillis { maze.solve() }
                    println(indent2 + "Solved in ${nbFmt.format(duration)} ms")

                        if (mazeSet.separateExport)
                    {
                        print(indent2 + "Exporting solved maze...\r")
                            duration = exportMaze(maze, "${filename}_solution")
                            println(indent2 + "Exported solved maze in ${nbFmt.format(duration)} ms")
                        }
                }

                // Distance map
                if (mazeSet.distanceMap)
                {
                    if (mazeSet.separateExport && mazeSet.solve)
                    {
                        maze.clearSolution()
                        }

                    print("Generating distance map...\r")
                        duration = measureTimeMillis {
                        maze.generateDistanceMap(mazeSet.distanceMapStart)
                        }
                    println(indent2 + "Distance map generated in ${nbFmt.format(duration)} ms")

                        if (mazeSet.separateExport)
                    {
                        print(indent2 + "Exporting distance mapped maze...\r")
                            duration = exportMaze(maze, "${filename}_distance_map")
                            println(indent2 + "Exported distance mapped maze in ${nbFmt.format(duration)} ms")
                        }
                }



                if (!mazeSet.separateExport)
                {
                    print(indent2 + "Exporting...\r")
                        duration = exportMaze(maze, filename)
                        println(indent2 + "Exported in ${nbFmt.format(duration)} ms")
                    }
            }

            println()
            }
    }

    println("Done in ${nbFmt.format(totalDuration)} ms.")
    }


private fun generateMaze(maze: Maze, mazeSet: Configuration.MazeSet): Maze {
        var vmaze = maze

        // Generate
mazeSet.generator.generate(vmaze)
        if (vmaze is UnicursalOrthogonalMaze) {
            vmaze = UnicursalOrthogonalMaze(vmaze)
        }

        // Add openings
        for (opening in mazeSet.openings) {
            vmaze.createOpening(opening)
        }

        // Braid
        if (mazeSet.braiding != null) {
            vmaze.braid(mazeSet.braiding)
        }

        return vmaze
    }


    private fun exportMaze(maze: Maze, filename: String) = measureTimeMillis {
        val output = config.output
        val style = config.style
        val format = output.format

        val canvas = output.createCanvas()
        canvas.antialiasing = style.antialiasing

        // Draw to canvas
        maze.drawTo(canvas, style)

        // Export to file
val fullFilename = filename + '.' + format.extension
        val file = File(output.path, fullFilename)
        canvas.exportTo(file)
    }

}
*/