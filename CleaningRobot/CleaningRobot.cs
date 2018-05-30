using System;
using System.Collections.Generic;
using CleaningRobot.BasicInstructions;

namespace CleaningRobot
{

    public class CleaningRobot
    {
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
        }

        public void ExecuteInstructions(List<IBasicInstruction> instruccionsList, String[,] map)
        {
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

        //TODO
        private void GoBack()
        {
            throw new NotImplementedException();
        }

        //TODO
        private void GoForward()
        {
            throw new NotImplementedException();
        }

        //TODO
        private void Clean()
        {
            throw new NotImplementedException();
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