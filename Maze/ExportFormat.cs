using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;


class ExportFormat
{
    [JsonPropertyName("Cells")]
    public List<Cell> cells { get; set; } = new List<Cell>();
    public class Cell
    {
        [JsonPropertyName("GridPosition")]
        public Tuple<int, int> gridPosition { get; set; }

        [JsonPropertyName("Center")]
        public Tuple<double, double> center { get; set; }

        [JsonPropertyName("Pillows")]
        public List<Tuple<string, double, double>> pillows { get; set; } = new List<Tuple<string, double, double>>();

        [JsonPropertyName("Sides")]
        public List<Tuple<string, string>> sides { get; set; } = new List<Tuple<string, string>>();
    }
}