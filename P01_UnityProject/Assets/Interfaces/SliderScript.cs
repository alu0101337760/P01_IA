using UnityEngine.UI;
using UnityEngine;

namespace IA_sim
{
    public class SliderScript : MonoBehaviour
    {

        private void Start()
        {
            gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate
            {
                PlacementManager.instance.SetObstaclePercentage((int)gameObject.GetComponent<Slider>().value);
            });
        }
    }
}