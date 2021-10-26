using System;
using System.Collections.Generic;
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

        private bool CheckIfPositionIsValid(List<Vector3> positions, Vector3 candidate)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i] == candidate || candidate.x > GroundManager.instance.maxX || candidate.z > GroundManager.instance.maxZ)
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
                List<Vector3> forbiddenPositions = PlacementManager.instance.obstacles;
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
                List<Vector3> forbiddenPositions = PlacementManager.instance.obstacles;
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

                List<Vector3> forbiddenPositions = PlacementManager.instance.obstacles;
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
                        Vector3 targetPos = hit.collider.gameObject.transform.position;
                        PlacementManager.instance.DestroyObstacle(targetPos);
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
