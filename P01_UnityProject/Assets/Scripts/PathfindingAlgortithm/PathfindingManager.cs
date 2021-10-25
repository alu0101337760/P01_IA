using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace IA_sim
{
    public class PathfindingManager : MonoBehaviour
    {
        public GameObject yellowPlane;
        public GameObject GreenPlane;
        public List<int[]> exploredPositions;
        public static PathfindingManager instance;
        List<int[]> forbiddenPos;

        public Slider slider;

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


        private int[] TranslatePositions(Vector3 pos)
        {
            return new int[2] { (int)pos.x, (int)pos.z };
        }


        private List<int[]> TranslateForbiddenPositions(Vector3[] forbiddenPositions)
        {
            List<int[]> output = new List<int[]>();
            for (int i = 0; i < forbiddenPositions.Length; i++)
            {
                output.Add(TranslatePositions(forbiddenPositions[i]));
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


        public void DrawExploredNodes(int threshold)
        {
            CleanGameObjectList(instancedMarks);
            int[][] operations = pathfinder.operations;

            //we draw the explored nodes
            if (exploredPositions.Count != 0)
            {
                for (int i = 0; i < Math.Min(exploredPositions.Count, threshold); i++)
                {
                    instancedMarks.Add(Instantiate(yellowPlane));
                    instancedMarks[instancedMarks.Count - 1].transform.position = new Vector3(exploredPositions[i][0], 0.1f, exploredPositions[i][1]);

                    //draw the candidates, applying the operators to all of the 
                    //explored nodes and making sure the positions are not occupied already
                    for (int j = 0; j < operations.Length; j++)
                    {
                        int[] candidatePos = new int[2] { exploredPositions[i][0] + operations[j][0], exploredPositions[i][1] + operations[j][1] };

                        if (instancedMarks.Exists(instancedMark => forbiddenPos.Exists(x => x[0] == candidatePos[0] && x[1] == candidatePos[1])))
                        {
                            continue;
                        }
                        else
                        {
                            instancedMarks.Add(Instantiate(GreenPlane));
                            instancedMarks[instancedMarks.Count - 1].transform.position = new Vector3(candidatePos[0], 0.75f, candidatePos[1]);
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
                int[] initial = TranslatePositions(PlacementManager.instance.locations[0].transform.position);
                int[] final = TranslatePositions(PlacementManager.instance.locations[1].transform.position);

                Vector3[] obstaclePositions = PlacementManager.instance.GetObstaclePositions();
                forbiddenPos = TranslateForbiddenPositions(obstaclePositions);

                pathfinder = new Astar(maxX, maxZ, initial, final, forbiddenPos);
                if (pathfinder.simulate())
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