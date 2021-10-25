using System.Collections.Generic;
using System;
namespace IA_sim
{
    public class Astar
    {
        int maxX, maxY;
        List<Node> openList;
        List<Node> closedList;
        int[] initialPosition;
        int[] target;
        List<int[]> forbiddenPositions;

        private class Node
        {
            public Node(Node parent, int[] pos, float g, float h)
            {
                this.parent = parent;
                position = pos;
                G = g;
                H = h;
                F = G + H;
            }
            public Node parent;
            public int[] position;
            public float F, G, H;
        }

        public Astar(int x, int y, int[] initial, int[] last, List<int[]> forbiddenPositions)
        {
            this.maxX = x;
            this.maxY = y;
            initialPosition = initial;
            target = last;
            this.forbiddenPositions = forbiddenPositions;
        }

        int[][] operations = { new int[2] { 1, 0 }, new int[2] { -1, 0 }, new int[2] { 0, 1 }, new int[2] { 0, -1 }, };


        private void InsertInList()
        {

        }

        private void OrderedInsert(List<Node> nodeList, Node node)
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].F >= node.F)
                {
                    nodeList.Insert(i, node);
                    return;
                }
            }
            nodeList.Insert(nodeList.Count, node);
        }


        private void CheckAndSolveIfAlreadyACandidate(Node candidate)
        {
            bool candidateIsBetter = true;
            for (int i = 0; i < openList.Count; i++)
            {
                if (candidate.position == openList[i].position)
                {
                    candidateIsBetter = false;
                    if (candidate.G < openList[i].G)
                    {
                        openList.RemoveAt(i);
                        candidateIsBetter = true;
                    }
                }
            }
            if (candidateIsBetter)
            {
                OrderedInsert(openList, candidate);
            }
        }

        private int CheckIfPositionIsInList(List<Node> nodeList, int[] candidatePos)
        {
            for (int i = nodeList.Count - 1; i >= 0; i--)
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
                candidatePos[0] > maxX ||
                candidatePos[1] < 0 ||
                candidatePos[1] > maxY;
        }

        private bool CheckIfPostionIsForbidden(int[] candidatePos)
        {

            for (int i = 0; i < forbiddenPositions.Count; i++)
            {
                if (forbiddenPositions[i][0] == candidatePos[0] && forbiddenPositions[i][1] == candidatePos[1])
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckIfPositionIsValid(int[] candidatePos, List<Node> closedList)
        {
            if (CheckIfPositionIsInList(closedList, candidatePos) >= 0)
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


        public bool simulate()
        {
            openList = new List<Node>();
            closedList = new List<Node>();
            Node currentNode;

            openList.Add(new Node(null, initialPosition, 0, CalculateManhattan(initialPosition)));

            while (openList.Count != 0 && closedList.Count < 1000)
            {
                //we add the node with the least F value to the closed node list
                currentNode = openList[0];
                closedList.Add(currentNode);
                openList.RemoveAt(0);

                if (currentNode.H == 0)
                {
                    ///// FOUND A SOLUTION /////

                    PathfindingManager.instance.DrawExploredNodes(GetExploredPositions());

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

                    Node ChildNode = new Node(currentNode, childPosition, childG, CalculateManhattan(childPosition));
                    CheckAndSolveIfAlreadyACandidate(ChildNode);
                }
            }

            return false;
        }

        public List<int[]> GetExploredPositions()
        {
            List<int[]> output = new List<int[]>();
            for (int i = 0; i < closedList.Count; i++)
            {
                output.Add(closedList[i].position);
            }
            return output;
        }

        public List<int[]> BacktrackSolution()
        {
            List<int[]> output = new List<int[]>();
            Node lastNode = closedList[closedList.Count - 1];
            Node currentNode = lastNode;
            while (currentNode.parent != null)
            {
                output.Add(currentNode.position);
                currentNode = currentNode.parent;
            }
            return output;
        }

    }
}
