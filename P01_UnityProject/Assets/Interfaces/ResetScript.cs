using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IA_sim
{
    public class ResetScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                PlacementManager.instance.ResetManager();
                PathfindingManager.instance.ResetManager();
            }
            );
        }
    }
}