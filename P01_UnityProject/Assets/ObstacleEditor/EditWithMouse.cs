using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class EditWithMouse : MonoBehaviour
    {

        public bool AddObstacles = true;

        public void AddObstaclesIsTrue()
        {
            AddObstacles = true;
        }

        public void AddObstaclesIsFalse()
        {
            AddObstacles = false;
        }

        private bool CheckIfPositionIsValid(Vector3[] positions, Vector3 candidate)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i] == candidate)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckIfColliderIsValid(Collider collider)
        {
            if (collider != null)
            {
                return collider.gameObject.tag == "Ground" ? true : false;
            }
            return false;
        }

        private void InstantiateWithMouse()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3[] forbiddenPositions = ObstacleManager.instance.GetObstaclePositions();
                Vector3 clickPosition = new Vector3();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    clickPosition = hit.point;
                }

                clickPosition = new Vector3((int)Math.Round(clickPosition.x), (int)Math.Round(clickPosition.y), (int)Math.Round(clickPosition.z));

                if (CheckIfPositionIsValid(forbiddenPositions, clickPosition) && CheckIfColliderIsValid(hit.collider))
                {
                    ObstacleManager.instance.InstantiateObstacle(clickPosition);
                }
                else
                {
                }
            }
        }

        private void DeleteWithMouse()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Obstacle")
                    {
                        GameObject target = hit.collider.gameObject;
                        ObstacleManager.instance.obstacles.Remove(target);
                        Destroy(target);
                    }
                }
            }
        }

        void Update()
        {
            if (AddObstacles)
            {
                InstantiateWithMouse();
            }
            DeleteWithMouse();
        }
    }
}
