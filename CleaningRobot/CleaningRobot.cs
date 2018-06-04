using CleaningRobot.BasicInstructions;
using System;
using System.Collections.Generic;

namespace CleaningRobot
{
    public class CleaningRobot
    {
        public List<OutputJson.Cell> visitedCells { get; private set; }
        public List<OutputJson.Cell> cleanedCells { get; set; }
        private int backOffStrategy = 0;

        public string[,] Map { get; private set; }
        public int Battery { get; private set; }
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public string FacingTo { get; private set; }

        public CleaningRobot(InputJson inputJson)
        {
            this.Battery = inputJson.battery;
            this.PositionX = inputJson.start.x;
            this.PositionY = inputJson.start.y;
            this.FacingTo = inputJson.start.facing;

            this.visitedCells = new List<OutputJson.Cell>();
            visitedCells.Add(new OutputJson.Cell { x = PositionX, y = PositionY });

            this.cleanedCells = new List<OutputJson.Cell>();
            this.Map = parseMap(inputJson.map);

        }

        private static string[,] parseMap(string[,] map)
        {
            int lengthX = map.GetLength(0);
            int lengthY = map.GetLength(1);
            string[,] tempMap = new string[lengthX, lengthY];

            for (int i = 0; i < lengthX; i++)
                for (int j = 0; j < lengthY; j++)
                    tempMap[i, j] = map[j, i];

            return tempMap;
        }

        public void ExecuteInstructions(List<IBasicInstruction> instructionsList)
        {
            if (backOffStrategy == 6)
                return;

            foreach (IBasicInstruction instruction in instructionsList)
            {
                if (this.Battery - instruction.EnergyConsumtion < 0)
                    break;

                switch (instruction.InstrucctionName)
                {
                    case "TL":
                        TurnLeft();
                        break;
                    case "TR":
                        TurnRight();
                        break;
                    case "A":
                        GoForward();
                        break;
                    case "B":
                        GoBack();
                        break;
                    case "C":
                        Clean();
                        break;
                }

                this.Battery -= instruction.EnergyConsumtion;
            }
        }

        private void GoBack()
        {
            switch (this.FacingTo)
            {
                case "N":
                    this.PositionY += 1;
                    break;
                case "S":
                    this.PositionY -= 1;
                    break;
                case "E":
                    this.PositionX -= 1;
                    break;
                case "W":
                    this.PositionX += 1;
                    break;
            }

            visitedCells.Add(new OutputJson.Cell { x = PositionX, y = PositionY });
        }

        private void GoForward()
        {
            int tempPositionX = PositionX;
            int tempPositionY = PositionY;

            switch (this.FacingTo)
            {
                case "N":
                    this.PositionY -= 1;
                    break;
                case "S":
                    this.PositionY += 1;
                    break;
                case "E":
                    this.PositionX += 1;
                    break;
                case "W":
                    this.PositionX -= 1;
                    break;
            }

            bool isOutOfBounds = PositionX < 0 || PositionY < 0 || PositionX >= Map.GetLength(0) || PositionY >= Map.GetLength(1);
            bool isValidCell = isOutOfBounds ? false : Map[PositionX, PositionY].Equals("S");
            if (isValidCell == false)
            {
                PositionX = tempPositionX;
                PositionY = tempPositionY;
                ExecuteBackOffStrategy(++backOffStrategy);
                return;
            }

            visitedCells.Add(new OutputJson.Cell { x = PositionX, y = PositionY });
        }

        private void ExecuteBackOffStrategy(int backOffStrategyNumber)
        {
            List<IBasicInstruction> instructions = null;
            switch (backOffStrategyNumber)
            {
                case 1:
                    instructions = InstructionsHelper.ConvertToBasicInstrucctions(new[] { "TR", "A" });
                    break;
                case 2:
                    instructions = InstructionsHelper.ConvertToBasicInstrucctions(new[] { "TL", "B", "TR", "A" });
                    break;
                case 3:
                case 5:
                    instructions = InstructionsHelper.ConvertToBasicInstrucctions(new[] { "TL", "TL", "A" });
                    break;
                case 4:
                    instructions = InstructionsHelper.ConvertToBasicInstrucctions(new[] { "TR", "B", "TR", "A" });
                    break;
                default:
                    return;
            }

            ExecuteInstructions(instructions);
            backOffStrategy = 0;
        }

        private void Clean()
        {
            cleanedCells.Add(new OutputJson.Cell { x = PositionX, y = PositionY });
        }

        private void TurnLeft()
        {
            switch (this.FacingTo)
            {
                case "N":
                    this.FacingTo = "W";
                    break;
                case "S":
                    this.FacingTo = "E";
                    break;
                case "E":
                    this.FacingTo = "N";
                    break;
                case "W":
                    this.FacingTo = "S";
                    break;
            }
        }

        private void TurnRight()
        {
            switch (this.FacingTo)
            {
                case "N":
                    this.FacingTo = "E";
                    break;
                case "S":
                    this.FacingTo = "W";
                    break;
                case "E":
                    this.FacingTo = "S";
                    break;
                case "W":
                    this.FacingTo = "N";
                    break;
            }
        }
    }
}