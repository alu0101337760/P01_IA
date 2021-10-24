using UnityEngine;
using UnityEngine.UI;

namespace IA_sim {
    public class InputFieldScript : MonoBehaviour
    {
        public bool isMaxX = false;
       
        // Start is called before the first frame update
        void Start()
        {
            if (isMaxX)
            {
                gameObject.GetComponent<TMPro.TMP_InputField>().onEndEdit.AddListener(delegate
                {
                    PlacementManager.instance.SetMaxX(gameObject.GetComponent<TMPro.TMP_InputField>().text);    
                });
            }
            else
            {
                gameObject.GetComponent<TMPro.TMP_InputField>().onEndEdit.AddListener(delegate
                {
                    PlacementManager.instance.SetMaxZ(gameObject.GetComponent<TMPro.TMP_InputField>().text);
                });
            }
        }
    }
}