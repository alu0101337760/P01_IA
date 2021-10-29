using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace IA_sim
{
    public class PathfindingManager : MonoBehaviour
    {
        public GameObject plane;
        public Slider slider;
        public TextScript nOfNodesInPath;
        public TextScript nOfNodes;

        List<int[]> forbiddenPos;
        List<int[]> exploredPositions;
        List<int[]> path;
        private List<GameObject> instancedMarks;


        public bool diagonalMode = false;
        public Astar.HeuristicMode heuristicMode = Astar.HeuristicMode.Manhattan;


        private Astar pathfinder;
        public long ElapsedMillisecond;
        public static PathfindingManager instance;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                instancedMarks = new List<GameObject>();
            }
        }

        public void ResetManager()
        {
            ClearInstancedMarks();
            forbiddenPos.Clear();
            path.Clear();
            nOfNodesInPath.UpdateText(0);
            exploredPositions.Clear();
            nOfNodes.UpdateText(0);
        }

        public void SetManhattan()
        {
            heuristicMode = Astar.HeuristicMode.Manhattan;
        }

        public void SetEuclid()
        {
            heuristicMode = Astar.HeuristicMode.Euclid;
        }

        public void SetDiagonalMovement(bool value)
        {
            diagonalMode = value;
        }

        private void ClearInstancedMarks()
        {
            for (int i = 0; i < instancedMarks.Count; i++)
            {
                GameObject target = instancedMarks[i];
                Destroy(target);
            }
            instancedMarks.Clear();
        }

        private void DeactivateInstancedMarks(int min)
        {
            for (int i = min; i < instancedMarks.Count; i++)
            {
                instancedMarks[i].SetActive(false);

            }
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


        private void InstantiateExploredNodes()
        {
            ClearInstancedMarks();
            for (int i = 0; i < exploredPositions.Count; i++)
            {
                instancedMarks.Add(Instantiate(plane));
                instancedMarks[instancedMarks.Count - 1].transform.position = ToVector3(exploredPositions[i]) + new Vector3(0, 0.05f, 0);
                instancedMarks[instancedMarks.Count - 1].transform.parent = this.transform;
                if (path.Exists(x => x[0] == exploredPositions[i][0] && x[1] == exploredPositions[i][1]))
                {
                    instancedMarks[instancedMarks.Count - 1].GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else
                {
                    instancedMarks[instancedMarks.Count - 1].GetComponent<MeshRenderer>().material.color = Color.yellow;
                }
                instancedMarks[instancedMarks.Count - 1].SetActive(false);
            }
        }

        public void DrawExploredNodes(int threshold)
        {
            DeactivateInstancedMarks(threshold + 1);
            nOfNodes.UpdateText(threshold);
            if (exploredPositions.Count != 0)
            {
                for (int i = 0; i < Math.Min(exploredPositions.Count, threshold); i++)
                {
                    instancedMarks[i].SetActive(true);
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
                if (pathfinder.simulate(heuristicMode, diagonalMode))
                {
                    Debug.Log("found a path");
                }
                else
                {
                    Debug.Log("didn't finda path");
                }
                this.exploredPositions = pathfinder.GetExploredPositions();
                slider.gameObject.GetComponent<SliderScript>().SetMaxValue(this.exploredPositions.Count);

                ElapsedMillisecond = pathfinder.GetElapsedMiliseconds();
                path = pathfinder.BacktrackSolution();
                nOfNodesInPath.UpdateText(path.Count);
                InstantiateExploredNodes();


            }
        }
    }
}