using UnityEngine.UI;
using UnityEngine;

namespace IA_sim
{
    public class SliderScript : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate
            {
                ObstacleManager.instance.SetObstaclePercentage((int)gameObject.GetComponent<Slider>().value);
            });
        }
    }
}