using System;
using System.Collections.Generic;
using System.Text;

namespace GameRules
{
    public class Board
    {
        public Piece[,] pieces;
        public bool cleared = false;
        bool whiteMoves;
        public Board()
        {
            pieces = new Piece[8, 8];
            Init();
        }

        public void Init()
        {
            //for (int i = 0; i < 3; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        SetPieceAt(j, i, Piece.whitePiece);
            //        SetPieceAt(7 - j, 7 - i, Piece.brownPiece);
            //    }
            //}

            SetPieceAt(0, 0, Piece.whitePiece);

            for (int i = 0; i < 7; i++)
            {
                if (i % 2 == 0)
                {
                    SetPieceAt(i, 1, Piece.whitePiece);
                    SetPieceAt(i, 3, Piece.whitePiece);
                    SetPieceAt(i, 5, Piece.whitePiece);
                    SetPieceAt(i, 7, Piece.whitePiece);
                }
                else
                {
                    SetPieceAt(i, 2, Piece.whitePiece);
                    SetPieceAt(i, 4, Piece.whitePiece);
                    SetPieceAt(i, 6, Piece.whitePiece);

                }
            }

            whiteMoves = true;
        }

        public void SetPieceAt(int x, int y, Piece piece)
        {
            pieces[x, y] = piece;
        }

        public void RemovePieceAt(int x, int y)
        {
            pieces[x, y] = 0;
        }

        public Char GetPieceAt(int x, int y)
        {
            if (pieces[x, y] == 0)
                return (char)Piece.none;
            return (char)pieces[x, y];
         }

        public void ClearMarkers()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (pieces[i,j] == Piece.marker)
                    {
                        RemovePieceAt(i, j);
                    }
                }
            }

        }
    }
}
