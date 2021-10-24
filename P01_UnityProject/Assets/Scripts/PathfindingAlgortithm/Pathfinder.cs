using System.Collections.Generic;
using System;
namespace IA_sim
{
    public class Pathfinder
    {
        private class Node
        {
            public int[] position;
            public int[] lastPosition;
            public float F, G, H;
        }

        int MaxX, MaxZ;
        List<Node> openList;
        List<Node> closedList;
        int[] target;

        bool simulate()
        {
            openList = new List<Node>();
            closedList = new List<Node>();

            while (openList.Count != 0)
            {

            }

            return false;
        }

        int CalculateManhattan(int[] currentPos)
        {
            return Math.Abs(currentPos[0] - target[0]) + Math.Abs(currentPos[1] - target[1]);
        }


    }
}
