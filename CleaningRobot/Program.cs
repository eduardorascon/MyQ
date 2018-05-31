using System;
using System.Collections.Generic;
using CleaningRobot.BasicInstructions;
using Newtonsoft.Json;
using System.IO;

namespace CleaningRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            InputFile inputFile = JsonConvert.DeserializeObject<InputFile>(File.ReadAllText(@"d:\json\test1.json"));

            CleaningRobot bot = new CleaningRobot(inputFile.battery, inputFile.start.x, inputFile.start.y, inputFile.start.facing);
            List<IBasicInstruction> commandList = InstructionsHelper.ConvertToBasicInstrucctions(inputFile.commands);
            string[,] map = inputFile.map;

            Simulation simulation = new Simulation(bot, commandList, map);
            simulation.Run();
        }
    }

    public class OutputFile
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

    public class InputFile
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
        public Simulation(CleaningRobot cleaningRobot, List<IBasicInstruction> commandList, String[,] map)
        {
            this.instructionsList = commandList;
            this.bot = cleaningRobot;
            bot.Map = map;
        }

        public void Run()
        {
            bot.ExecuteInstructions(instructionsList);
            bot.PrintResult();
        }
    }
}
