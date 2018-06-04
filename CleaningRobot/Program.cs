using CleaningRobot.BasicInstructions;
using Newtonsoft.Json;
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
}
