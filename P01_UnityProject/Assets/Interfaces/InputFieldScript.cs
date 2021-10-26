using UnityEngine;
using UnityEngine.UI;

namespace IA_sim
{
    public class InputFieldScript : MonoBehaviour
    {
        public bool isMaxX = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        private void Update()
        {
            if (GroundManager.instance != null)
            {
                if (isMaxX)
                {
                    gameObject.GetComponent<TMPro.TMP_InputField>().onEndEdit.AddListener(delegate
                    {
                        GroundManager.instance.SetMaxX(gameObject.GetComponent<TMPro.TMP_InputField>().text);
                    });
                    Destroy(this);
                }
                else
                {
                    gameObject.GetComponent<TMPro.TMP_InputField>().onEndEdit.AddListener(delegate
                    {
                        GroundManager.instance.SetMaxZ(gameObject.GetComponent<TMPro.TMP_InputField>().text);
                    });
                    Destroy(this);
                }
            }
        }
    }
}