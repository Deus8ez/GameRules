﻿using System;
using System.Text;
using GameRules;

namespace GameDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Controller controller = new Controller(board);
            while (true)
            {
                Print(ToAscii(board));
                string move = Console.ReadLine();
                if (move == "")
                {
                    break;
                } else if (move == "s")
                {
                    controller.SetTurn();
                    continue;
                } else if(move == "c")
                {
                    board.ClearMarkers();
                    continue;
                } else if(move == "r")
                {
                    board.Init();
                    board.ClearMarkers();
                }

                controller.SetCmd(move);

                board.ClearMarkers();

                if (!controller.IsPiece())
                {
                    Console.WriteLine("no existing piece");
                } 
                else if (!controller.IsFreeSquare())
                {
                    Console.WriteLine("invalid square");
                } 
                else if (!controller.IsValidMove())
                {
                    if(!controller.IsLongSkip(controller._from, controller._to))
                    {
                        Console.WriteLine("invalid jump");
                    } else
                    {
                        controller.Jump();
                        //controller.SetTurn();
                        continue;
                    }
                    Console.WriteLine("invalid move");
                }
                else
                {
                    controller.Move();
                    //controller.SetTurn();
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
                    sb.Append(board.GetPieceAt(j, i) + " ");
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
