using System;
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
            Robot robot = new Robot(controller, board);
            Starter starter = new Starter(controller, board, robot);
            starter.RunGame(true);    
        }
    }
}
