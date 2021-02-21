using System;
using System.Collections.Generic;
using System.Text;

namespace GameRules
{
    public class Controller
    {
        private Board _board;
        private Piece _piece;

        private bool whiteMoves = true;
        public (int x, int y) _from;
        public (int x, int y) _to; 

        public Controller(Board board)
        {
            _board = board;
        }

        public void SetCmd(string cmd)
        {
            _from = ((cmd[0] - 'a'), (cmd[1] - '1'));
            _to = ((cmd[2] - 'a'), (cmd[3] - '1'));
            _piece = whiteMoves ? Piece.whitePiece : Piece.brownPiece;
        }

        public bool IsPiece()
        {          
            return _board.board[_from.x, _from.y].piece == _piece;
        }

        public bool IsFreeSquare()
        {
            if(_to.x < 8 &&  _to.x > -1  && _to.y < 8 && _to.y > -1)
            {
                return _board.board[_to.x, _to.y].piece == Piece.none || _board.board[_to.x, _to.y].piece == Piece.marker;
            } 
            
            return false;
        }

        public bool IsFreeSquare(int x, int y)
        {
            if (x < 8 && x > -1 && y < 8 && y > -1)
            {
                return _board.board[x, y].piece == Piece.none || _board.board[x, y].piece == Piece.marker;
            }

            return false;
        }

        public bool IsValidMove()
        {
            return Math.Abs(_to.y - _from.y) == 1 && _from.x == _to.x ||
                      Math.Abs(_to.x - _from.x) == 1 && _from.y == _to.y; 
        }

        public bool IsSkip((int x, int y) from, (int x, int y) to, bool horizontally)
        {

            if (horizontally && to.x - from.x > 0)
            {
                return (_board.GetSquareAt(to.x, to.y).piece == Piece.none || _board.GetSquareAt(to.x, to.y).piece == Piece.marker) && !IsFreeSquare(from.x + 1, from.y) && (_board.GetSquareAt(to.x, to.y).mustBeUnmarked == false); 
            } 
            else if(horizontally && to.x - from.x < 0)
            {
                return (_board.GetSquareAt(to.x, to.y).piece == Piece.none || _board.GetSquareAt(to.x, to.y).piece == Piece.marker) && !IsFreeSquare(from.x - 1, from.y) && (_board.GetSquareAt(to.x, to.y).mustBeUnmarked == false);
            }
            else if (!horizontally && to.y - from.y > 0)
            {
                return (_board.GetSquareAt(to.x, to.y).piece == Piece.none || _board.GetSquareAt(to.x, to.y).piece == Piece.marker) && !IsFreeSquare(from.x, from.y + 1) && (_board.GetSquareAt(to.x, to.y).mustBeUnmarked == false);
            } 
            else if (!horizontally && to.y - from.y < 0)
            {
                return (_board.GetSquareAt(to.x, to.y).piece == Piece.none || _board.GetSquareAt(to.x, to.y).piece == Piece.marker) && !IsFreeSquare(from.x, from.y + 1) && (_board.GetSquareAt(to.x, to.y).mustBeUnmarked == false);
            }
            else
            {
                return false;
            }

        }

        public (int x, int y) IsOnBoard((int x, int y) coords)
        {
            if (coords.x < 8 && coords.x > -1 && coords.y < 8 && coords.y > -1)
            {
                return coords;
            };

            return (-9, -9);
        }

        public bool IsFreeAndValid((int x, int y) from, (int x, int y) to, bool horizontally)
        {
            return to != (-9, -9) && IsSkip(from, to, horizontally);
        }

