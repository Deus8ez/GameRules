using System;
using System.Collections.Generic;
using System.Text;

namespace GameRules
{
    public class Starter
    {
        private Controller _controller;
        private Board _board;
        private Robot _robot;

        public Starter(Controller controller, Board board, Robot robot)
        {
            _controller = controller;
            _board = board;
            _robot = robot;
        }

        public void RunGame(bool againstAI)
        {

            if (againstAI)
            {
                while (true)
                {
                    _board.ClearMarkers();
                    Print(ToAscii(_board));
                    string move = Console.ReadLine();
                    if (move == "")
                    {
                        break;
                    }
                    else if (move == "s")
                    {
                        _controller.SetTurn();
                        continue;
                    }
                    else if (move == "c")
                    {
                        _board.ClearMarkers();
                        continue;
                    }
                    else if (move == "r")
                    {
                        _board.Init();
                        _board.ClearMarkers();
                    }
                    else if (move == "2")
                    {
                        _robot.MakeMove();
                        _robot.Mark();

                        continue;
                    }

                    _controller.SetCmd(move);

                    _controller.CheckExistingSkips();

                    Print(ToAscii(_board));

                    move = Console.ReadLine();

                    if (move == "1")
                    {
                        if (!_controller.IsPiece())
                        {
                            Console.WriteLine("no existing piece");
                        }
                        else if (!_controller.IsFreeSquare())
                        {
                            Console.WriteLine("invalid square");
                        }
                        else if (!_controller.IsValidMove())
                        {
                            if (!_controller.IsValidLongSkip(_controller._from, _controller._from, _controller._to))
                            {
                                Console.WriteLine("invalid jump");
                            }
                            else
                            {
                                _controller.Jump();
                                _robot.MakeMove();
                                _robot.Mark();
                                continue;
                            }
                            Console.WriteLine("invalid move");
                        }
                        else
                        {
                            _controller.Move();
                            _robot.MakeMove();
                            _robot.Mark();
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            else
            {
                while (true)
                {
                    _board.ClearMarkers();
                    Print(ToAscii(_board));
                    string move = Console.ReadLine();
                    if (move == "")
                    {
                        break;
                    }
                    else if (move == "s")
                    {
                        _controller.SetTurn();
                        continue;
                    }
                    else if (move == "c")
                    {
                        _board.ClearMarkers();
                        continue;
                    }
                    else if (move == "r")
                    {
                        _board.Init();
                        _board.ClearMarkers();
                    }

                    _controller.SetCmd(move);

                    _controller.CheckExistingSkips();
                    Print(ToAscii(_board));

                    move = Console.ReadLine();

                    if (move == "1")
                    {
                        if (!_controller.IsPiece())
                        {
                            Console.WriteLine("no existing piece");
                        }
                        else if (!_controller.IsFreeSquare())
                        {
                            Console.WriteLine("invalid square");
                        }
                        else if (!_controller.IsValidMove())
                        {
                            if (!_controller.IsValidLongSkip(_controller._from, _controller._from, _controller._to))
                            {
                                Console.WriteLine("invalid jump");
                            }
                            else
                            {
                                _controller.Jump();
                                _controller.SetTurn();
                                continue;
                            }
                            Console.WriteLine("invalid move");
                        }
                        else
                        {
                            _controller.Move();
                            _controller.SetTurn();
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        static string ToAscii(Board board)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  +-----------------+");
            for (int i = 7; i >= 0; i--)
            {
                sb.Append(i + 1);
                sb.Append(" | ");
                for (int j = 0; j < 8; j++)
                {
                    sb.Append((char)board.GetSquareAt(j, i).piece + " ");
                }
                sb.AppendLine("|");
            }
            sb.AppendLine("  +-----------------+");
            sb.AppendLine("  + a b c d e f g h +");

            return sb.ToString();

        }

        static void Print(string text)
        {
            ConsoleColor old = Console.ForegroundColor;
            foreach (char x in text)
            {
                if (x >= 'a' && x <= 'z')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (x >= 'A' && x <= 'Z')
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                Console.Write(x);
            }
            Console.ForegroundColor = old;
        }


    }
}
