using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class GroundManager : MonoBehaviour
    {
        public int maxX = 0;
        public int maxZ = 0;

        public List<GameObject> walls;
        public GameObject tile;
        public List<List<GameObject>> tiles;
        public float wallsHeight;

        private bool built = false;

        public static GroundManager instance;

        void Start()
        {
            if (instance == null)
            {
                instance = this;
                tiles = new List<List<GameObject>>();
                Build();

            }
        }

        private void Build()
        {
            if (maxZ != 0 && maxX != 0 && !built)
            {
                CalculateGroundPlacement();
                CalculateWallsSize();
                built = true;
            }
        }

        public void SetMaxX(string str)
        {
            Debug.Log(str);
            if (maxX == 0)
            {
                maxX = int.Parse(str);
                Build();
            }
        }

        public void SetMaxZ(string str)
        {
            Debug.Log(str);
            if (maxZ == 0)
            {
                maxZ = int.Parse(str);
                Build();
            }
        }

        private void CalculateWallsSize()
        {
            walls[0].transform.position = new Vector3(maxX + 1f, 0f, maxZ / 2f);
            walls[0].transform.localScale = new Vector3(walls[0].transform.localScale.x, walls[0].transform.localScale.y, walls[0].transform.localScale.z * maxZ + 3);

            walls[1].transform.position = new Vector3(-1f, 0f, maxZ / 2f);
            walls[1].transform.localScale = new Vector3(walls[1].transform.localScale.x, walls[1].transform.localScale.y, walls[1].transform.localScale.z * maxZ + 3);

            walls[2].transform.position = new Vector3(maxX / 2f, 0f, maxZ + 1f);
            walls[2].transform.localScale = new Vector3(walls[2].transform.localScale.x * maxX + 3, walls[2].transform.localScale.y, walls[2].transform.localScale.z);

            walls[3].transform.position = new Vector3(maxX / 2f, 0f, -1f);
            walls[3].transform.localScale = new Vector3(walls[3].transform.localScale.x * maxX + 3, walls[3].transform.localScale.y, walls[3].transform.localScale.z);
        }

        private void CalculateGroundPlacement()
        {
            for (int i = 0; i <= maxX; i++)
            {
                tiles.Add(new List<GameObject>());
                for (int j = 0; j <= maxZ; j++)
                {
                    tiles[i].Add(Instantiate(tile));
                    tiles[i][tiles[i].Count - 1].transform.position = new Vector3(i, 0f, j);
                    tiles[i][tiles[i].Count - 1].transform.parent = this.transform;
                    tiles[i][tiles[i].Count - 1].GetComponent<MeshRenderer>().material.color = Color.gray;
                }
            }
        }
    }
}
