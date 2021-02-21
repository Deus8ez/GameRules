using System;
using System.Collections.Generic;
using System.Text;

namespace GameRules
{
    public class Square
    {
        public Piece piece;
        public bool visited = false;
        public bool mustBeUnmarked = false;

        public Square(Piece newPiece)
        {
            piece = newPiece;
        }
    }
}
