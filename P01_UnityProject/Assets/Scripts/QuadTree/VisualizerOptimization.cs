using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class VisualizerOptimization
    {
        private int MaxSquareSize;
        public List<Square> squares;

        public class Square
        {
            public int edgeSize;
            public int[] centerPosition;

            public Square(int size, int[] centerPosition)
            {
                edgeSize = size;
                this.centerPosition = centerPosition;
            }
        };

        public VisualizerOptimization(List<int[]> exploredNodes)
        {
            this.squares = new List<Square>();
            int maxX = 0;
            int maxY = 0;
            int minX = int.MaxValue;
            int minY = int.MaxValue;

            for (int i = 0; i < exploredNodes.Count; i++)
            {
                maxX = exploredNodes[i][0] > maxX ? exploredNodes[i][0] : maxX;
                maxY = exploredNodes[i][1] > maxY ? exploredNodes[i][1] : maxY;
                minX = exploredNodes[i][0] < minX ? exploredNodes[i][0] : minX;
                minY = exploredNodes[i][1] < minY ? exploredNodes[i][1] : minY;
            }

            MaxSquareSize = Mathf.Max((maxX - minX), (maxY - minY), 1);

            float pos = Mathf.Ceil(Mathf.Log(MaxSquareSize, 2));
            MaxSquareSize = (int)Mathf.Pow(2, pos);

            Optimize(exploredNodes);
        }

        private bool CheckIfSquareFits(int i, int squareSize, List<int[]> positionList)
        {
            if (squareSize > 0)
            {
                int[] topRight = new int[2] { positionList[i][0] + squareSize, positionList[i][1] + squareSize };
                int[] topLeft = new int[2] { positionList[i][0], positionList[i][1] + squareSize };
                int[] bottomRight = new int[2] { positionList[i][0] + squareSize, positionList[i][1] };
                int[] bottomLeft = positionList[i];

                return positionList.Exists(x => x[0] == topRight[0] && x[1] == topRight[1]) &&
                    positionList.Exists(x => x[0] == topLeft[01] && x[1] == topLeft[1]) &&
                    positionList.Exists(x => x[0] == bottomRight[0] && x[1] == bottomRight[1]) &&
                    positionList.Exists(x => x[0] == bottomLeft[0] && x[1] == bottomLeft[1]);
            }

            return true;

        }

        private void RemoveFromList(int index, int squareSize, ref List<int[]> positionList)
        {
            if (squareSize > 0)
            {
                int[] originalPos = positionList[index];
                for (int i = 0; i < squareSize; i++)
                {
                    for (int j = 0; j < squareSize; j++)
                    {
                        int[] candidate = new int[2] { originalPos[0] + i, originalPos[1] + j };
                        int foundIndex = positionList.FindIndex(0, x => x[0] == candidate[0] && x[1] == candidate[1]);
                        positionList.RemoveAt(foundIndex);
                    }
                }
            }
            else positionList.RemoveAt(index);
        }

        public void Optimize(List<int[]> positionList)
        {

            int squareSize = MaxSquareSize * 2;
            int counter = 0;
            while (positionList.Count > 0 && squareSize != 0)
            {

                squareSize = squareSize / 2;
                for (int i = 0; i < positionList.Count; i++)
                {
                    if (CheckIfSquareFits(i, squareSize, positionList))
                    {
                        int[] centerPos = new int[2] { positionList[i][0] + (squareSize+1) / 2, positionList[i][1] + (squareSize+1) / 2 };
                        Square newSquare = new Square(squareSize + 1, centerPos);
                        squares.Add(newSquare);
                        RemoveFromList(i, squareSize, ref positionList);
                    }
                }
                counter++;
            }
        }
    }
}