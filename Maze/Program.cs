using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Diagnostics;
//using System.Dynamic;
namespace MazeCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Debug.WriteLine("Debug Information-Product Starting "); 
            //Debug.WriteLine(Directory.GetCurrentDirectory());

            var path = "test.json";
            var reader = new StreamReader(path, true); // 上書き
            string json = reader.ReadToEnd();

            var config = JsonSerializer.Deserialize<ConfigurationFormat>(json);
            Debug.WriteLine(config.mazes[0].count);
            Debug.WriteLine(config.mazes[0].count == null);
            new MazeGenerator(new ConfigurationParser().Parse(config)).Generate();
            //new MazeGenerator(new Configuration(config.mazes, config.output, config.style))
            //using dynamic document = System.Text.Json.JsonDocument.Parse(@"{ ""prop1"": ""value1"", ""prop2"": ""value2""}");

            //new MazeGenerator(new ConfigurationParser(.);
            /*
            try
            {
                if (args.Count() != 0)
                {
                    foreach(var arg in args)
                    {
                        var file = File(arg)
                        if (file.exists())
                        {
                            if (args.size > 1)
                            {
                                println("Configuration file: ${file.absolutePath}")
                            }
                            val configJson = FileInputStream(file).use { JSONObject(JSONTokener(it)) }
                            MazeGenerator((ConfigurationParser.parse(configJson))).generate()
                        }
                        else
                        {
                            paramError("Configuration file at ${file.absolutePath} doesn't exists.")
                        }
                    }
                }
                else
                {
                    paramError("No configuration file provided.")
                }
            }
            catch (exception: ParameterException) {
                println("ERROR: ${exception.message}")
            }
            */
        }
    }
}

