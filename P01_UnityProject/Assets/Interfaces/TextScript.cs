using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IA_sim
{
    public class TextScript : MonoBehaviour
    {
        public bool isNumberOfExpored = false;
        private void Start()
        {
            if (isNumberOfExpored)
            {
                gameObject.GetComponent<Text>().text = "Number of explored nodes:";
            }
            else
            {
                gameObject.GetComponent<Text>().text = "Number of nodes in path:";
            }
        }

        public void UpdateText(int value)
        {
            if (isNumberOfExpored)
            {
                gameObject.GetComponent<Text>().text = "Number of explored nodes: " + value;
            }
            else
            {
                gameObject.GetComponent<Text>().text = "Number of nodes in path: " + value;
            }
        }
    }

}