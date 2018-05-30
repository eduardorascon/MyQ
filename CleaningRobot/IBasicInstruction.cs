using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningRobot.BasicInstructions
{
    public interface IBasicInstruction
    {
        string InstrucctionName { get; }
        int EnergyConsumtion { get; }
    }

    public static class InstructionsHelper
    {
        public static List<IBasicInstruction> ConvertToBasicInstrucctions(string[] commands)
        {
            List<IBasicInstruction> instructions = new List<IBasicInstruction>();

            foreach (string command in commands)
            {
                switch (command)
                {
                    case "TL":
                        instructions.Add(new TurnLeft());
                        break;
                    case "TR":
                        instructions.Add(new TurnRight());
                        break;
                    case "A":
                        instructions.Add(new Advance());
                        break;
                    case "B":
                        instructions.Add(new Back());
                        break;
                    case "C":
                        instructions.Add(new Clean());
                        break;
                    default:
                        throw new ApplicationException("Invalid command.");
                }
            }

            return instructions;
        }
    }

    class TurnLeft : IBasicInstruction
    {

        public int EnergyConsumtion
        {
            get
            {
                return 1;
            }
        }

        public string InstrucctionName
        {
            get
            {
                return "TL";
            }
        }
    }

    class TurnRight : IBasicInstruction
    {
        public int EnergyConsumtion
        {
            get
            {
                return 1;
            }
        }

        public string InstrucctionName
        {
            get
            {
                return "TR";
            }
        }
    }

    class Advance : IBasicInstruction
    {
        public int EnergyConsumtion
        {
            get
            {
                return 2;
            }
        }

        public string InstrucctionName
        {
            get
            {
                return "A";
            }
        }
    }

    class Back : IBasicInstruction
    {
        public int EnergyConsumtion
        {
            get
            {
                return 4;
            }
        }
        public string InstrucctionName
        {
            get
            {
                return "B";
            }
        }
    }

    class Clean : IBasicInstruction
    {
        public int EnergyConsumtion
        {
            get
            {
                return 5;
            }
        }

        public string InstrucctionName
        {
            get
            {
                return "C";
            }
        }
    }
}
