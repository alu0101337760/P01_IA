using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class PlacementManager : MonoBehaviour
    {
        public GameObject obstacle;
        public GameObject taxi;
        public GameObject finalLocation;

        public GameObject[] locations;
        public List<GameObject> obstacles;

        public int maxX;
        public int maxZ;
        public int obstaclePercentage;

        public static PlacementManager instance;

        // Start is called before the first frame update
        void Start()
        {
            if (instance == null)
            {
                build();
            }
            RandomSpawn();
        }

        private void build()
        {
            instance = this;
            locations = new GameObject[2];
            obstacles = new List<GameObject>();
        }

        public void SetObstaclePercentage(int p)
        {
            Debug.Log(p);
            obstaclePercentage = p;
        }

        public void SetMaxX(string str)
        {
            Debug.Log(str);
            if (maxX == 0)
            {
                maxX = int.Parse(str);
            }
        }
        public void SetMaxZ(string str)
        {
            Debug.Log(str);
            if (maxZ == 0)
            {
                maxZ = int.Parse(str);
            }
        }
        public void InstantiateObstacle(Vector3 pos)
        {
            obstacles.Add(Instantiate(obstacle));
            Transform obstacleTransform = obstacles[obstacles.Count - 1].GetComponent<Transform>();
            obstacleTransform.position = pos;
            obstacleTransform.parent = this.transform;
        }

        public void InstantiateInitialLocation(Vector3 pos)
        {
            if (locations[0] != null)
            {                
                Destroy(locations[0]);
            }
            pos = new Vector3(pos.x, 0.2f, pos.z);
            locations[0] = Instantiate(taxi);
            Transform transform = locations[0].transform;
            transform.position = pos;

        }

        public void InstantiateFinalLocation(Vector3 pos)
        {
            if (locations[1] != null)
            {
                Destroy(locations[1]);
            }
            locations[1] = Instantiate(finalLocation);
            Transform transform = locations[1].transform;
            transform.position = pos;

        }


        private void DestroyAllObstacles()
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                GameObject toDelete = obstacles[i];
                Destroy(toDelete);
            }
            obstacles.Clear();
        }

        public void RandomSpawn()
        {
            DestroyAllObstacles();
            List<Vector3> freeLocations = new List<Vector3>();
            int numberOfObstacles = CalculateNumberOfObstacles();
            int obstacleSum = 0;

            // we build a list with all the posible locations 
            for (int i = 0; i <= maxX; i++)
            {
                for (int j = 0; j <= maxZ; j++)
                {
                    freeLocations.Add(new Vector3(i, 0, j));
                }
            }

            //we randomly access to the elments of the list, instantiating an obstacle
            //and removing that element from the list.
            while (obstacleSum < numberOfObstacles)
            {
                int index = Random.Range(0, freeLocations.Count);
                InstantiateObstacle(freeLocations[index]);
                freeLocations.RemoveAt(index);
                obstacleSum++;
            }
        }

        private int CalculateNumberOfObstacles()
        {
            int matrixsize = maxX * maxZ;
            return (int)((matrixsize * obstaclePercentage) / 100);
        }

        public Vector3[] GetLocations() { 
            Vector3[] output = new Vector3[2];

            output[0] = locations[0].transform.position;
            output[1] = locations[1].transform.position;

            return output;
        }

        public Vector3[] GetObstaclePositions()
        {
            Vector3[] output = new Vector3[obstacles.Count];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = obstacles[i].transform.position;
            }
            return output;
        }
    }
}