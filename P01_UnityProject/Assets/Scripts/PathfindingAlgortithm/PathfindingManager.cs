using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class PathfindingManager : MonoBehaviour
    {
        public GameObject exploredNode;
        private Astar pathfinder;
                       
        public List<GameObject> exploredPositions;

        public static PathfindingManager instance;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
             
        private void CleanGameObjectList(List<GameObject> list)
        {
            for (int i= 0; i < list.Count; i++)
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

        public void DrawExploredNodes(List<int[]> exploredPositions)
        {
            CleanGameObjectList(this.exploredPositions);
            for (int i = 0; i < exploredPositions.Count; i++)
            {
                this.exploredPositions.Add(Instantiate(exploredNode));
                this.exploredPositions[i].transform.position = new Vector3(exploredPositions[i][0], 0.1f, exploredPositions[i][1]);
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
                List<int[]> forbiddenPos = TranslateForbiddenPositions(obstaclePositions);

                pathfinder = new Astar(maxX, maxZ, initial, final, forbiddenPos);
                if (pathfinder.simulate())
                {
                    Debug.Log("found a path");
                }
                else
                {
                    Debug.Log("didn't finda path");
                }                
            }
        }
    }
}