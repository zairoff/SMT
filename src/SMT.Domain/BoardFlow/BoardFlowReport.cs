namespace SMT.Domain.BoardFlow
{
    public class BoardFlowReport
    {
        public int ReaderId { get; set; }

        public int Passed { get; set; }

        public int Missing { get; set; }

        public int PreviousPassed { get; set; }
    }
}
