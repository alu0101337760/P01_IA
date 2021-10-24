using System.Collections.Generic;
using System;
namespace IA_sim
{
    public class Pathfinder
    {
        int MaxX, MaxZ;
        List<Node> openList;
        List<Node> closedList;
        int[] initialPosition;
        int[] target;
        List<int[]> forbiddenPositions;

        private class Node
        {
            public Node(int[] pos, int[] lastPos, float g, float h)
            {
                position = pos;
                lastPosition = lastPos;
                G = g;
                H = h;
                F = G + H;
            }

            public int[] position;
            public int[] lastPosition;
            public float F, G, H;
        }

        public Pathfinder()
        {

        }

        int[][] operations = { new int[2] { 1, 0 }, new int[2] { -1, 0 }, new int[2] { 0, 1 }, new int[2] { 0, -1 }, };

        private void OrderedInsert(List<Node> nodeList, Node node)
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (node.F < nodeList[i].F)
                {
                    nodeList.Insert(i, node);
                }
            }
        }

        private int CheckIfPositionIsInList(List<Node> nodeList, int[] candidatePos)
        {
            for (int i = nodeList.Count - 1; i >= 0; i++)
            {
                if (nodeList[i].position == candidatePos)
                {
                    return i;
                }
            }
            return -1;
        }

        private bool CheckIfPositionIsOutOfBounds(int[] candidatePos)
        {
            return
                candidatePos[0] < 0 ||
                candidatePos[0] > MaxX ||
                candidatePos[1] < 0 ||
                candidatePos[1] > MaxZ;
        }

        private bool CheckIfPostionIsForbidden(int[] candidatePos)
        {
            for (int i = 0; i < forbiddenPositions.Count; i++)
            {
                if (forbiddenPositions[i] == candidatePos)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfPositionIsValid(int[] candidatePos, List<Node> closedList)
        {
            if (CheckIfPositionIsInList(closedList, candidatePos) < 0)
                return false;

            if (CheckIfPositionIsOutOfBounds(candidatePos))
                return false;

            if (CheckIfPostionIsForbidden(candidatePos))
                return false;

            return true;
        }

        private int CalculateManhattan(int[] currentPos)
        {
            return Math.Abs(currentPos[0] - target[0]) + Math.Abs(currentPos[1] - target[1]);
        }


        bool simulate()
        {
            openList = new List<Node>();
            closedList = new List<Node>();
            Node currentNode;

            openList.Add(new Node(initialPosition, new int[] { 0, 0 }, 0, CalculateManhattan(initialPosition)));

            while (openList.Count != 0)
            {
                //we add the node with the least F value to the closed node list
                currentNode = openList[0];
                closedList.Add(currentNode);
                openList.RemoveAt(0);

                if (currentNode.H == 0)
                {
                    ///// FOUND A SOLUTION /////
                    return true;
                }

                //Generate children of current node
                for (int i = 0; i < operations.Length; i++)
                {
                    int[] childPosition = new int[2] { currentNode.position[0] + operations[i][0], currentNode.position[1] + operations[i][1] };
                    float childG = currentNode.G + 1;

                    //If the position is invalid, continue
                    if (!CheckIfPositionIsValid(childPosition, closedList))
                    {
                        continue;
                    }
                    //if the position is already in the open list, this path might
                    //be more efficent, so we check the G value.

                    int index = CheckIfPositionIsInList(openList, childPosition);
                    if (index >= 0)
                    {
                        if (openList[index].G > childG)
                        {
                            openList.RemoveAt(index);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    OrderedInsert(openList, new Node(childPosition, currentNode.position, childG, CalculateManhattan(childPosition)));

                }
            }

            return false;
        }



    }
}
