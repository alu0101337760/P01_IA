using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace IA_sim
{
    public class PathfindingManager : MonoBehaviour
    {
        public GameObject plane;
        public List<int[]> exploredPositions;
        public static PathfindingManager instance;
        List<int[]> forbiddenPos;     

        public Slider slider;

        public bool diagonalMode = false;
        public bool diagonalsCosts2 = false;
        public Astar.HeuristicMode heuristicMode = Astar.HeuristicMode.Manhattan;

        private Astar pathfinder;
        private List<GameObject> instancedMarks;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                instancedMarks = new List<GameObject>();
            }
        }

        private void CleanGameObjectList(List<GameObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject target = list[i];
                Destroy(target);

            }
            list.Clear();
        }

        private Vector3 ToVector3(int[] pos)
        {
            return new Vector3(pos[0], 0, pos[1]);
        }
        private int[] ToIntArr(Vector3 pos)
        {
            return new int[2] { (int)pos.x, (int)pos.z };
        }


        private List<int[]> TranslateForbiddenPositions(Vector3[] forbiddenPositions)
        {
            List<int[]> output = new List<int[]>();
            for (int i = 0; i < forbiddenPositions.Length; i++)
            {
                output.Add(ToIntArr(forbiddenPositions[i]));
            }
            return output;
        }


        private bool CheckConditions()
        {
            return
                PlacementManager.instance.maxX != 0 &&
                PlacementManager.instance.maxZ != 0 &&
                PlacementManager.instance.locations[0] != null &&
                PlacementManager.instance.locations[1] != null;
        }

        private bool CheckIfAlreadyRendered(int[] candidatePos)
        {
            //bool output = false;

            //for (int i = instancedMarks.Count - 1; i >= 0; i--)
            //{
            //    if ((int)instancedMarks[i].transform.position.x == candidatePos[0] && (int)instancedMarks[i].transform.position.z == candidatePos[1])
            //    {
            //        output = true;
            //        break;
            //    }
            //}

            return instancedMarks.Exists(x => ToIntArr(x.transform.position).Equals(candidatePos)) || forbiddenPos.Exists(x => x[0] == candidatePos[0] && x[1] == candidatePos[1]);
        }

        public void DrawExploredNodes(int threshold)
        {
            CleanGameObjectList(instancedMarks);
            int[][] operations = pathfinder.operations;

            //we draw the explored nodes
            if (exploredPositions.Count != 0)
            {
                for (int i = 0; i < Math.Min(exploredPositions.Count, threshold); i++)
                {
                    //instancedMarks.Add(Instantiate(plane));
                    //instancedMarks[instancedMarks.Count - 1].transform.position = ToVector3(exploredPositions[i])+ new Vector3(0,0.05f,0);
                    //instancedMarks[instancedMarks.Count - 1].transform.parent = this.transform;
                    //instancedMarks[instancedMarks.Count - 1].GetComponent<MeshRenderer>().material.color = Color.yellow;
                    Camera.main.GetComponent<DrawQuadGL>().DrawGLQuads(exploredPositions.GetRange(0, threshold), Color.yellow);
                }
                //draw the candidates, applying the operators to all of the 
                //explored nodes and making sure the positions are not occupied already
                for (int i = 0; i < Math.Min(exploredPositions.Count, threshold); i++)
                {
                    for (int j = 0; j < operations.Length; j++)
                    {
                        int[] candidatePos = new int[2] { exploredPositions[i][0] + operations[j][0], exploredPositions[i][1] + operations[j][1] };

                        if (!CheckIfAlreadyRendered(candidatePos))
                        {
                            //instancedMarks.Add(Instantiate(plane));
                            //instancedMarks[instancedMarks.Count - 1].transform.position = ToVector3(candidatePos)+ new Vector3(0, 0.01f, 0);
                            //instancedMarks[instancedMarks.Count - 1].GetComponent<MeshRenderer>().material.color = Color.green;
                        }

                    }
                }
            }
        }


        public void Simulate()
        {
            
            if (CheckConditions())
            {
                int maxX = PlacementManager.instance.maxX;
                int maxZ = PlacementManager.instance.maxZ;
                int[] initial = ToIntArr(PlacementManager.instance.locations[0].transform.position);
                int[] final = ToIntArr(PlacementManager.instance.locations[1].transform.position);

                Vector3[] obstaclePositions = PlacementManager.instance.GetObstaclePositions();
                forbiddenPos = TranslateForbiddenPositions(obstaclePositions);

                pathfinder = new Astar(maxX, maxZ, initial, final, forbiddenPos);
                if (pathfinder.simulate(heuristicMode, diagonalsCosts2, diagonalMode))
                {
                    Debug.Log("found a path");
                }
                else
                {
                    Debug.Log("didn't finda path");
                }
                this.exploredPositions = pathfinder.GetExploredPositions();
                slider.gameObject.GetComponent<SliderScript>().SetMaxValue(this.exploredPositions.Count);

            }
        }
    }
}