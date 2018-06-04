using CleaningRobot.BasicInstructions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CleaningRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = @"C:\publicacion\test2.json"; // args[0];
            string outputFile = @"C:\publicacion\test2_result222.json"; // args[1];

            InputJson inputJson = JsonConvert.DeserializeObject<InputJson>(File.ReadAllText(inputFile));

            CleaningRobot bot = new CleaningRobot(inputJson);
            List<IBasicInstruction> commandList = InstructionsHelper.ConvertToBasicInstrucctions(inputJson.commands);

            Simulation simulation = new Simulation(bot, commandList);
            simulation.Run();
            simulation.PrintResult(outputFile);
        }
    }

    public class OutputJson
    {
        public Cell[] visited { get; set; }
        public Cell[] cleaned { get; set; }
        public Final final { get; set; }
        public int battery { get; set; }

        public class Final
        {
            public Cell cell { get; set; }
            public string facing { get; set; }
        }
        public class Cell
        {
            public int x { get; set; }
            public int y { get; set; }
        }
    }

    public class InputJson
    {
        public string[,] map { get; set; }
        public StartJson start { get; set; }
        public string[] commands { get; set; }
        public int battery { get; set; }

        public class StartJson
        {
            public int x { get; set; }
            public int y { get; set; }
            public string facing { get; set; }
        }
    }

    public class Simulation
    {
        private CleaningRobot bot;
        private List<IBasicInstruction> instructionsList;
        public Simulation(CleaningRobot cleaningRobot, List<IBasicInstruction> commandList)
        {
            this.instructionsList = commandList;
            this.bot = cleaningRobot;
        }

        public void Run()
        {
            bot.ExecuteInstructions(instructionsList);
        }

        public void PrintResult(string outputFile)
        {
            OutputJson outputJson = new OutputJson
            {
                visited = bot.visitedCells.ToArray(),
                cleaned = bot.cleanedCells.ToArray(),
                final = new OutputJson.Final
                {
                    cell = new OutputJson.Cell
                    {
                        x = bot.PositionX,
                        y = bot.PositionY
                    },
                    facing = bot.FacingTo
                },
                battery = bot.Battery
            };

            try
            {
                File.WriteAllText(outputFile, JsonConvert.SerializeObject(outputJson, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.Write("Privilegios?" + ex.Message);
                Console.ReadLine();
            }
        }
    }
}
