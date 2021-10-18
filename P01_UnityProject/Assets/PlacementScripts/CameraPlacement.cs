using System;
using UnityEngine;

namespace IA_sim
{
    public class CameraPlacement : MonoBehaviour
    {
        private void Start()
        {

        }
        private void Update()
        {
            if (ObstacleManager.instance != null)
            {
                CalculateCameraHeight();
                Destroy(this);
            }
        }
        public void CalculateCameraHeight()
        {
            //float halfMaxSide = (Math.Max(ObstacleManager.instance.maxX, ObstacleManager.instance.maxZ)/2);

            //float h = (float)(halfMaxSide / (Math.Sin(gameObject.GetComponent<Camera>().fieldOfView)/2));

            //float height = (float)Math.Sqrt((h * h) - (halfMaxSide * halfMaxSide));

            gameObject.transform.position = new Vector3((ObstacleManager.instance.maxX / 2f - .5f) * .1f, gameObject.transform.position.y, (ObstacleManager.instance.maxZ / 2f - .5f) * .1f);

          
        }
    }
}