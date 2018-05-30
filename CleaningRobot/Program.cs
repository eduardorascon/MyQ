using System;
using System.Collections.Generic;
using CleaningRobot.BasicInstructions;

namespace CleaningRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            CleaningRobot bot = new CleaningRobot(80, 1, 0, "N");
            List<IBasicInstruction> commandList = InstructionsHelper.ConvertToBasicInstrucctions(new string[] { "TL", "A", "C", "A", "C", "TR", "A", "C" });
            String[,] map = new string[4, 4] {
                { "S", "S", "S", "S" },
                { "S", "S", "C", "S" },
                { "S", "S", "S", "S" },
                { "S", "null", "S", "S" }
            };

            Simulation simulation = new Simulation(bot, commandList, map);
            simulation.Run();
        }
    }

    public class Simulation
    {
        private CleaningRobot bot;
        private List<IBasicInstruction> instructionsList;
        private String[,] map;
        public Simulation(CleaningRobot cleaningRobot, List<IBasicInstruction> commandList, String[,] map)
        {
            this.instructionsList = commandList;
            this.bot = cleaningRobot;
            this.map = map;
        }

        public void Run()
        {
            bot.ExecuteInstructions(instructionsList, map);

        }
    }
}
