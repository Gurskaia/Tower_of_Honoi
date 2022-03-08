using System;
using System.Collections.Generic;
using System.Threading;
using static System.Console;


namespace Gurskaia_Tower_Of_Hanoi
{
    class State
    {
        public int N, From, To, Temp, Step;

        public State(int n, int from, int to, int temp, int step)
        {
            N = n;
            From = from;
            To = to;
            Temp = temp;
            Step = step;
        }
    }

    class Game
    {
        static private Queue<MoveRecord> queue;
        static private Towers tower;

        private static void moveTowerStepByStep()
        {
            Clear();
            TowerUtilities.DisplayTowers(tower);
            MoveRecord turn;
            Stack<State> stack = new Stack<State>();
            WriteLine("\nPress a key to see next move");
            if (ReadKey().KeyChar.ToString().ToUpper() == "X") return;
            stack.Push(new State(tower.NumberOfDiscs, 1, 3, 2, 0));
            while (stack.Count > 0)
            {
                State state = stack.Peek();
                switch (state.Step)
                {
                    case 0:
                        if (state.N == 0)
                        {
                            stack.Pop();
                        }
                        else
                        {
                            ++state.Step;
                            stack.Push(new State(state.N - 1, state.From, state.Temp, state.To, 0));
                        }
                        break;
                    case 1:
                        turn = tower.Move(state.From, state.To);
                        queue.Enqueue(turn);
                        TowerUtilities.DisplayTowers(tower);
                        WriteLine($"Move {turn.MoveNumber} complete.  Successfully moved disc from pole {turn.From} to pole {turn.To}.");
                        if (tower.IsComplete) return;
                        WriteLine("Press a key to see next move");
                        if (ReadKey().KeyChar.ToString().ToUpper() == "X") return;
                        ++state.Step;
                        stack.Push(new State(state.N - 1, state.Temp, state.To, state.From, 0));
                        break;
                    case 2:
                        stack.Pop();
                        break;
                }
            }
        }

        private static void moveTowerAuto(int from, int to, int temp, int n)
        {
            if (n > 1) moveTowerAuto(from, temp, to, n - 1);
            MoveRecord turn = tower.Move(from, to);
            queue.Enqueue(turn);
            TowerUtilities.DisplayTowers(tower);
            WriteLine($"Move {turn.MoveNumber} complete.  Successfully moved disc from pole {turn.From} to pole {turn.To}.");

            Thread.Sleep(300);
            if (n > 1) moveTowerAuto(temp, to, from, n - 1);
        }

        public static void run()
        {
            queue = new Queue<MoveRecord>();
            int numberOfDiscs = 0;
            MoveRecord temp;
            do
            {
                Write("How many discs in your tower (default is 5, max is 9): ");
                int.TryParse(ReadKey().KeyChar.ToString(), out numberOfDiscs);
                tower = numberOfDiscs == 0 ? new Towers(5) : new Towers(numberOfDiscs);
                string key;
                do
                {
                    Clear();
                    TowerUtilities.DisplayTowers(tower);
                    WriteLine("\nOptions:");
                    WriteLine("- M - Solve the puzzle manually");
                    WriteLine("- A - Auto-solve");
                    WriteLine("- S - Auto-solve step-by-step");
                    Write("Choose an approach: ");
                    key = ReadKey().KeyChar.ToString().ToUpper();

                } while (key != "M" && key != "A" && key != "S");

                if (key == "M")
                {
                    game();
                }
                else if (key == "A")
                {
                    moveTowerAuto(1, 3, 2, tower.NumberOfDiscs);
                    WriteLine($"Number of moves: {tower.NumberOfMoves}");
                }
                else if (key == "S")
                {
                    moveTowerStepByStep();
                }

                Write("\nWould you like to see the list of moves? (Y): ");
                if (ReadKey().KeyChar.ToString().ToUpper() == "Y")
                {
                    WriteLine();
                    WriteLine();
                    while (queue.Count > 0)
                    {
                        temp = queue.Dequeue();
                        WriteLine($" {temp.MoveNumber}. Disc {temp.Disc} moved from tower {temp.From} to tower {temp.To}");
                    }
                }
                WriteLine("\nWant to try again? (Y): ");
                if (ReadKey().KeyChar.ToString().ToUpper() != "Y") return;
                Clear();
            } while (true);
        }

