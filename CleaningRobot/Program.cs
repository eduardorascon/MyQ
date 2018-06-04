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
            string inputFile = args[0];
            string outputFile = args[1];

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
            public int X { get; set; }
            public int Y { get; set; }
            public string facing { get; set; }
        }
        public class Cell
        {
            public int X { get; set; }
            public int Y { get; set; }

            public override int GetHashCode()
            {
                return this.X.GetHashCode() + this.Y.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Cell tempCell = (Cell)obj;
                return this.X == tempCell.X && this.Y == tempCell.Y;
            }
        }

        public class CellSorter : IComparer<Cell>
        {
            public int Compare(Cell cell1, Cell cell2)
            {
                int compareX = cell1.X.CompareTo(cell2.X);

                if (compareX == 0)
                    return cell1.Y.CompareTo(cell2.Y);

                return compareX;
            }
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
            IComparer<OutputJson.Cell> cellSorter = new OutputJson.CellSorter();
            List<OutputJson.Cell> tempVisited = new List<OutputJson.Cell>(bot.visitedCells);
            tempVisited.Sort(cellSorter);
            List<OutputJson.Cell> tempCleaned = new List<OutputJson.Cell>(bot.cleanedCells);
            tempCleaned.Sort(cellSorter);

            OutputJson outputJson = new OutputJson
            {
                visited = tempVisited.ToArray(),
                cleaned = tempCleaned.ToArray(),
                final = new OutputJson.Final
                {
                    X = bot.PositionX,
                    Y = bot.PositionY,
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
                Console.Write("" + ex.Message);
                Console.ReadLine();
            }
        }
    }
}
