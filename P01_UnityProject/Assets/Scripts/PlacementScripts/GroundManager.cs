using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class GroundManager : MonoBehaviour
    {
        public List<GameObject> walls;
        public GameObject tile;
        public List<List<GameObject>> tiles;
        public float wallsHeight;

        public static GroundManager instance;

        void Start()
        {
            if (instance != null)
            {
                instance = this;
                tiles = new List<List<GameObject>>();
            }
        }

        private void CalculateWallsSize()
        {
            float x = PlacementManager.instance.maxX;
            float z = PlacementManager.instance.maxZ;

            walls[0].transform.position = new Vector3(x + 1f, 0f, z / 2f);
            walls[0].transform.localScale = new Vector3(walls[0].transform.localScale.x, walls[0].transform.localScale.y, walls[0].transform.localScale.z * z + 3);

            walls[1].transform.position = new Vector3(-1f, 0f, z / 2f);
            walls[1].transform.localScale = new Vector3(walls[1].transform.localScale.x, walls[1].transform.localScale.y, walls[1].transform.localScale.z * z + 3);

            walls[2].transform.position = new Vector3(x / 2f, 0f, z + 1f);
            walls[2].transform.localScale = new Vector3(walls[2].transform.localScale.x * x + 3, walls[2].transform.localScale.y, walls[2].transform.localScale.z);

            walls[3].transform.position = new Vector3(x / 2f, 0f, -1f);
            walls[3].transform.localScale = new Vector3(walls[3].transform.localScale.x * x + 3, walls[3].transform.localScale.y, walls[3].transform.localScale.z);
        }

        private void CalculateGroundPlacement()
        {
            int x = PlacementManager.instance.maxX;
            int z = PlacementManager.instance.maxZ;

            for (int i = 0; i <= x; i++)
            {
                for (int j = 0; j <= z; j++)
                {
                    tiles[i].Add(Instantiate(tile));
                    tiles[i][tiles.Count - 1].transform.position = new Vector3(i, 0f, j);
                    tiles[i][tiles.Count - 1].transform.parent = this.transform;
                }
            }
        }


        void Update()
        {
            if (PlacementManager.instance != null)
            {
                if (PlacementManager.instance.maxX != 0 && PlacementManager.instance.maxZ != 0)
                {
                    CalculateGroundPlacement();
                    CalculateWallsSize();
                    Destroy(this);
                }
            }
        }
    }
}