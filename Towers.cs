using System;
using System.Collections.Generic;

namespace Gurskaia_Tower_Of_Hanoi
{
    public class Towers
    {
        private Stack<int>[] towers = new Stack<int>[3];
        private int numberOfMoves = 0;
        private bool isComplete;

        public int NumberOfDiscs { get; }
        public int MinimumPossibleMoves { get; }
        public int NumberOfMoves { get { return numberOfMoves; } }
        public bool IsComplete { get { return isComplete; } }

        public Towers(int numberOfDiscs)
        {
            if (numberOfDiscs < 1 || numberOfDiscs > 9)
                throw new InvalidHeightException(numberOfDiscs);

            NumberOfDiscs = numberOfDiscs;
            MinimumPossibleMoves = (int)Math.Pow(2, numberOfDiscs) - 1;
            for (int i = 0; i < 3; i++)
            {
                towers[i] = new Stack<int>();
            }
            for (int i = numberOfDiscs; i > 0; --i)
            {
                towers[0].Push(i);
            }
        }

        public MoveRecord Move(int from, int to)
        {
            --from;
            --to;
            if (from == to) throw new InvalidMoveException(false);
            if (from < 0 || from > 2 || to < 0 || to > 2) throw new InvalidMoveException();
            if (towers[from].Count == 0) throw new InvalidMoveException(from + 1);
            if (towers[from].Count != 0
                && towers[to].Count != 0
                && towers[from].Peek() > towers[to].Peek())
                throw new InvalidMoveException(from + 1, to + 1);
            towers[to].Push(towers[from].Pop());
            ++numberOfMoves;
            if (towers[2].Count == NumberOfDiscs) isComplete = true;
            return new MoveRecord(numberOfMoves, towers[to].Peek(), from + 1, to + 1);
        }

        public int[][] ToArray()
        {
            int[][] arr = new int[3][];
            for (int i = 0; i < 3; i++)
            {
                arr[i] = towers[i].ToArray();
            }
            return arr;
        }

    }
}
