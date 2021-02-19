using System;
using System.Collections.Generic;
using System.Text;

namespace GameRules
{
    class Draft
    {

        //public bool IsValidMove(int from_x, int from_y, int to_x, int to_y)
        //{
        //    return Math.Abs(to_y - from_y) == 1 && from_x == to_x ||
        //              Math.Abs(to_x - from_x) == 1 && from_y == to_y ||
        //              IsSkip(from_x, from_y, to_x, to_y);
        //}
        //c2e4
        //c2c4 c4e4

        //a1a3 a3c3
        //a1c1 c1c3
        //a1c3

        //22 54
        //public bool IsValidLongMove()
        //{
        //    (int x, int y) current = _from;
        //    (int x, int y) dest = _to;
        //    List<ValueTuple<int, int>> neighbours = new List<ValueTuple<int, int>>();
        //(int x, int y) north = (_from.x, _from.y + 2);
        //(int x, int y) east = (_from.x + 2, _from.y);
        //(int x, int y) south = (_from.x, _from.y - 2);
        //(int x, int y) west = (_from.x - 2, _from.y);
        //    neighbours.Add(north);
        //    neighbours.Add(east);
        //    neighbours.Add(south);
        //    neighbours.Add(west);
        //    int leastDist = Int32.MaxValue;
        //    for (int j = 0; j < 4; j++)
        //    {
        //        if ((dest.x - neighbours[j].Item1) + (dest.y - neighbours[j].Item2) < leastDist)
        //        {
        //            leastDist = (dest.x - neighbours[j].Item1) + (dest.y - neighbours[j].Item2);
        //        }

        //        if (IsFreeSquare(neighbours[j]))
        //        {

        //        };
        //    }
        //    return
        //}

        //a1a3
        //public bool IsSkip(int from_x, int from_y, int to_x, int to_y)
        //{
        //    return (!IsFreeSquare((to_x), Math.Abs((from_y + to_y) / 2)) || !IsFreeSquare(Math.Abs((from_x + to_x) / 2), (to_y)))
        //        && IsFreeSquare();
        //}
        //wa3a4
        //wa3b4
    }
}
