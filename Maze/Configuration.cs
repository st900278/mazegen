using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Class representing the JSON configuration file content.
 * Use [ConfigurationParser] to create it from JSON.
 */
public class Configuration 
{
    public List<MazeSet> mazeSets;
    public JsonOutput jsonOutput;
    public Output output;
    public Style style;

    public Configuration(List<MazeSet> mazeSets, JsonOutput jsonOutput, Output output, Style style)
    {
        this.mazeSets = mazeSets;
        this.jsonOutput = jsonOutput;
        this.output = output;
        this.style = style;
    }
    public class MazeSet
    {
        public string name { get; set; }
        public int count { get; set; }
        public Func<Maze> creator { get; set; }
        public Generator generator { get; set; }
        public Maze.Braiding braiding { get; set; }
        public List<Position> openings { get; set; }
        public bool solve { get; set; }
        public bool distanceMap { get; set; }
        public Position distanceMapStart { get; set; }
        public bool separtateExport { get; set; }

        public MazeSet(string name, int count, Func<Maze> creator, Generator generator, Maze.Braiding braiding, List<Position> openings, bool solve, bool distanceMap, Position distanceMapStart, bool separtateExport)
        {
            this.name = name;
            this.count = count;
            this.creator = creator;
            this.generator = generator;
            this.braiding = braiding;
            this.openings = openings;
            this.solve = solve;
            this.distanceMap = distanceMap;
            this.distanceMapStart = distanceMapStart;
            this.separtateExport = separtateExport;
        }
    }

    public class JsonOutput
    {
        public double sideLength { get; set; }
        public string path { get; set; }
        public JsonOutput(string path,  double sideLength)
        {
            this.path = path;
            this.sideLength = sideLength;
        }
    }
    public abstract class Output
    {

    }
    public class Style 
    {
        public double cellSize;
        public Color backgroundColor;
        public Color color;
        // BasicStroke stroke;
        public Color solutionColor;
        //BasicStoke solutionStroke
        int distanceMapRange;
        public List<Color> distanceMapColors;
        public bool antialiasing;

    }

    
}

public enum Color
{
    Green,
    Red,
    Black,
    White,
    Blue
}