﻿using System;
using System.Collections.Generic;
using CleaningRobot.BasicInstructions;

namespace CleaningRobot
{
    public class CleaningRobot
    {
        private List<string> visitedCells, cleanedCells;
        private int backOffStrategy = 0;
        public string[,] Map { get; set; }
        protected int Battery { get; set; }
        protected int PositionX { get; set; }
        protected int PositionY { get; set; }
        protected String FacingTo { get; set; }

        public CleaningRobot(int battery, int positionX, int positionY, String facing)
        {
            this.Battery = battery;
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.FacingTo = facing;

            this.visitedCells = new List<string>();
            this.cleanedCells = new List<string>();
        }

        public void ExecuteInstructions(List<IBasicInstruction> instruccionsList)
        {
            if (backOffStrategy == 6)
                return;

            foreach (IBasicInstruction instruction in instruccionsList)
            {
                if (this.Battery - instruction.EnergyConsumtion < 0)
                    break;

                this.Battery -= instruction.EnergyConsumtion;

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
            }
        }

        internal void PrintResult()
        {
            Console.WriteLine(string.Format("visited : {0}", visitedCells.Count));
            Console.WriteLine(string.Format("cleaned : {0}", cleanedCells.Count));
            Console.WriteLine(string.Format("final : X:{0}, Y:{1}, facing:{2}", PositionX, PositionY, FacingTo));
            Console.WriteLine(string.Format("battery : {0}", Battery));
            Console.ReadLine();
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

            visitedCells.Add(string.Format("X:{0}, Y:{1}", this.PositionX, this.PositionY));
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
            if (isValidCell == false || isOutOfBounds)
            {
                PositionX = tempPositionX;
                PositionY = tempPositionY;
                ExecuteBackOffStrategy(backOffStrategy++);
            }

            visitedCells.Add(string.Format("X:{0}, Y:{1}", this.PositionX, this.PositionY));
        }

        private void ExecuteBackOffStrategy(int backOffStrategy)
        {
            List<IBasicInstruction> instructions = null;
            switch (backOffStrategy)
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
            cleanedCells.Add(string.Format("X:{0}, Y:{1}", this.PositionX, this.PositionY));
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