using System.Collections.Generic;

namespace CleaningRobot
{
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
}