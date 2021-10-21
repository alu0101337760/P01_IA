using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class ObstacleManager : MonoBehaviour
    {
        public GameObject obstacle;
        public List<GameObject> obstacles;
        public int maxX;
        public int maxZ;
        public int obstaclePercentage;

        public static ObstacleManager instance;

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
            obstacles = new List<GameObject>();
        }

        public void SetObstaclePercentage(int p)
        {
            obstaclePercentage = p;
        }

        public void SetMaxX(string str)
        {
            if (maxX == 0)
            {
                maxX = int.Parse(str);
            }
        }
        public void SetMaxZ(string str)
        {
            if (maxZ == 0)
            {
                maxZ = int.Parse(str);
            }
        }
        public void InstantiateObstacle(Vector3 pos)
        {
            if (pos.x <= maxX && pos.z <= maxZ)
            {
                obstacles.Add(Instantiate(obstacle));
                Transform obstacleTransform = obstacles[obstacles.Count - 1].GetComponent<Transform>();
                obstacleTransform.position = pos;
            }
        }

        public void RandomSpawn()
        {
            List<Vector3> freeLocations = new List<Vector3>();
            int numberOfObstacles = CalculateNumberOfObstacles();
            int obstacleSum = 0;

            // we build a list with all the posible locations 
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxZ; j++)
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

        bool SpawnObstacleWithProbability(Vector3 pos)
        {
            int chance = (int)Random.Range(0, 101);

            if (chance < obstaclePercentage)
            {
                obstacles.Add(Instantiate(obstacle));

                Transform obstacleTransform = obstacles[obstacles.Count - 1].GetComponent<Transform>();
                obstacleTransform.position = pos;
                return true;

            }
            return false;
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

        void Update()
        {

        }
    }
}