using System;
using System.Collections.Generic;
using System.Text;

namespace GameRules
{
    public class Robot
    {
        private Controller _controller;
        private Board _board;
        private List<Square> pieces = new List<Square>();

        public Robot(Controller newController, Board newBoard)
        {
            _controller = newController;
            _board = newBoard;
            SetPieces();
        }
        
        public void SetPieces()
        {
            for (int i = 7; i > 3; i--)
            {
                for (int j = 7; j > 4; j--)
                {
                    pieces.Add(_board.board[i, j]);
                }
            }
        }

        public void MakeMove()
        {
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 7; j >= 0; j--)
                {
                    if (_controller.endGame == true)
                    {
                        _controller.LockSquares();
                    }

                    if (_board.board[i, j].piece == Piece.brownPiece && _board.GetSquareAt(i, j).markedByRobot == false)
                    {
                        _controller._from = (i, j);
                        _controller.MarkValidNeighbours(i, j);
                        _controller.IsSkipable((i, j));
                    }
                }
            }
        }

        public void Mark()
        {
            _controller.EstimateAndMark((0, 0));
        }
    }
}
