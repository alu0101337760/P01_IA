using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class GroundPlacement : MonoBehaviour
    {
        public List<GameObject> walls;

        public float wallsHeight;
            

        private void CalculateWallsSize()
        {
            float x = PlacementManager.instance.maxX;
            float z = PlacementManager.instance.maxZ;

            walls[0].transform.position = gameObject.transform.position + new Vector3(x / 2f + 1f, 0f, 0f);
            walls[0].transform.localScale = new Vector3(walls[0].transform.localScale.x, walls[0].transform.localScale.y, walls[0].transform.localScale.z * z + 3);

            walls[1].transform.position = gameObject.transform.position + new Vector3(-x / 2f - 1f, 0f, 0f);
            walls[1].transform.localScale = new Vector3(walls[1].transform.localScale.x, walls[1].transform.localScale.y, walls[1].transform.localScale.z * z + 3);

            walls[2].transform.position = gameObject.transform.position + new Vector3(0f, 0f, z / 2f + 1f);
            walls[2].transform.localScale = new Vector3(walls[2].transform.localScale.x * x + 3, walls[2].transform.localScale.y, walls[2].transform.localScale.z);

            walls[3].transform.position = gameObject.transform.position + new Vector3(0f, 0f, -z / 2f - 1f);
            walls[3].transform.localScale = new Vector3(walls[3].transform.localScale.x * x + 3, walls[3].transform.localScale.y, walls[3].transform.localScale.z);
        }

        private void CalculateGroundPlacement()
        {
            int x = PlacementManager.instance.maxX;
            int z = PlacementManager.instance.maxZ;
            Vector3 scale = gameObject.transform.localScale;

            scale.x *= x + 1;
            scale.z *= z + 1;

            gameObject.transform.localScale = scale;

            gameObject.transform.position = new Vector3(((x / 2f)), gameObject.transform.position.y, ((z / 2f)));
        }

        public void ResetGround() {
            gameObject.transform.position = new Vector3(0, 0, 0);
            gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            walls[0].transform.position = new Vector3(1,0,0);
            walls[0].transform.localScale = new Vector3(1, 1, 1);
            walls[1].transform.position = new Vector3(-1,0,0);
            walls[1].transform.localScale = new Vector3(1, 1, 1);
            walls[2].transform.position = new Vector3(0,0,1);
            walls[2].transform.localScale = new Vector3(1, 1, 1);
            walls[3].transform.position = new Vector3(0,0,-1);
            walls[3].transform.localScale = new Vector3(1, 1, 1);
        }
        
        public void CalculateTerrain()
        {
            CalculateGroundPlacement();
            CalculateWallsSize();
        }

    }
}