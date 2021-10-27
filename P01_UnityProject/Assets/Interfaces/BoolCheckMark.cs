using UnityEngine;
using UnityEngine.UI;

namespace IA_sim
{
    public class BoolCheckMark : MonoBehaviour
    {
        public bool diagonalMode;
        public bool showPath;
        public bool showExplored;

        void Start()
        {
            if (diagonalMode)
            {
                gameObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate
                {
                    PathfindingManager.instance.SetDiagonalMovement(gameObject.GetComponent<Toggle>().isOn);
                });
            }

            if (showPath)
            {
                gameObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate
                {
                    PathfindingManager.instance.SetShowPath(gameObject.GetComponent<Toggle>().isOn);
                });
            }
            if (showExplored)
            {
                gameObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate
                {
                    PathfindingManager.instance.SetShowExploredNodes(gameObject.GetComponent<Toggle>().isOn);
                });
            }
      
        }
    }
}