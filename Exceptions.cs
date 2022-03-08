using System;

namespace Gurskaia_Tower_Of_Hanoi
{
     class InvalidMoveException : Exception
    {
        public InvalidMoveException()
            : base("Invalid tower number.") { }

        public InvalidMoveException(bool flag)
        : base("Move cancelled.") { }

        public InvalidMoveException(int from)
            : base($"Tower {from} is empty.") { }

        public InvalidMoveException(int from, int to)
            : base($"Top disc of tower {from} is larger than top disc on tower {to}.") { }
    }

     class InvalidHeightException : Exception
    {
        public InvalidHeightException(int height) :
            base(height + " is invalid height. It must be from 1 to 9.")
        { }
    }
}

