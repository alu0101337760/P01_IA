using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_sim
{
    public class PathfindingManager : MonoBehaviour
    {            
        private Pathfinder pathfinder;

        private void Start()
        {
            pathfinder = new Pathfinder();
        }

        public void Simulate()
        {

        }
    }
}