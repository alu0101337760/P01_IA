using System;
using UnityEngine;

namespace IA_sim
{
    public class EditWithMouse : MonoBehaviour
    {

        public bool AddObstacles = true;
        public bool SelectingInitialLocation = false;
        public bool SelectingFinalLocation = false;


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
                if (positions[i] == candidate || candidate.x > PlacementManager.instance.maxX || candidate.z > PlacementManager.instance.maxZ)
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

        public void SelectInitialLocation()
        {
            SelectingInitialLocation = true;
            AddObstacles = false;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3[] forbiddenPositions = PlacementManager.instance.GetObstaclePositions();
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
                    PlacementManager.instance.InstantiateInitialLocation(clickPosition);
                    SelectingInitialLocation = false;
                    
                }
            }
        }
        public void SelectFinalLocation()
        {
            SelectingFinalLocation = true;
            AddObstacles = false;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3[] forbiddenPositions = PlacementManager.instance.GetObstaclePositions();
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
                    PlacementManager.instance.InstantiateFinalLocation(clickPosition);
                    SelectingFinalLocation = false;
                }
            }
        }
        private void InstantiateWithMouse()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3[] forbiddenPositions = PlacementManager.instance.GetObstaclePositions();
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
                    PlacementManager.instance.InstantiateObstacle(clickPosition);
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
                        PlacementManager.instance.obstacles.Remove(target);
                        Destroy(target);
                    }
                }
            }
        }

        private void Update()
        {
            if (SelectingInitialLocation)
            {
                SelectInitialLocation();
            }
            else if (SelectingFinalLocation)
            {
                SelectFinalLocation();
            }
            else 
            {
                if (AddObstacles)
                {
                    InstantiateWithMouse();
                }
                else
                {
                    DeleteWithMouse();
                }
            }
        }
    }
}
