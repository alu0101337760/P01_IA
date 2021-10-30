using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IA_sim
{
    public class Astar
    {
        //Initial Information
        private int maxX, maxY;
        private int[] initialPosition;
        private int[] target;
        List<int[]> forbiddenPositions;

        //Lists for A*
        List<Node> openList;
        HashSet<Node> closedSet;
        public int[][] operations = { new int[2] { 1, 0 }, new int[2] { -1, 0 }, new int[2] { 0, 1 }, new int[2] { 0, -1 },
                                      new int[2] { 1, 1 }, new int[2] { 1, -1 }, new int[2] { -1, 1 }, new int[2] { -1, -1 }};


        //Nodes
        Node initialNode;
        Node finalNode;

        //Diagnostics
        public Stopwatch stopwatch;
        public int iterations;

        //Node comparation implementation for the hashset
        private class NodeComparer : IEqualityComparer<Node>
        {
            public bool Equals(Node x, Node y)
            {
                return (x.x == y.x && x.y == y.y);
            }

            public int GetHashCode(Node obj)
            {
                int hCode = obj.x ^ obj.y;
                return hCode.GetHashCode();
            }
        }

        //Enum with the heuristic functions
        public enum HeuristicMode
        {
            Manhattan,
            Euclid
        }

        //Class node
        private class Node
        {
            public Node parent;
            public int x;
            public int y;
            public float F, G, H;

            public Node(Node parent, int x, int y, float g, float h)
            {
                this.parent = parent;
                this.x = x;
                this.y = y;
                G = g;
                H = h;
                F = G + H;
            }
        }


        public Astar(int x, int y, int[] initial, int[] last, List<int[]> forbiddenPositions)
        {
            this.maxX = x;
            this.maxY = y;
            initialPosition = initial;
            target = last;
            this.forbiddenPositions = forbiddenPositions;
            stopwatch = new Stopwatch();
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
            //In case the candidate node is already in the open list
            //and the node in the open list has a greater G value, that means
            //that the current route we have taken with the candidate node is 
            //more efficent than the one that we took before.

            bool candidateIsBetter = true;
            for (int i = 0; i < openList.Count; i++)
            {
                if (candidate.x == openList[i].x && candidate.y == openList[i].y)
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

        private bool CheckIfPositionIsOutOfBounds(Node candidate)
        {
            return
                candidate.x < 0 ||
                candidate.x > maxX ||
                candidate.y < 0 ||
                candidate.y > maxY;
        }

        private bool CheckIfPostionIsForbidden(Node candidate)
        {
            return forbiddenPositions.Exists(x => x[0] == candidate.x && x[1] == candidate.y);
        }

        private bool CheckIfPositionIsValid(Node candidate, HashSet<Node> closedList)
        {
            return !closedList.Contains(candidate) &&
            !CheckIfPositionIsOutOfBounds(candidate) &&
            !CheckIfPostionIsForbidden(candidate);
        }

        private float CalculateHeuristic(int x, int y, HeuristicMode mode)
        {
            switch (mode)
            {
                //manhattan
                case HeuristicMode.Manhattan:
                    return Math.Abs(x - target[0]) + Math.Abs(y - target[1]);

                //euclid
                case HeuristicMode.Euclid:
                    return (float)Math.Sqrt(Math.Pow(x - target[0], 2) + Math.Pow(y - target[1], 2));

                //manhattan by default
                default:
                    return Math.Abs(x - target[0]) + Math.Abs(y - target[1]);

            }
        }

        private float CalculateGValue(Node parent, int operationIndex)
        {
            return parent.G + 1;
        }

        public bool simulate(HeuristicMode Hmode, bool diagonal)
        {
            
            stopwatch.Start();
            NodeComparer nc = new NodeComparer();
            openList = new List<Node>();
            closedSet = new HashSet<Node>(nc);
            Node currentNode;

            openList.Add(new Node(null, initialPosition[0], initialPosition[1], 0, CalculateHeuristic(initialPosition[0], initialPosition[1], Hmode)));
            initialNode = openList[0];

            iterations = 0;

            stopwatch.Start();
            while (openList.Count != 0)
            {
                iterations++;
                //we add the node with the least F value to the closed node list
                currentNode = openList[0];
                closedSet.Add(currentNode);
                openList.RemoveAt(0);

                if (currentNode.H == 0)
                {
                    ///// FOUND A SOLUTION /////
                    finalNode = currentNode;
                    stopwatch.Stop();
                    return true;
                }

                int childX;
                int childY;
                //Generate children of current node
                for (int i = 0; i < (diagonal ? operations.Length : 4); i++)
                {
                    childX = currentNode.x + operations[i][0];
                    childY = currentNode.y + operations[i][1];
                    Node childNode = new Node(currentNode, childX, childY, CalculateGValue(currentNode, i), CalculateHeuristic(childX, childY, Hmode));

                    //If the position is invalid, continue
                    if (CheckIfPositionIsValid(childNode, closedSet))
                    {
                        CheckAndSolveIfAlreadyACandidate(childNode);
                    }
                }
            }
            stopwatch.Stop();

            return false;
        }

        public double GetElapsedMiliseconds()
        {
            return stopwatch.Elapsed.TotalMilliseconds;
        }


        public List<int[]> GetExploredPositions()
        {
            List<int[]> output = new List<int[]>();
            Node[] nodeArr = new Node[closedSet.Count];
            closedSet.CopyTo(nodeArr);
            for (int i = 0; i < closedSet.Count; i++)
            {
                output.Add(new int[2] { nodeArr[i].x, nodeArr[i].y });
            }
            return output;
        }

        public List<int[]> BacktrackSolution()
        {
            List<int[]> output = new List<int[]>();

            if (finalNode != null)
            {
                Node currentNode = finalNode;
                while (currentNode.parent != null)
                {
                    output.Add(new int[2] { currentNode.x, currentNode.y });
                    currentNode = currentNode.parent;
                }
            }
            return output;
        }

    }
}
