using System;
using System.Collections.Generic;
using System.Text;

namespace GameRules
{
    public class Board
    {
        public Square[,] board;
        public bool cleared = false;
        public bool whiteMoves;

        public Board()
        {
            board = new Square[8, 8];
            Init();
        }

        public void Init()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    SetPieceAt(j, i, new Square(Piece.none));
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    SetPieceAt(j, i, new Square(Piece.whitePiece));
                    SetPieceAt(7 - j, 7 - i, new Square(Piece.brownPiece));
                }
            }

            whiteMoves = true;
        }

        public void SetPieceAt(int x, int y, Square square)
        {
            board[x, y] = square;
        }

        public void RemovePieceAt(int x, int y)
        {
            board[x, y].piece = Piece.none;
        }

        public Square GetSquareAt(int x, int y)
        {
            return board[x, y];
         }

        public void ClearMarkers()
        {
            foreach (Square sq in board)
            {
                if(sq.piece == Piece.marker || sq.piece == Piece.none)
                {
                    sq.piece = Piece.none;
                    sq.visited = false;
                    sq.mustBeUnmarked = false;
                }
            }
        }
    }
}
