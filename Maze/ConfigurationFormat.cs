using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
public class ConfigurationFormat
{
    [JsonPropertyName("mazes")]
    public List<MazeSetting> mazes { get; set; }
    [JsonPropertyName("jsonoutput")]
    public JsonOutputStyle json { get; set; }

    [JsonPropertyName("output")]
    public OutputStyle output { get; set; } 
    [JsonPropertyName("style")]
    public Style style { get; set; } = new Style();

    

    public class MazeSetting
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("count")]
        public int count { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("size")]
        public int size { get; set; }

        [JsonPropertyName("algorithm")]
        public string algorithm { get; set; }

        [JsonPropertyName("braid")]
        public int braid { get; set; }

        [JsonPropertyName("shape")]
        public string shape { get; set; }


        [JsonPropertyName("openings")]
        public List<List<int>> openings { get; set; }

        [JsonPropertyName("solve")]
        public bool solve { get; set; }

        [JsonPropertyName("distanceMap")]
        public bool distanceMap { get; set; }

        [JsonPropertyName("distanceMapStart")]
        public List<int> distanceMapStart { get; set; }

        [JsonPropertyName("separateExport")]
        public bool separateExport { get; set; }
    }

    public class JsonOutputStyle
    {
        [JsonPropertyName("path")]
        public string path { get; set; }
        [JsonPropertyName("sideLength")]
        public double sideLength { get; set; }
        
    }
    public class OutputStyle
    {
        [JsonPropertyName("format")]
        public string format { get; set; }
        [JsonPropertyName("path")]
        public string path { get; set; }
        [JsonPropertyName("svgOptimization")]
        public int svgOptimization { get; set; }
        [JsonPropertyName("svgPrecision")]
        public int svgPrecision { get; set; }
    }
    public class Style
    {
        [JsonPropertyName("cellSize")]
        public int cellSize { get; set; }

        [JsonPropertyName("backgroundColor")]
        public string backgroundColor { get; set; }

        [JsonPropertyName("color")]
        public string color { get; set; }

        [JsonPropertyName("strokeWidth")]
        public int strokeWidth { get; set; }

        [JsonPropertyName("solutionColor")]
        public string solutionColor { get; set; }

        [JsonPropertyName("solutionStrokeWidth")]
        public int solutionStrokeWidth { get; set; }

        [JsonPropertyName("strokeCap")]
        public string strokeCap { get; set; }

        [JsonPropertyName("distanceMapRange")]
        public string distanceMapRange { get; set; }

        [JsonPropertyName("distanceMapColors")]
        public List<string> distanceMapColors { get; set; }
        
        [JsonPropertyName("antialiasing")]
        public bool antialiasing { get; set; }
    }
}