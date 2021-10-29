using UnityEngine;
using UnityEngine.UI;

namespace IA_sim
{
    public class BoolCheckMark : MonoBehaviour
    {
        public bool diagonalMode;
      
        void Start()
        {
            if (diagonalMode)
            {
                gameObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate
                {
                    PathfindingManager.instance.SetDiagonalMovement(gameObject.GetComponent<Toggle>().isOn);
                });
            }      
        }
    }
}