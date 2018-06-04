using CleaningRobot.BasicInstructions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CleaningRobot
{

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