        private static void game()
        {
            int from = 0,
                to = 0;


            char key;

            const string CtrlZ = "\u001a";
            const string CtrlY = "\u0019";

            bool needUndo = false,
                 needRedo = false,
                 turnComplete = false;

            Stack<MoveRecord> undo = new Stack<MoveRecord>();
            Stack<MoveRecord> redo = new Stack<MoveRecord>();
            MoveRecord temp = null;

            Clear();



            while (true)
            {
                TowerUtilities.DisplayTowers(tower);
                WriteLine();
                if (needUndo)
                {
                    if (undo.Count == 0)
                    {
                        WriteLine("Can't undo.");
                    }
                    else
                    {
                        WriteLine($"Move {tower.NumberOfMoves} complete by undo of move {temp.MoveNumber}.  Disc {temp.Disc} restored to tower {temp.From} from tower {temp.To}\n");
                    }
                    needUndo = false;
                }
                if (needRedo)
                {
                    if (redo.Count == 0)
                    {
                        WriteLine("Can't redo.");
                    }
                    else
                    {
                        WriteLine($"Move {tower.NumberOfMoves} complete by redo of move {temp.MoveNumber}.  Disc {temp.Disc} returned to tower {temp.To} from tower {temp.From}\n");
                    }
                    needRedo = false;
                }
                else if (turnComplete)
                {
                    turnComplete = false;
                    queue.Enqueue(temp);
                    undo.Push(temp);
                    WriteLine($"Move {tower.NumberOfMoves} complete. Successfully moved disc from tower {from} to tower {to}\n");
                    if (tower.IsComplete) break;
                }
                WriteLine($"Move {tower.NumberOfMoves + 1}:");
                if (undo.Count == 0)
                {
                    Write("Enter 'from' tower number, or \"x\" to quit: ");
                }
                else if (redo.Count == 0)
                {
                    Write("Enter 'from' tower number, \"Ctrl+z\" to undo, or \"x\" to quit: ");
                }
                else
                {
                    Write("Enter 'from' tower number, \"Ctrl+z\" to undo, \"Ctrl+y\" to redo,  or \"x\" to quit: ");
                }
                key = ReadKey().KeyChar;
                if (key == 'x' || key == 'X') break;
                if (key.ToString() == CtrlZ && undo.Count != 0)
                {
                    temp = undo.Pop();
                    redo.Push(temp);
                    queue.Enqueue(tower.Move(temp.To, temp.From));
                    needUndo = true;
                    continue;
                }
                if (key.ToString() == CtrlY && redo.Count != 0)
                {
                    temp = redo.Pop();
                    undo.Push(temp);
                    queue.Enqueue(tower.Move(temp.From, temp.To));
                    needRedo = true;
                    continue;
                }
                int.TryParse(key.ToString(), out from);

                Write("\n\nEnter 'to' tower number or enter to cancel: ");
                int.TryParse(ReadKey().KeyChar.ToString(), out to);
                if (to == 0) continue;
                WriteLine('\n');
                try
                {
                    temp = tower.Move(from, to);
                    turnComplete = true;
                }
                catch (Exception e)
                {
                    WriteLine(e.Message);
                    ReadKey();
                }
            }

            if (!tower.IsComplete)
            {
                WriteLine($"\n\nWell, you hung in there for {tower.NumberOfMoves} moves. Nice try.");
            }
            else if (tower.NumberOfMoves != tower.MinimumPossibleMoves)
            {
                WriteLine($"It took you {tower.NumberOfMoves} moves. Not bad, but it can be done in {tower.MinimumPossibleMoves}\n");
            }
            else
            {
                WriteLine($"It took you {tower.NumberOfMoves} moves. Congrats! That's the minimum!\n");
            }
        }
    }
}
