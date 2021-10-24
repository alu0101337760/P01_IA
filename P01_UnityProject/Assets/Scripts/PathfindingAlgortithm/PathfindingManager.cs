using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class PathfindingManager : MonoBehaviour
    {
        public GameObject taxi;
        public int finalX;
        public int finalZ;
        public int maxX;
        public int maxZ;
        

        private Pathfinder pathfinder;

        private void Start()
        {
            pathfinder = new Pathfinder();
        }

        public void Simulate()
        {
            this.maxX = PlacementManager.instance.maxX;
            this.maxZ = PlacementManager.instance.maxZ;

        }
    }
}