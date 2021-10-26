using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class PlacementManager : MonoBehaviour
    {
        public GameObject taxi;
        public GameObject finalLocation;

        public GameObject[] locations;
        public List<Vector3> obstacles;
        public int obstaclePercentage;

        public static PlacementManager instance;

        // Start is called before the first frame update
        void Start()
        {
            if (instance == null)
            {
                build();
            }
        }

        private void build()
        {
            instance = this;
            locations = new GameObject[2];
            obstacles = new List<Vector3>();
        }

        public void SetObstaclePercentage(int p)
        {
            Debug.Log(p);
            obstaclePercentage = p;
        }
           
       
        public void InstantiateObstacle(Vector3 pos)
        {
            obstacles.Add(pos);
            GroundManager.instance.tiles[(int)pos.x][(int)pos.z].GetComponent<MeshRenderer>().material.color = Color.red;
            GroundManager.instance.tiles[(int)pos.x][(int)pos.z].tag = "Obstacle";
        }

        public void DestroyObstacle(Vector3 pos)
        {
            GroundManager.instance.tiles[(int)pos.x][(int)pos.z].GetComponent<MeshRenderer>().material.color = Color.gray;
            GroundManager.instance.tiles[(int)pos.x][(int)pos.z].tag = "Ground";
            obstacles.Remove(pos);
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
                GroundManager.instance.tiles[(int)obstacles[i].x][(int)obstacles[i].z].GetComponent<MeshRenderer>().material.color = Color.gray;
                GroundManager.instance.tiles[(int)obstacles[i].x][(int)obstacles[i].z].tag = "Ground";
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
            for (int i = 0; i <= GroundManager.instance.maxX; i++)
            {
                for (int j = 0; j <= GroundManager.instance.maxZ; j++)
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
            int matrixsize = GroundManager.instance.maxX * GroundManager.instance.maxZ;
            return (int)((matrixsize * obstaclePercentage) / 100);
        }         
    }
}