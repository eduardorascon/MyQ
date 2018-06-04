namespace CleaningRobot
{
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
}