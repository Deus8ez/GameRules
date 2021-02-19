using System;
using System.Collections.Generic;
using System.Text;

namespace GameRules
{
    public class Controller
    {
        private Board _board;
        private Piece _piece;
        private Piece _marker;

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
            _marker = Piece.marker;
        }

        public bool IsPiece()
        {          
            return _board.pieces[_from.x, _from.y] != 0 && _board.pieces[_from.x, _from.y] == _piece;
        }

        public bool IsFreeSquare()
        {
            if(_to.x < 8 &&  _to.x > -1  && _to.y < 8 && _to.y > -1)
            {
                return _board.pieces[_to.x, _to.y] == 0;
            } 
            
            return false;
        }

        public bool IsFreeSquare(int x, int y)
        {
            if (x < 8 && x > -1 && y < 8 && y > -1)
            {
                return _board.pieces[x, y] == 0;
            } 
            
            return false;
        }

        public bool IsValidMove()
        {
            return Math.Abs(_to.y - _from.y) == 1 && _from.x == _to.x ||
                      Math.Abs(_to.x - _from.x) == 1 && _from.y == _to.y; 
        }

        public bool IsSkip((int x, int y) from, (int x, int y) to)
        {
            return (!IsFreeSquare((to.x), Math.Abs((from.y + to.y) / 2)) || !IsFreeSquare(Math.Abs((from.x + to.x) / 2), (to.y)))
                && _board.GetPieceAt(to.x, to.y) != Piece.whitePiece.GetHashCode() && _board.GetPieceAt(to.x, to.y) != Piece.brownPiece.GetHashCode();
        }

        public (int x, int y) IsOnBoard((int x, int y) coords)
        {
            if (coords.x < 8 && coords.x > -1 && coords.y < 8 && coords.y > -1)
            {
                return coords;
            };

            return (-9, -9);
        }

        public bool IsFreeAndValid((int x, int y) from, (int x, int y) to)
        {
            return to != (-9, -9) && IsSkip(from, to) && _board.GetPieceAt(to.x, to.y) != Piece.marker.GetHashCode();
        }

        public bool IsLongSkip((int x, int y) from, (int x, int y) to)
        {
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

            _board.SetPieceAt(from.x, from.y, _marker);

            if (directionCoords.x > 0 && directionCoords.y > 0)
            {
                //north-east;
                if (!IsFound && IsFreeAndValid(from, northSquare))
                {
                    IsFound = IsLongSkip(northSquare, to);
                }
                else if (!IsFound && IsFreeAndValid(from, eastSquare))
                {
                    IsFound = IsLongSkip(eastSquare, to);
                }
            }
            else if (directionCoords.x < 0 && directionCoords.y > 0)
            {
                //north-west;
                if (!IsFound && IsFreeAndValid(from, northSquare))
                {
                    IsFound = IsLongSkip(northSquare, to);
                }
                else if (!IsFound && IsFreeAndValid(from, westSquare))
                {
                    IsFound = IsLongSkip(westSquare, to);
                }

            }
            else if (directionCoords.x < 0 && directionCoords.y < 0)
            {
                //south-west;
                if (!IsFound && IsFreeAndValid(from, southSquare))
                {
                    IsFound = IsLongSkip(southSquare, to);
                }
                else if (!IsFound && IsFreeAndValid(from, westSquare))
                {
                    IsFound = IsLongSkip(westSquare, to);
                }

            }
            else if (directionCoords.x > 0 && directionCoords.y < 0)
            {
                //south-east
                if (!IsFound && IsFreeAndValid(from, eastSquare))
                {
                    IsFound = IsLongSkip(eastSquare, to);
                }
                else if (!IsFound && IsFreeAndValid(from, southSquare))
                {
                    IsFound = IsLongSkip(southSquare, to);
                }
            } 
            else if (directionCoords.x == 0 && directionCoords.y > 0)
            {
                //north
                if (!IsFound && IsFreeAndValid(from, northSquare))
                {
                    IsFound = IsLongSkip(northSquare, to);
                }
                if (!IsFound && westSquare != (-9, -9) && IsSkip(from, westSquare))
                {
                    IsFound = IsLongSkip(westSquare, to);
                }
                if (!IsFound && eastSquare != (-9, -9) && IsSkip(from, eastSquare))
                {
                    IsFound = IsLongSkip(eastSquare, to);
                }
            }
            else if (directionCoords.x == 0 && directionCoords.y < 0)
            {
                //south
                if (!IsFound && IsFreeAndValid(from, southSquare))
                {
                    IsFound = IsLongSkip(southSquare, to);
                }
                if (!IsFound && westSquare != (-9, -9) && IsSkip(from, westSquare))
                {
                    IsFound = IsLongSkip(westSquare, to);
                }
                if (!IsFound && eastSquare != (-9, -9) && IsSkip(from, eastSquare))
                {
                    IsFound = IsLongSkip(eastSquare, to);
                }
            }
            else if (directionCoords.x > 0 && directionCoords.y == 0)
            {
                //east
                if (!IsFound && IsFreeAndValid(from, eastSquare))
                {
                    IsFound = IsLongSkip(eastSquare, to);
                }
                if (!IsFound && northSquare != (-9, -9) && IsSkip(from, northSquare))
                {
                    IsFound = IsLongSkip(northSquare, to);
                }
                if (!IsFound && southSquare != (-9, -9) && IsSkip(from, southSquare))
                {
                    IsFound = IsLongSkip(southSquare, to);
                }
            }
            else if (directionCoords.x < 0 && directionCoords.y == 0)
            {
                //west
                if (!IsFound && IsFreeAndValid(from, westSquare))
                {
                    IsFound = IsLongSkip(westSquare, to);
                }
                if (!IsFound && northSquare != (-9, -9) && IsSkip(from, northSquare))
                {
                    _board.RemovePieceAt(from.x, from.y);
                    IsFound = IsLongSkip(northSquare, to);
                }
                if (!IsFound && southSquare != (-9, -9) && IsSkip(from, southSquare))
                {
                    _board.RemovePieceAt(from.x, from.y);
                    IsFound = IsLongSkip(southSquare, to);
                }
            }

            if (!IsFound && _board.GetPieceAt(from.x, from.y) == _marker.GetHashCode())
            {

                _board.RemovePieceAt(from.x, from.y);

                if (from.x == _from.x && from.y == _from.y)
                {
                    _board.RemovePieceAt(from.x, from.y);
                    _board.SetPieceAt(from.x, from.y, _piece);
                }
            }

            return IsFound;
        }

        public void Move()
        {
            _board.RemovePieceAt(_from.x, _from.y);
            _board.SetPieceAt(_from.x, _from.y, _marker);
            _board.SetPieceAt(_to.x, _to.y, _piece);
        }

        public void Jump()
        {
            _board.SetPieceAt(_to.x, _to.y, _piece);
        }

        public void SetTurn()
        {
            whiteMoves = !whiteMoves;
        }
    }
}
