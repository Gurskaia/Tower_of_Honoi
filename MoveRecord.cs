namespace Gurskaia_Tower_Of_Hanoi
{
    public class MoveRecord
    {
        public int MoveNumber { get; }
        public int Disc { get; }
        public int From { get; }
        public int To { get; }

        public MoveRecord(int moveNumber, int disc, int from, int to)
        {
            MoveNumber = moveNumber;
            Disc = disc;
            From = from;
            To = to;
        }
    }
}