        public bool IsLongSkip((int x, int y) jumpedFromSquare, (int x, int y) from, (int x, int y) to)
        {

            if (_board.board[from.x, from.y].visited == true)
            {
                _board.board[jumpedFromSquare.x, jumpedFromSquare.y].mustBeUnmarked = true;
            }
            else
            {
                _board.board[jumpedFromSquare.x, jumpedFromSquare.y].visited = true;
            }

            if ((from.x == to.x) && (from.y == to.y))
            {
                return true;
            }

            (int x, int y) northSquare = IsOnBoard((from.x, from.y + 2));
            (int x, int y) eastSquare = IsOnBoard((from.x + 2, from.y));
            (int x, int y) southSquare = IsOnBoard((from.x, from.y - 2));
            (int x, int y) westSquare = IsOnBoard((from.x - 2, from.y));
            
            (int x, int y) directionCoords = ((to.x - from.x), (to.y - from.y));

            if (directionCoords.x + directionCoords.y == 0 && directionCoords.x == 1 && directionCoords.y == 1) return false;

            bool IsFound = false;

            if (directionCoords.x > 0 && directionCoords.y > 0)
            {
                //north-east;
                if (!IsFound && IsFreeAndValid(from, northSquare, false))
                {
                    IsFound = IsLongSkip(from, northSquare, to);
                }
                else if (!IsFound && IsFreeAndValid(from, eastSquare, true))
                {
                    IsFound = IsLongSkip(from, eastSquare, to);
                }
            }
            else if (directionCoords.x < 0 && directionCoords.y > 0)
            {
                //north-west;
                if (!IsFound && IsFreeAndValid(from, northSquare, false))
                {
                    IsFound = IsLongSkip(from, northSquare, to);
                }
                else if (!IsFound && IsFreeAndValid(from, westSquare, true))
                {
                    IsFound = IsLongSkip(from, westSquare, to);
                }
            }
            else if (directionCoords.x < 0 && directionCoords.y < 0)
            {
                //south-west;
                if (!IsFound && IsFreeAndValid(from, southSquare, false))
                {
                    IsFound = IsLongSkip(from, southSquare, to);
                }
                else if (!IsFound && IsFreeAndValid(from, westSquare, true))
                {
                    IsFound = IsLongSkip(from, westSquare, to);
                }

            }
            else if (directionCoords.x > 0 && directionCoords.y < 0)
            {
                //south-east
                if (!IsFound && IsFreeAndValid(from, eastSquare, true))
                {
                    IsFound = IsLongSkip(from, eastSquare, to);
                }
                else if (!IsFound && IsFreeAndValid(from, southSquare, false))
                {
                    IsFound = IsLongSkip(from, southSquare, to);
                }
            } 
            else if (directionCoords.x == 0 && directionCoords.y > 0)
            {
                //north
                if (!IsFound && IsFreeAndValid(from, northSquare, false))
                {
                    IsFound = IsLongSkip(from, northSquare, to);
                }
                if (!IsFound && IsFreeAndValid(from, westSquare, true))
                {
                    IsFound = IsLongSkip(from, westSquare, to);
                }
                if (!IsFound && IsFreeAndValid(from, eastSquare, true))
                {
                    IsFound = IsLongSkip(from, eastSquare, to);
                }
            }
            else if (directionCoords.x == 0 && directionCoords.y < 0)
            {
                //south
                if (!IsFound && IsFreeAndValid(from, southSquare, false))
                {
                    IsFound = IsLongSkip(from, southSquare, to);
                }
                if (!IsFound && IsFreeAndValid(from, westSquare, true))
                {
                    IsFound = IsLongSkip(from, westSquare, to);
                }
                if (!IsFound && IsFreeAndValid(from, eastSquare, true))
                {
                    IsFound = IsLongSkip(from, eastSquare, to);
                }
            }
            else if (directionCoords.x > 0 && directionCoords.y == 0)
            {
                //east
                if (!IsFound && IsFreeAndValid(from, eastSquare, true))
                {
                    IsFound = IsLongSkip(from, eastSquare, to);
                }
                if (!IsFound && IsFreeAndValid(from, northSquare, false))
                {
                    IsFound = IsLongSkip(from, northSquare, to);
                }
                if (!IsFound && IsFreeAndValid(from, southSquare, false))
                {
                    IsFound = IsLongSkip(from, southSquare, to);
                }
            }
            else if (directionCoords.x < 0 && directionCoords.y == 0)
            {
                //west
                if (!IsFound && IsFreeAndValid(from, westSquare, true))
                {
                    IsFound = IsLongSkip(from, westSquare, to);
                }
                if (!IsFound && IsFreeAndValid(from, northSquare, false))
                {
                    IsFound = IsLongSkip(from, northSquare, to);
                }
                if (!IsFound && IsFreeAndValid(from, southSquare, false))
                {
                    IsFound = IsLongSkip(from, southSquare, to);
                }
            }

            if (!IsFound && _board.GetSquareAt(from.x, from.y).visited == true)
            {

                _board.board[from.x, from.y].piece = Piece.none;

                if (from.x == _from.x && from.y == _from.y)
                {
                    _board.RemovePieceAt(from.x, from.y);
                    _board.SetPieceAt(from.x, from.y, new Square(_piece));
                }
            }

            return IsFound;
        }

        public void Move()
        {
            _board.ClearMarkers();
            _board.RemovePieceAt(_from.x, _from.y);
            _board.board[_from.x, _from.y].visited = true;
            _board.board[_from.x, _from.y].piece = Piece.marker;
            _board.SetPieceAt(_to.x, _to.y, new Square(_piece));
        }

        public void Jump()
        {
            _board.ClearMarkers();
            _board.RemovePieceAt(_from.x, _from.y);

            foreach (Square sq in _board.board)
            {
                if(sq.visited == true && sq.mustBeUnmarked == false)
                {
                    sq.piece = Piece.marker;
                }
            }

            _board.SetPieceAt(_to.x, _to.y, new Square(_piece));
        }

        public void SetTurn()
        {
            whiteMoves = !whiteMoves;
        }
    }
}
