using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IA_sim
{
    public class TextScript : MonoBehaviour
    {
        public bool isNumberOfPath = false;
        public bool isNumberOfExpored = false;
        public bool isElapsedTime = false;
        private void Start()
        {
            if (isNumberOfExpored)
            {
                gameObject.GetComponent<Text>().text = "Number of explored nodes: 0";
            }
            else if (isNumberOfPath)
            {
                gameObject.GetComponent<Text>().text = "Number of nodes in path: 0";
            }
            else if (isElapsedTime)
            {
                gameObject.GetComponent<Text>().text = "Execution time: 0 ms";
            }
        }

        public void UpdateText(int value)
        {
            if (isNumberOfExpored)
            {
                gameObject.GetComponent<Text>().text = "Number of explored nodes: " + value;
            }
            else if (isNumberOfPath)
            {
                gameObject.GetComponent<Text>().text = "Number of nodes in path: " + value;
            }
            else if (isElapsedTime)
            {
                gameObject.GetComponent<Text>().text = "Execution time: " + value + " ms";
            }
        }

        public void UpdateText(double value)
        {
            gameObject.GetComponent<Text>().text = "Execution time: " + value + " ms";
        }
    }

}