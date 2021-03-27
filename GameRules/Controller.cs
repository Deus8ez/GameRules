using System;
using System.Collections.Generic;
using System.Text;

namespace GameRules
{
    public class Controller
    {
        private Board _board;
        private Piece _piece;
        private List<KeyValuePair<(int xFrom, int yFrom), (int xTo, int yTo)>> listOfSquares = new List<KeyValuePair<(int xFrom, int yFrom), (int xTo, int yTo)>>();


        private bool whiteMoves = true;
        internal (int x, int y) _from;
        internal (int x, int y) _to;

        internal bool endGame = false;

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

        public bool IsValidMove((int x, int y) from, (int x, int y) to)
        {
            return Math.Abs(to.y - from.y) == 1 && from.x == to.x ||
                      Math.Abs(to.x - from.x) == 1 && from.y == to.y;
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
                return (_board.GetSquareAt(to.x, to.y).piece == Piece.none || _board.GetSquareAt(to.x, to.y).piece == Piece.marker) && !IsFreeSquare(from.x, from.y - 1) && (_board.GetSquareAt(to.x, to.y).mustBeUnmarked == false);
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

        public bool IsMarked((int x, int y) to)
        {
            return _board.board[to.x, to.y].piece == Piece.marker;
        }

        public bool IsValidLongSkip((int x, int y) jumpedFromSquare, (int x, int y) from, (int x, int y) to)
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
                if (!IsFound && IsMarked(northSquare))
                {
                    IsFound = IsValidLongSkip(from, northSquare, to);
                }
                else if (!IsFound && IsMarked(eastSquare))
                {
                    IsFound = IsValidLongSkip(from, eastSquare, to);
                }
            }
            else if (directionCoords.x < 0 && directionCoords.y > 0)
            {
                //north-west;
                if (!IsFound && IsMarked(northSquare))
                {
                    IsFound = IsValidLongSkip(from, northSquare, to);
                }
                else if (!IsFound && IsMarked(westSquare))
                {
                    IsFound = IsValidLongSkip(from, westSquare, to);
                }
            }
            else if (directionCoords.x < 0 && directionCoords.y < 0)
            {
                //south-west;
                if (!IsFound && IsMarked(southSquare))
                {
                    IsFound = IsValidLongSkip(from, southSquare, to);
                }
                else if (!IsFound && IsMarked(westSquare))
                {
                    IsFound = IsValidLongSkip(from, westSquare, to);
                }

            }
            else if (directionCoords.x > 0 && directionCoords.y < 0)
            {
                //south-east
                if (!IsFound && IsMarked(eastSquare))
                {
                    IsFound = IsValidLongSkip(from, eastSquare, to);
                }
                else if (!IsFound && IsMarked(southSquare))
                {
                    IsFound = IsValidLongSkip(from, southSquare, to);
                }
            }
            else if (directionCoords.x == 0 && directionCoords.y > 0)
            {
                //north
                if (!IsFound && IsMarked(northSquare))
                {
                    IsFound = IsValidLongSkip(from, northSquare, to);
                }
                if (!IsFound && IsMarked(westSquare))
                {
                    IsFound = IsValidLongSkip(from, westSquare, to);
                }
                if (!IsFound && IsMarked(eastSquare))
                {
                    IsFound = IsValidLongSkip(from, eastSquare, to);
                }
            }
            else if (directionCoords.x == 0 && directionCoords.y < 0)
            {
                //south
                if (!IsFound && IsMarked(southSquare))
                {
                    IsFound = IsValidLongSkip(from, southSquare, to);
                }
                if (!IsFound && IsMarked(westSquare))
                {
                    IsFound = IsValidLongSkip(from, westSquare, to);
                }
                if (!IsFound && IsMarked(eastSquare))
                {
                    IsFound = IsValidLongSkip(from, eastSquare, to);
                }
            }
            else if (directionCoords.x > 0 && directionCoords.y == 0)
            {
                //east
                if (!IsFound && IsMarked(eastSquare))
                {
                    IsFound = IsValidLongSkip(from, eastSquare, to);
                }
                if (!IsFound && IsMarked(northSquare))
                {
                    IsFound = IsValidLongSkip(from, northSquare, to);
                }
                if (!IsFound && IsMarked(southSquare))
                {
                    IsFound = IsValidLongSkip(from, southSquare, to);
                }
            }
            else if (directionCoords.x < 0 && directionCoords.y == 0)
            {
                //west
                if (!IsFound && IsMarked(westSquare))
                {
                    IsFound = IsValidLongSkip(from, westSquare, to);
                }
                if (!IsFound && IsMarked(northSquare))
                {
                    IsFound = IsValidLongSkip(from, northSquare, to);
                }
                if (!IsFound && IsMarked(southSquare))
                {
                    IsFound = IsValidLongSkip(from, southSquare, to);
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

        public void IsSkipable((int x, int y) from)
        {
            if(_board.board[from.x, from.y].piece == Piece.none)
            {
                _board.board[from.x, from.y].visited = true;
                listOfSquares.Add(new KeyValuePair<(int xFrom, int yFrom), (int xTo, int yTo)>((_from.x, _from.y),(from.x, from.y)));
            }

            (int x, int y) northSquare = IsOnBoard((from.x, from.y + 2));
            (int x, int y) eastSquare = IsOnBoard((from.x + 2, from.y));
            (int x, int y) southSquare = IsOnBoard((from.x, from.y - 2));
            (int x, int y) westSquare = IsOnBoard((from.x - 2, from.y));

            if(IsFreeAndValid(from, northSquare, false) && _board.board[northSquare.x, northSquare.y].visited != true)
            {
                IsSkipable(northSquare);
            } 
            if (IsFreeAndValid(from, southSquare, false) && _board.board[southSquare.x, southSquare.y].visited != true)
            {
                IsSkipable(southSquare);
            }
            if (IsFreeAndValid(from, eastSquare, true) && _board.board[eastSquare.x, eastSquare.y].visited != true)
            {
                IsSkipable(eastSquare);
            } 
            if (IsFreeAndValid(from, westSquare, true) && _board.board[westSquare.x, westSquare.y].visited != true)
            {
                IsSkipable(westSquare);
            }
        }

        public void Move()
        {
            _board.ClearMarkers();
            _board.RemovePieceAt(_from.x, _from.y);
            _board.board[_from.x, _from.y].visited = true;
            _board.board[_from.x, _from.y].piece = Piece.marker;
            _board.SetPieceAt(_to.x, _to.y, new Square(_piece));
            listOfSquares.Clear();
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
            listOfSquares.Clear();
        }

        public void Mark()
        {
            foreach (Square sq in _board.board)
            {
                if (sq.visited == true && sq.mustBeUnmarked == false)
                {
                    sq.piece = Piece.marker;
                }
            }
        }

        public void SetTurn()
        {
            whiteMoves = !whiteMoves;
        }

        public void EstimateAndMark((int x, int y) to)
        {
            int minDistance = 0;
            int squareToZeroDistance = 0;
            (int x, int y) direction = (0, 0);
            (int x, int y) squareToMark = (-9, -9);
            (int x, int y) squareToRemove = (-9, -9);
            
            for (int i = 0; i < listOfSquares.Count; i++)
            {
                squareToZeroDistance = (listOfSquares[i].Key.xFrom - to.x) + (listOfSquares[i].Key.yFrom - to.y);
                direction = ((listOfSquares[i].Value.xTo - listOfSquares[i].Key.xFrom), (listOfSquares[i].Value.yTo - listOfSquares[i].Key.yFrom));
                if (squareToZeroDistance >= minDistance && ((direction.x == 0 && direction.y < 0) || (direction.x < 0 && direction.y == 0)))
                {
                    minDistance = squareToZeroDistance;
                    squareToMark = listOfSquares[i].Value;
                    squareToRemove = listOfSquares[i].Key;
                }
            }

            if (squareToMark.x != -9 && endGame == false)
            {
                _board.RemovePieceAt(squareToRemove.x, squareToRemove.y);
                _board.SetPieceAt(squareToMark.x, squareToMark.y, new Square(Piece.brownPiece));
                listOfSquares.Clear();
            } else
            {
                endGame = true;
                if(listOfSquares.Count - 1 > 0)
                {
                    for (int i = listOfSquares.Count - 1; i > 0; i--)
                    {
                        direction = ((listOfSquares[i].Value.xTo - listOfSquares[i].Key.xFrom), (listOfSquares[i].Value.yTo - listOfSquares[i].Key.yFrom));
                        if (direction.x > 0 && direction.y == 0)
                        {
                            squareToMark = listOfSquares[i].Value;
                            squareToRemove = listOfSquares[i].Key;
                            break;
                        }
                    }

                    for (int i = listOfSquares.Count - 1; i > 0; i--)
                    {
                        direction = ((listOfSquares[i].Value.xTo - listOfSquares[i].Key.xFrom), (listOfSquares[i].Value.yTo - listOfSquares[i].Key.yFrom));
                        if (direction.x == 0 && direction.y < 0)
                        {
                            squareToMark = listOfSquares[i].Value;
                            squareToRemove = listOfSquares[i].Key;
                            break;
                        }
                    }

                    _board.RemovePieceAt(squareToRemove.x, squareToRemove.y);
                    _board.SetPieceAt(squareToMark.x, squareToMark.y, new Square(Piece.brownPiece));
                    listOfSquares.Clear();
                } else
                {
                    robotWon = true;
                }
            }
        }

        public void MarkValidNeighbours()
        {
            (int x, int y) northSquare = IsOnBoard((_from.x, _from.y + 1));
            (int x, int y) eastSquare = IsOnBoard((_from.x + 1, _from.y));
            (int x, int y) southSquare = IsOnBoard((_from.x, _from.y - 1));
            (int x, int y) westSquare = IsOnBoard((_from.x - 1, _from.y));

            List<(int, int)> directions = new List<(int, int)>()
            {
                northSquare,
                eastSquare,
                southSquare,
                westSquare
            };

            foreach ((int x, int y) direction in directions)
            {
                ValidateAndMark(direction);
            }
        }

        public void MarkValidNeighbours(int x, int y)
        {
            (int x, int y) northSquare = IsOnBoard((x, y + 1));
            (int x, int y) eastSquare = IsOnBoard((x + 1, y));
            (int x, int y) southSquare = IsOnBoard((x, y - 1));
            (int x, int y) westSquare = IsOnBoard((x - 1, y));

            List<(int, int)> directions = new List<(int, int)>()
            {
                northSquare,
                eastSquare,
                southSquare,
                westSquare
            };

            foreach((int x, int y) direction in directions)
            {
                ValidateAndMark(direction);
            }
        }

        public void ValidateAndMark((int x, int y) square)
        {
            if (IsValidMove(_from, square) && _board.board[square.x, square.y].piece == Piece.none && _board.GetSquareAt(_from.x, _from.y).markedByRobot == false)
            {
                _board.board[square.x, square.y].visited = true;
                listOfSquares.Add(new KeyValuePair<(int xFrom, int yFrom), (int xTo, int yTo)>((_from.x, _from.y), (square.x, square.y)));
            }
        }

        public void CheckExistingSkips()
        {
            MarkValidNeighbours();
            IsSkipable(_from);
            Mark();
        }

        public void LockSquares()
        {
            for (int i = 0; i < 3; i++)
            {
                int count = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (_board.GetSquareAt(j, i).piece == Piece.brownPiece)
                    {
                        count++;
                    }
                }

                if(count == 4)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        _board.GetSquareAt(j, i).markedByRobot = true; 
                    }
                }
            }
        }
    }
}
